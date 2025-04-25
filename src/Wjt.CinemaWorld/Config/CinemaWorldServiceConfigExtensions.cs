namespace Wjt.CinemaWorld.Config;

using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Polly;

public static class CinemaWorldServiceConfigExtensions
{
    public static IServiceCollection AddCinemaWorldService(this IServiceCollection services, string baseUrl, string accessToken)
    {
        services.AddHttpClient<ICinemaWorldService, CinemaWorldService>(ConfigureCinemaWorldService(baseUrl, accessToken))
            // retry 3 times 80 + 160 + 320 = 560ms
            .AddTransientHttpErrorPolicy(
                builder =>
                    builder.WaitAndRetryAsync(
                        3,
                        retryCount => TimeSpan.FromMilliseconds(Math.Pow(80, retryCount))));

        return services;
    }

        public static Action<HttpClient> ConfigureCinemaWorldService(string baseUrl, string accessToken) => client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-access-token", accessToken);
        };
}
