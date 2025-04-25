using Microsoft.AspNetCore.Mvc;
using Wjt.CinemaWorld.Config;
using Wjt.FilmWorld.Config;
using Wjt.Movies;
using Wjt.Movies.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddCinemaWorldService(builder.Configuration.GetSection("CinemaWorldApi").Get<CinemaWorldApiOptions>())
    .AddFilmWorldService(builder.Configuration.GetSection("FilmWorldApi").Get<FilmWorldApiOptions>())
    .AddMovieService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/health", () => "I am alive!");

app.MapGet("/api/movies", (IMovieService movieService, [FromQuery(Name = "title")]string partialTitle = "") =>
{
    var filmWorldMovies = movieService.GetMoviesAsync(partialTitle);
    return Results.Ok(filmWorldMovies);
});

app.Run();