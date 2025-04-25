namespace Wjt.Movies;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Wjt.CinemaWorld;
using Wjt.FilmWorld;
using Wjt.Movies.Payloads;

public class MovieService(ILogger<MovieService> logger, HybridCache cache, ICinemaWorldService cinemaWorldService, IFilmWorldService filmWorldService) : IMovieService
{
    public async IAsyncEnumerable<MovieItem> GetMoviesAsync(string partialTitle = "")
    {
        await foreach (var getMoviesTask in Task.WhenEach(GetMoviesFromCinemaWorldAsync(partialTitle), GetMoviesFromFilmWorldAsync(partialTitle)))
        {
            var movies = await getMoviesTask;

            foreach (var item in movies)
            {
                yield return item;
            }        
        }
    }

    private async Task<IEnumerable<MovieItem>> GetMoviesFromCinemaWorldAsync(string partialTitle)
    {
        try
        {
            return await cache.GetOrCreateAsync($"CinemaWorldMovies-{partialTitle}", async token => {
                var (cinemaWorldMovies, cinemaResponse) = await cinemaWorldService.GetMoviesAsync();

                cinemaResponse.EnsureSuccessStatusCode();

                if (cinemaWorldMovies != null)
                {
                    return cinemaWorldMovies.Movies
                        .Where(m => m.Type == "movie")
                        .Where(m => string.IsNullOrEmpty(partialTitle) || m.Title.Contains(partialTitle, StringComparison.OrdinalIgnoreCase))
                        .Select(m => new MovieItem
                        (
                            Title: m.Title,
                            Year: m.Year,
                            ExternalID: m.ID,
                            Poster: m.Poster,
                            Vendor: "CinemaWorld"
                        ));
                }

                throw new Exception("Failed to fetch movies from CinemaWorld");
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching movies from CinemaWorld, return empty list");
            return [];
        }
    }

    private async Task<IEnumerable<MovieItem>> GetMoviesFromFilmWorldAsync(string partialTitle)
    {
        try
        {
            return await cache.GetOrCreateAsync($"FilmWorldMovies-{partialTitle}", async token => {
                var (filmWorldMovies, filmResponse) = await filmWorldService.GetMoviesAsync();

                filmResponse.EnsureSuccessStatusCode();

                if (filmWorldMovies != null)
                {
                    return filmWorldMovies.Movies
                        .Where(m => m.Type == "movie")
                        .Where(m => string.IsNullOrEmpty(partialTitle) || m.Title.Contains(partialTitle, StringComparison.OrdinalIgnoreCase))
                        .Select(m => new MovieItem
                        (
                            Title: m.Title,
                            Year: m.Year,
                            ExternalID: m.ID,
                            Poster: m.Poster,
                            Vendor: "FilmWorld"
                        ));
                }

                throw new Exception("Failed to fetch movies from FilmWorld");
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching movies from FilmWorld, return empty list");
            return [];
        }
    }
}
