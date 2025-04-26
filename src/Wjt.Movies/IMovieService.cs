namespace Wjt.Movies;

using Wjt.Movies.Payloads;

public interface IMovieService
{
    Task<MovieDetails?> GetMovieDetailsAsync(MovieVendor vendor, string id);
    IAsyncEnumerable<MovieItem> GetMoviesAsync(string partialTitle);
}