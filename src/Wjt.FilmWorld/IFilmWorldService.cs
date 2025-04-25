namespace Wjt.FilmWorld;

using Wjt.FilmWorld.Payloads;

public interface IFilmWorldService
{
    Task<(MovieDetails?, HttpResponseMessage)> GetMovieDetailsAsync(string id);
    Task<(MoviesResponse?, HttpResponseMessage)> GetMoviesAsync();
}
