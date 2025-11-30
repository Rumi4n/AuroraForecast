namespace AuroraForecast.Aurora.Models
{
    public class AuroraApiResponse
    {
        public DateTime Date { get; set; }
        public ThreeDayForecast? ThreeDay { get; set; }
    }
}
