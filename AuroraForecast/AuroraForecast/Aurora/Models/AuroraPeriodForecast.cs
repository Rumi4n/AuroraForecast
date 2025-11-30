namespace AuroraForecast.Aurora.Models;

public class AuroraPeriodForecast
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal KP { get; set; }
    public string Colour { get; set; } = string.Empty;
}