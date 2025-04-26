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
    public async Task<MovieDetails?> GetMovieDetailsAsync(MovieVendor vendor, string id)
    {
        return vendor switch
        {
            MovieVendor.CinemaWorld => await GetMovieDetailsFromCinemaWorldAsync(id),
            MovieVendor.FilmWorld => await GetMovieDetailsFromFilmWorldAsync(id),
            _ => null
        };
    }

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

    private async Task<MovieDetails?> GetMovieDetailsFromCinemaWorldAsync(string id)
    {
        try
        {
            return await cache.GetOrCreateAsync($"CinemaWorldMovieDetails-{id}", async token => {
                var (movieDetails, response) = await cinemaWorldService.GetMovieDetailsAsync(id);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                if (movieDetails != null)
                {
                    return new MovieDetails
                    (
                        Title: movieDetails.Title,
                        Year: movieDetails.Year,
                        Rated: movieDetails.Rated,
                        Released: movieDetails.Released,
                        Runtime: movieDetails.Runtime,
                        Genre: movieDetails.Genre,
                        Director: movieDetails.Director,
                        Writer: movieDetails.Writer,
                        Actors: movieDetails.Actors,
                        Plot: movieDetails.Plot,
                        Language: movieDetails.Language,
                        Country: movieDetails.Country,
                        Awards: movieDetails.Awards,
                        Poster: movieDetails.Poster,
                        Metascore: movieDetails.Metascore,
                        Rating: movieDetails.Rating,
                        Votes: movieDetails.Votes,
                        ExternalID: movieDetails.ID,
                        Type: movieDetails.Type,
                        Price: movieDetails.Price
                    );
                }

                throw new Exception("Failed to fetch movie details from CinemaWorld");
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching movie details from CinemaWorld");
            throw;
        }
    }

    private async Task<MovieDetails?> GetMovieDetailsFromFilmWorldAsync(string id)
    {
        try
        {
            return await cache.GetOrCreateAsync($"FilmWorldMovieDetails-{id}", async token => {
                var (movieDetails, response) = await filmWorldService.GetMovieDetailsAsync(id);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                if (movieDetails != null)
                {
                    return new MovieDetails
                    (
                        Title: movieDetails.Title,
                        Year: movieDetails.Year,
                        Rated: movieDetails.Rated,
                        Released: movieDetails.Released,
                        Runtime: movieDetails.Runtime,
                        Genre: movieDetails.Genre,
                        Director: movieDetails.Director,
                        Writer: movieDetails.Writer,
                        Actors: movieDetails.Actors,
                        Plot: movieDetails.Plot,
                        Language: movieDetails.Language,
                        Country: movieDetails.Country,
                        Awards: null,
                        Poster: movieDetails.Poster,
                        Metascore: movieDetails.Metascore,
                        Rating: movieDetails.Rating,
                        Votes: movieDetails.Votes,
                        ExternalID: movieDetails.ID,
                        Type: movieDetails.Type,
                        Price: movieDetails.Price
                    );
                }

                throw new Exception("Failed to fetch movie details from FilmWorld");
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching movie details from FilmWorld");
            throw;
        }
    }
}
