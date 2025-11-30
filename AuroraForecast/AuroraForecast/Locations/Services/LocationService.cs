using AuroraForecast.Locations.Interfaces;
using AuroraForecast.Locations.Models;
using AuroraForecast.User.Models;

namespace AuroraForecast.Locations.Services;

class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<IEnumerable<Location>> GetUserLocationsAsync(int userId)
    {
        return await _locationRepository.GetUserLocationsAsync(userId).ConfigureAwait(false);
    }

    public async Task<Location> AddAsync(Location location)
    {
        return await _locationRepository.AddAsync(location).ConfigureAwait(false);
    }

    public async Task DeleteAsync(int locationId)
    {
        await _locationRepository.DeleteAsync(locationId).ConfigureAwait(false);
    }
}