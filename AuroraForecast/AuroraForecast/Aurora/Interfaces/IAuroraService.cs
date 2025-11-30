namespace AuroraForecast.Aurora.Interfaces;

public interface IAuroraService
{
    Task<IEnumerable<Models.AuroraForecast>> GetForecastsAsync(int userId);
}