using AuroraForecast.User.Interfaces;

namespace AuroraForecast.User.Services;

class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Models.User GetUserById(int id)
    {
        return _userRepository.GetUserById(id);
    }

    public Models.User GenerateUser()
    {
        Models.User user = new Models.User
        {
            Name = "GeneratedUser_" + Guid.NewGuid()
        };
        return _userRepository.AddUser(user);
    }
}