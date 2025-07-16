using System.Net.Http;
using System.Threading.Tasks;
using TravelEaseApi.Models; // ✅ For ApiSettings

namespace TravelEaseApi.Services
{
    public class FlightSearchService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken;
        private readonly string _marker;

        public FlightSearchService(HttpClient httpClient, ApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiToken = apiSettings.TravelPayoutsApiToken;
            _marker = apiSettings.TravelPayoutsMarker;
        }

        public async Task<string> SearchFlightsAsync(string from, string to, string departDate)
        {
            string requestUrl = $"https://api.travelpayouts.com/aviasales/v3/prices_for_dates?origin={from}&destination={to}&departure_at={departDate}&token={_apiToken}&marker={_marker}";

            var response = await _httpClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
