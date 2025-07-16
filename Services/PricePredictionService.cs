using TravelEaseApi.Models;

namespace TravelEaseApi.Services
{
    public class PricePredictionService
    {
        public PricePredictionResult PredictPrice(FlightQuery query)
        {
            var basePrice = 10000;
            var random = new Random();
            var variation = random.Next(2000, 5000);

            return new PricePredictionResult
            {
                From = query.From ?? "Unknown",
                To = query.To ?? "Unknown",
                Month = query.Month ?? "Unknown",
                PredictedPriceMin = basePrice,
                PredictedPriceMax = basePrice + variation
            };
        }
    }
}
