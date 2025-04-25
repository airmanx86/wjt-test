namespace Wjt.Movies;

using Wjt.Movies.Payloads;

public interface IMovieService
{
    IAsyncEnumerable<MovieItem> GetMoviesAsync(string partialTitle);
}