using Wjt.CinemaWorld;
using Wjt.CinemaWorld.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddCinemaWorldService(builder.Configuration.GetSection("CinemaWorldApi").Get<CinemaWorldApiOptions>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/health", () => "I am alive!");

app.MapGet("/api/movies", async (ICinemaWorldService cinemaWorldService) =>
{
    var (movies, response) = await cinemaWorldService.GetMoviesAsync();

    if (response.IsSuccessStatusCode)
    {
        return Results.Ok(movies);
    }

    return Results.Problem("Error fetching movies");
});

app.Run();