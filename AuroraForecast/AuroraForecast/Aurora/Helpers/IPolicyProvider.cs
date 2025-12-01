using Polly;
using System.Runtime.CompilerServices;

namespace AuroraForecast.Aurora.Helpers;

internal interface IPolicyProvider
{
    IAsyncPolicy GetExternalApiRetryAsyncPolicy([CallerMemberName] string methodName = "");
}