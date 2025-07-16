using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TravelEaseApi.Services
{
    public class FlightSearchService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FlightSearchService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> SearchFlightsAsync(string from, string to, string departDate)
        {
            var token = _configuration["TravelPayouts:Token"];
            var marker = _configuration["TravelPayouts:Marker"];

            string requestUrl = $"https://api.travelpayouts.com/aviasales/v3/prices_for_dates?origin={from}&destination={to}&departure_at={departDate}&token={token}&marker={marker}";

            var response = await _httpClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
