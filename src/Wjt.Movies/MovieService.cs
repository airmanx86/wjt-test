namespace Wjt.Movies;

using System.Collections.Generic;
using System.Threading.Tasks;
using Wjt.CinemaWorld;
using Wjt.FilmWorld;
using Wjt.Movies.Payloads;

public class MovieService(ICinemaWorldService cinemaWorldService, IFilmWorldService filmWorldService) : IMovieService
{
    public async IAsyncEnumerable<MovieItem> GetMoviesAsync()
    {
        await foreach (var getMoviesTask in Task.WhenEach(GetMoviesFromCinemaWorldAsync(), GetMoviesFromFilmWorldAsync()))
        {
            var movies = await getMoviesTask;

            foreach (var item in movies)
            {
                yield return item;
            }        
        }
    }

    private async Task<IEnumerable<MovieItem>> GetMoviesFromCinemaWorldAsync()
    {
        var (cinemaWorldMovies, cinemaResponse) = await cinemaWorldService.GetMoviesAsync();

        if (cinemaResponse.IsSuccessStatusCode && cinemaWorldMovies != null)
        {
            return cinemaWorldMovies.Movies
                .Where(m => m.Type == "movie")
                .Select(m => new MovieItem
                (
                    Title: m.Title,
                    Year: m.Year,
                    ExternalID: m.ID,
                    Poster: m.Poster,
                    Vendor: "CinemaWorld"
                ));
        }

        return [];
    }

    private async Task<IEnumerable<MovieItem>> GetMoviesFromFilmWorldAsync()
    {
        var (filmWorldMovies, filmResponse) = await filmWorldService.GetMoviesAsync();

        if (filmResponse.IsSuccessStatusCode && filmWorldMovies != null)
        {
            return filmWorldMovies.Movies
                .Where(m => m.Type == "movie")
                .Select(m => new MovieItem
                (
                    Title: m.Title,
                    Year: m.Year,
                    ExternalID: m.ID,
                    Poster: m.Poster,
                    Vendor: "FilmWorld"
                ));
        }

        return [];
    }
}
