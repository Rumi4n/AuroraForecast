using AuroraForecast.Data;
using AuroraForecast.User.Interfaces;

namespace AuroraForecast.User.Repositories;

class UserRepository : IUserRepository
{
    private readonly AuroraDbContext _context;

    public UserRepository(AuroraDbContext context)
    {
        _context = context;
    }

    public Models.User GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id) 
            ?? throw new KeyNotFoundException($"User with ID {id} not found.");
    }

    public Models.User AddUser(Models.User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
}