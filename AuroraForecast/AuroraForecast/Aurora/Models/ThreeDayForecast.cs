namespace AuroraForecast.Aurora.Models;

public class ThreeDayForecast
{
    public DateTime Date { get; set; }
    public List<DateTime> Dates { get; set; } = new();
    public List<List<ThreeDayValue>> Values { get; set; } = new();
}