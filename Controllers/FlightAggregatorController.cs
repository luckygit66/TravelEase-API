using Microsoft.AspNetCore.Mvc;
using TravelEaseApi.Services; // 🔑 Required to find FlightAggregatorService

namespace TravelEaseApi.Controllers // 🔑 Namespace required!
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightAggregatorController : ControllerBase
    {
        private readonly FlightAggregatorService _flightAggregatorService;

        public FlightAggregatorController(FlightAggregatorService flightAggregatorService)
        {
            _flightAggregatorService = flightAggregatorService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string from, string to, string date)
        {
            var result = await _flightAggregatorService.SearchFlightsAsync(from, to, date);
            if (result == null)
            {
                return BadRequest("Failed to fetch flights.");
            }

            return Ok(result);
        }
    }
}
