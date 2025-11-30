using AuroraForecast.Aurora.Models;
using AuroraForecast.Locations.Models;

namespace AuroraForecast.Aurora.Interfaces;

internal interface IAuroraApiWrapper
{
    Task<AuroraApiResponse> GetAuroraDataAsync(Location location);
}