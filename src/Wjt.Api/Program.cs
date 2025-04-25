using Wjt.CinemaWorld;
using Wjt.CinemaWorld.Config;
using Wjt.FilmWorld;
using Wjt.FilmWorld.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddCinemaWorldService(builder.Configuration.GetSection("CinemaWorldApi").Get<CinemaWorldApiOptions>())
    .AddFilmWorldService(builder.Configuration.GetSection("FilmWorldApi").Get<FilmWorldApiOptions>());


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/health", () => "I am alive!");

app.MapGet("/api/movies", async (ICinemaWorldService cinemaWorldService, IFilmWorldService filmWorldService) =>
{
    var (cinemaWorldMovies, cinemaResponse) = await cinemaWorldService.GetMoviesAsync();
    var (filmWorldMovies, filmWorldResponse) = await filmWorldService.GetMoviesAsync();
    
    if (cinemaResponse.IsSuccessStatusCode && filmWorldResponse.IsSuccessStatusCode)
    {
        var combinedMovies = new
        {
            CinemaWorldMovies = cinemaWorldMovies,
            FilmWorldMovies = filmWorldMovies
        };

        return Results.Ok(combinedMovies);
    }

    return Results.Problem("Error fetching movies");
});

app.Run();