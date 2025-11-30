using AuroraForecast.Aurora.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuroraForecast.Aurora.Controllers
{
    [ApiController]
    [Route("aurora")]
    public class AuroraController : ControllerBase
    {
        private readonly ILogger<AuroraController> _logger;
        private readonly IAuroraService _auroraService;

        public AuroraController(ILogger<AuroraController> logger, IAuroraService auroraService)
        {
            _logger = logger;
            _auroraService = auroraService;
        }

        [HttpGet("forecast/user/{userId}", Name = "GetAsync")]
        public async Task<ObjectResult> GetAsync(int userId)
        {
            try
            {
                var forecasts = await _auroraService.GetForecastsAsync(userId);
                return Ok(forecasts);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch aurora data for user {UserId}", userId);
                return StatusCode(503, "Unable to fetch aurora data from external service");
            }
        }
    }
}
