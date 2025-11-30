namespace AuroraForecast.Aurora.Services;

internal class HttpClientHelper
{
    public static HttpClient GetHttpClient()
    {
        var result = new HttpClient();

        result.DefaultRequestHeaders.Add("User-Agent", "AuroraForecast/1.0 (github.com/Rumi4n/AuroraForecast)");

        return result;
    }
}