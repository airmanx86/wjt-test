using Wjt.CinemaWorld.Payloads;

public interface ICinemaWorldService
{
    Task<(MovieDetails?, HttpResponseMessage)> GetMovieDetailsAsync(string id);
    Task<(MoviesResponse?, HttpResponseMessage)> GetMoviesAsync();
}
