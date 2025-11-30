namespace AuroraForecast.Aurora.Models;

public class ThreeDayValue
{
    public DateTime Date { get; set; }
    public bool Now { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
}