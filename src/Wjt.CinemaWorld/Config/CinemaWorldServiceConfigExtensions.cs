namespace Wjt.CinemaWorld.Config;

using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Timeout;

public static class CinemaWorldServiceConfigExtensions
{
    public static IServiceCollection AddCinemaWorldService(this IServiceCollection services, CinemaWorldApiOptions? options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            throw new ArgumentException("BaseUrl cannot be null or empty", nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.AccessToken))
        {
            throw new ArgumentException("AccessToken cannot be null or empty", nameof(options));
        }

        services.AddHttpClient<ICinemaWorldService, CinemaWorldService>(ConfigureCinemaWorldService(options))
            // retry 3 times 80 + 160 + 320 = 560ms
            .AddTransientHttpErrorPolicy(
                builder =>
                    builder
                        .Or<TimeoutRejectedException>()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                        .WaitAndRetryAsync(
                            3,
                            retryCount => TimeSpan.FromMilliseconds(80 * retryCount))
                        .WrapAsync(Policy.TimeoutAsync(TimeSpan.FromMilliseconds(options.TimeoutInMilliseconds))));

        return services;
    }

    public static Action<HttpClient> ConfigureCinemaWorldService(CinemaWorldApiOptions options) => client =>
    {
        client.BaseAddress = new Uri(options.BaseUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("x-access-token", options.AccessToken);
    };
}
