using AuroraForecast.Aurora.Helpers;
using AuroraForecast.Aurora.Interfaces;
using AuroraForecast.Aurora.Models;
using AuroraForecast.Locations.Models;


namespace AuroraForecast.Aurora.Services;

class AuroraApiWrapperWithRetry : IAuroraApiWrapper
{
    private readonly IAuroraApiWrapper _auroraApiWrapper;
    private readonly IPolicyProvider _policyProvider;

    public AuroraApiWrapperWithRetry(IAuroraApiWrapper auroraApiWrapper, IPolicyProvider policyProvider)
    {
        _auroraApiWrapper = auroraApiWrapper;
        _policyProvider = policyProvider;
    }

    public async Task<AuroraApiResponse> GetAuroraDataAsync(Location location)
    {
        return await _policyProvider.GetExternalApiRetryAsyncPolicy()
            .ExecuteAsync(async () => await _auroraApiWrapper.GetAuroraDataAsync(location));
    }
}