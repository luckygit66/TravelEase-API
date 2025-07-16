using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using TravelEaseApi.Models;

namespace TravelEaseApi.Services
{
    public class GPTParserService
    {
        private readonly OpenAIService _openAI;
        private readonly ILogger<GPTParserService> _logger;

        public GPTParserService(IConfiguration config, ILogger<GPTParserService> logger)
        {
            _logger = logger;
            _openAI = new OpenAIService(new OpenAiOptions
            {
                ApiKey = config["OpenAI:ApiKey"]
            });
        }

        public async Task<FlightQuery> ParseInputAsync(string userInput)
        {
            _logger.LogInformation("Received user input for parsing: {UserInput}", userInput);

           var prompt = $@"
Extract the following details from this user input: '{userInput}'
Return JSON ONLY in this format:

{{
    ""From"": ""source city"",
    ""To"": ""destination city"",
    ""Month"": ""month of travel"",
    ""MaxBudget"": numeric max budget or null
}}

Example:
Input: 'Find me a flight from Delhi to Dubai next month under 20000'
Output:
{{
    ""From"": ""Delhi"",
    ""To"": ""Dubai"",
    ""Month"": ""next month"",
    ""MaxBudget"": 20000
}}

Respond ONLY with this JSON format.";


            var chatRequest = new ChatCompletionCreateRequest
            {
                Model = OpenAI.ObjectModels.Models.Gpt_4,
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a travel NLP assistant."),
                    ChatMessage.FromUser(prompt)
                }
            };

            try
            {
                var result = await _openAI.ChatCompletion.CreateCompletion(chatRequest);

                if (result.Choices == null || !result.Choices.Any())
                {
                    _logger.LogWarning("OpenAI returned no choices.");
                    return null;
                }

                var json = result.Choices.First().Message.Content;
                _logger.LogDebug("Raw GPT response: {Json}", json);

                var query = JsonConvert.DeserializeObject<FlightQuery>(json);
                _logger.LogInformation("Successfully parsed FlightQuery: {@Query}", query);

                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing user input via OpenAI");
                return null;
            }
        }
    }
}
