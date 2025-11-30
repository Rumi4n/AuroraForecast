using AuroraForecast.Locations.Models;

namespace AuroraForecast.Locations.Interfaces;

internal interface ILocationRepository
{
    Task<IEnumerable<Location>> GetUserLocationsAsync(int userId);
    Task<Location> AddAsync(Location location);
    Task DeleteAsync(int locationId);
}