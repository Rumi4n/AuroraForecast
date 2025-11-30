using AuroraForecast.Locations.Models;

namespace AuroraForecast.Aurora.Models
{
    public class AuroraForecast
    {
        public Location Location { get; set; } = null!;

        public List<AuroraDayForecast> DayForecasts { get; set; } = new();
    }
}
