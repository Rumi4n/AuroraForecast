namespace AuroraForecast.User.Interfaces;

internal interface IUserRepository
{
    Models.User GetUserById(int id);
    Models.User AddUser(Models.User user);
}