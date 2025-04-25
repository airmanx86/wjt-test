namespace Wjt.CinemaWorld;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Wjt.CinemaWorld.Payloads;

public class CinemaWorldService(HttpClient httpClient): ICinemaWorldService
{
    private const string MoviesEndpoint = "/api/cinemaworld/movies";

    private const string MovieDetailsEndpoint = "/api/cinemaworld/movie/";

    private readonly HttpClient _httpClient = httpClient;

    public async Task<(MoviesResponse?, HttpResponseMessage)> GetMoviesAsync()
    {
        var response = await _httpClient.GetAsync(MoviesEndpoint);

        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            var movieResponse = await JsonSerializer.DeserializeAsync<MoviesResponse>(stream);
            return (movieResponse, response);
        }
        else
        {
            return (null, response);
        }
    }

    public async Task<(MovieDetails?, HttpResponseMessage)> GetMovieDetailsAsync(string id)
    {
        var response = await _httpClient.GetAsync(MovieDetailsEndpoint + Uri.EscapeDataString(id));

        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            var movieDetails = await JsonSerializer.DeserializeAsync<MovieDetails>(stream);
            return (movieDetails, response);
        }
        else
        {
            return (null, response);
        }
    }
}