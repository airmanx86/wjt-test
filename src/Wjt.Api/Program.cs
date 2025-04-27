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
    .AddMovieService(builder.Configuration.GetSection("MovieService").Get<MovieServiceOptions>())
    .AddCors(options =>
    {
        options.AddPolicy("DevelopmentCORS", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("DevelopmentCORS");
}

app.UseHttpsRedirection();

app.MapGet("/api/health", () => "I am alive!");

app.MapGet("/api/movies", (IMovieService movieService, [FromQuery(Name = "title")]string partialTitle = "") =>
{
    var filmWorldMovies = movieService.GetMoviesAsync(partialTitle);
    return Results.Ok(filmWorldMovies);
});

app.MapGet("/api/movies/{vendor}/{id}", async (IMovieService movieService, MovieVendor vendor, string id) =>
{
    try
    {
        var movieDetails = await movieService.GetMovieDetailsAsync(vendor, id);

        if (movieDetails == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(movieDetails);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error fetching movie details");
        return Results.Problem("Movie details is unavailable", statusCode: 503);
    }
});

app.Run();