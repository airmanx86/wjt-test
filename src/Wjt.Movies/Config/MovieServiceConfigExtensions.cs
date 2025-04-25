namespace Wjt.Movies.Config;

using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

public static class MovieServiceConfigExtensions
{
    public static IServiceCollection AddMovieService(this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>()
            .AddHybridCache(
                options => options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromSeconds(30),
                    LocalCacheExpiration = TimeSpan.FromSeconds(30),
                });

        return services;
    }
}