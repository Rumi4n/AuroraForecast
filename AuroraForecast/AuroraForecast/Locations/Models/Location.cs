namespace AuroraForecast.Locations.Models;

public class Location
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}