using System.Globalization;
using System.Text.Json;
using AuroraForecast.Aurora.Helpers;
using AuroraForecast.Aurora.Interfaces;
using AuroraForecast.Aurora.Models;
using AuroraForecast.Locations.Models;

namespace AuroraForecast.Aurora.Services;

class AuroraApiWrapper : IAuroraApiWrapper
{
    public async Task<AuroraApiResponse> GetAuroraDataAsync(Location location)
    {
        var httpClient = HttpClientHelper.GetHttpClient();

        var endpoint = BuildAuroraEndpoint(location);

        var response = await httpClient.GetAsync(endpoint).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to fetch aurora data: {response.StatusCode}");
        }

        var jsonContent = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var apiResponse = JsonSerializer.Deserialize<AuroraApiResponse>(jsonContent, options);

        if (apiResponse?.ThreeDay == null)
        {
            throw new InvalidOperationException("ThreeDay data not found in API response");
        }

        return apiResponse;
    }

    private static Uri BuildAuroraEndpoint(Location location)
    {
        //https://api.auroras.live/v1/?type=all&lat=50.06143&long=19.93658&forecast=false
        var uriBuilder = new UriBuilder("https://api.auroras.live/v1/");

        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        query["type"] = "all";
        query["lat"] = location.Latitude.ToString(CultureInfo.InvariantCulture);
        query["long"] = location.Longitude.ToString(CultureInfo.InvariantCulture);
        query["forecast"] = "false";

        uriBuilder.Query = query.ToString() ?? string.Empty;

        var endpoint = uriBuilder.Uri;
        return endpoint;
    }
}