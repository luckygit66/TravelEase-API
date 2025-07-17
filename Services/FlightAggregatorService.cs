using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TravelEaseApi.Models; // ✅ Keep this at the top with other usings

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
            // 🔧 MOCKED DATA INSTEAD OF REAL API CALL
            var mockResponse = new
            {
                data = new Dictionary<string, object>
                {
                    {
                        "SIN", new Dictionary<string, object>
                        {
                            {
                                "0", new {
                                    airline = "QZ",
                                    departure_at = "2025-08-10T00:05:00+07:00",
                                    return_at = "2025-08-19T17:55:00+08:00",
                                    expires_at = "2025-07-17T09:04:12Z",
                                    price = 204,
                                    flight_number = 249
                                }
                            }
                        }
                    },
                    {
                        "PAR", new Dictionary<string, object>
                        {
                            {
                                "0", new {
                                    airline = "VY",
                                    departure_at = "2025-09-02T19:50:00+01:00",
                                    return_at = "2025-09-06T13:40:00+02:00",
                                    expires_at = "2025-07-17T09:06:09Z",
                                    price = 102,
                                    flight_number = 8945
                                }
                            }
                        }
                    }
                },
                currency = "usd",
                success = true
            };

            var json = JsonSerializer.Serialize(mockResponse);
            return await Task.FromResult(json);
        }
    }
}
