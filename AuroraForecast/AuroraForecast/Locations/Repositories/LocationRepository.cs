using AuroraForecast.Data;
using AuroraForecast.Locations.Interfaces;
using AuroraForecast.Locations.Models;
using Microsoft.EntityFrameworkCore;

namespace AuroraForecast.Locations.Repositories;

class LocationRepository : ILocationRepository
{
    private readonly AuroraDbContext _context;

    public LocationRepository(AuroraDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Location>> GetUserLocationsAsync(int userId)
    {
        return await _context.Locations.Where(u => u.UserId == userId).ToListAsync();
    }

    public async Task<Location> AddAsync(Location location)
    {
        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task DeleteAsync(int locationId)
    {
        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);

        if(location == null)
        {
            return;
        }

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();
    }
}