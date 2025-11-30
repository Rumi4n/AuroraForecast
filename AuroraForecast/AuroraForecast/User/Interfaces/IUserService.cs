namespace AuroraForecast.User.Interfaces;

public interface IUserService
{
    Models.User GetUserById(int id);
    Models.User GenerateUser();
}