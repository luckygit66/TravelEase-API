using System.Net.Http;
using System.Threading.Tasks;
using TravelEaseApi.Models; // ✅ This is required for ApiSettings

namespace TravelEaseApi.Services
{
    public class FlightAggregatorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken;

        public FlightAggregatorService(ApiSettings apiSettings)
        {
            _httpClient = new HttpClient();
            _apiToken = apiSettings.TravelPayoutsApiToken;
            Console.WriteLine($"Travelpayouts API Token: {_apiToken}");
        }

        public async Task<string> SearchFlightsAsync(string from, string to, string date)
        {
            var url = $"https://api.travelpayouts.com/v1/prices/cheap?origin={from}&destination={to}&depart_date={date}&currency=usd&token={_apiToken}";

            Console.WriteLine($"Calling URL: {url}");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed with status code: {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
