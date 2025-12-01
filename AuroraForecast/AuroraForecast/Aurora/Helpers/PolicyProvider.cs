using System.Runtime.CompilerServices;
using AuroraForecast.Aurora.Controllers;
using Polly;

namespace AuroraForecast.Aurora.Helpers;

class PolicyProvider : IPolicyProvider
{
    private readonly ILogger<AuroraController> _logger;

    public int MaxRetryAttempts = 7;

    public PolicyProvider(ILogger<AuroraController> logger)
    {
        _logger = logger;
    }

    public IAsyncPolicy GetExternalApiRetryAsyncPolicy([CallerMemberName] string methodName = "")
    {
        return Policy.Handle<Exception>()
            .WaitAndRetryAsync(
                MaxRetryAttempts,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryAttempt) =>
                {
                    _logger.LogWarning(
                        "Retrying {MethodName} (attempt {RetryAttempt}/{MaxRetries}) in {Delay} s. Exception: {Exception}",
                        methodName, retryAttempt, MaxRetryAttempts, timeSpan.TotalSeconds, exception.Message);
                });
    }
}