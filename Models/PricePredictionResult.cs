namespace TravelEaseApi.Models
{
    public class PricePredictionResult
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Month { get; set; }
        public int PredictedPriceMin { get; set; }
        public int PredictedPriceMax { get; set; }
    }
}
