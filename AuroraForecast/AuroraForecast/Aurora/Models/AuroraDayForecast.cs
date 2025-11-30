namespace AuroraForecast.Aurora.Models;

public class AuroraDayForecast
{
    public DateTime Date { get; set; }
    public List<AuroraPeriodForecast> PeriodForecasts { get; set; } = new();
}