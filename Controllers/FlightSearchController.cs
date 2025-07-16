using Microsoft.AspNetCore.Mvc;
using TravelEaseApi.Services;
using TravelEaseApi.Models;

namespace TravelEaseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightSearchController : ControllerBase
    {
        private readonly GPTParserService _gptParserService;
        private readonly ILogger<FlightSearchController> _logger;

        public FlightSearchController(GPTParserService gptParserService, ILogger<FlightSearchController> logger)
        {
            _gptParserService = gptParserService;
            _logger = logger;
        }

        [HttpPost("parse")]
        public async Task<IActionResult> ParseFlightQuery([FromBody] string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                _logger.LogWarning("Empty user input received.");
                return BadRequest("Input cannot be empty.");
            }

            try
            {
                _logger.LogInformation("Processing flight search input...");
                var query = await _gptParserService.ParseInputAsync(userInput);

                if (query == null)
                {
                    _logger.LogError("GPTParserService returned null.");
                    return StatusCode(500, "Failed to parse the input.");
                }

                return Ok(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in ParseFlightQuery");
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }
    }
}
