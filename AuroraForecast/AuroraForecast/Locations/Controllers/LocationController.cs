using AuroraForecast.Locations.Interfaces;
using AuroraForecast.Locations.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuroraForecast.Locations.Controllers
{
    [ApiController]
    [Route("location")]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;
        private ILocationService _locationService;

        public LocationController(ILogger<LocationController> logger, ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }

        [HttpGet("user/{userId}", Name = "GetUserLocationsAsync")]
        public Task<IEnumerable<Location>> GetUserLocationsAsync(int userId)
        {
            return _locationService.GetUserLocationsAsync(userId);
        }

        [HttpPut("", Name = "AddLocationAsync")]
        public Task<Location> AddLocationAsync(Location location)
        {
            return _locationService.AddAsync(location);
        }


        [HttpDelete("{id}", Name = "DeleteLocationsAsync")]
        public async Task DeleteLocationsAsync(int locationId)
        {
            await _locationService.DeleteAsync(locationId);
        }
    }
}
