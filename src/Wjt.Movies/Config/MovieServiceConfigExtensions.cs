namespace Wjt.Movies.Config;

using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

public static class MovieServiceConfigExtensions
{
    public static IServiceCollection AddMovieService(this IServiceCollection services, MovieServiceOptions? options)
    {
        ArgumentNullException.ThrowIfNull(options);

        services.AddScoped<IMovieService, MovieService>()
            .AddHybridCache(
                cacheOptions => cacheOptions.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromSeconds(options.CacheDurationInSeconds),
                    LocalCacheExpiration = TimeSpan.FromSeconds(options.CacheDurationInSeconds),
                });

        return services;
    }
}