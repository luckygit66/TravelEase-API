using Microsoft.AspNetCore.Mvc;
using TravelEaseApi.Services;
using TravelEaseApi.Models;

namespace TravelEaseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightPricePredictionController : ControllerBase
    {
        private readonly PricePredictionService _pricePredictionService;

        public FlightPricePredictionController(PricePredictionService pricePredictionService)
        {
            _pricePredictionService = pricePredictionService;
        }

        [HttpPost("predict")]
        public IActionResult PredictPrice([FromBody] FlightQuery query)
        {
            if (query == null)
                return BadRequest("Invalid flight query.");

            var prediction = _pricePredictionService.PredictPrice(query);
            return Ok(prediction);
        }
    }
}
