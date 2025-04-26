namespace Wjt.Test.Movies;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Wjt.CinemaWorld;
using Wjt.FilmWorld;
using Wjt.Movies;
using Wjt.Movies.Payloads;

public class MovieServiceTests
{
    private Fixture _fixture = new();

    private ICinemaWorldService _cinemaWorldServiceMock;

    private IFilmWorldService _filmWorldServiceMock;

    private ILogger<MovieService> _loggerMock;

    private HybridCache _cacheMock;

    private MovieService _subject;

    [SetUp]
    public void Setup()
    {
        _cinemaWorldServiceMock = Substitute.For<ICinemaWorldService>();
        _filmWorldServiceMock = Substitute.For<IFilmWorldService>();
        _loggerMock = Substitute.For<ILogger<MovieService>>();
        _cacheMock = new MockHybridCache();
        _subject = new MovieService(_loggerMock, _cacheMock, _cinemaWorldServiceMock, _filmWorldServiceMock);
    }

    [Test]
    public async Task GetMovieWithCinemaWorldAndId_ShouldReturnMovieDetails()
    {
        _cinemaWorldServiceMock.GetMovieDetailsAsync(Arg.Any<string>()).Returns(Task.FromResult(((CinemaWorld.Payloads.MovieDetails?)new(
            Title: "Movie 1",
            Year: "1998",
            Rated: "PG",
            Released: "25 May 1998",
            Runtime: "121 min",
            Genre: "Genre 1",
            Director: "George Director",
            Writer: "George Writer",
            Actors: "Actor 1, Actor 2",
            Plot: "Movie Plot",
            Language: "English",
            Country: "USA",
            Awards: "Won 6 Oscars. Another 48 wins & 28 nominations.",
            Poster: "http://localhost/poster1.jpg",
            Metascore: "92",
            Rating: "8.7",
            Votes: "915,459",
            ID: "1",
            Type: "movie",
            Price: 123.5m
        ), new HttpResponseMessage(HttpStatusCode.OK))));

        var result = await _subject.GetMovieDetailsAsync(MovieVendor.CinemaWorld, _fixture.Create<int>().ToString());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo("Movie 1"));
        Assert.That(result.Year, Is.EqualTo("1998"));
        Assert.That(result.Rated, Is.EqualTo("PG"));
        Assert.That(result.Released, Is.EqualTo("25 May 1998"));
        Assert.That(result.Runtime, Is.EqualTo("121 min"));
        Assert.That(result.Genre, Is.EqualTo("Genre 1"));
        Assert.That(result.Director, Is.EqualTo("George Director"));
        Assert.That(result.Writer, Is.EqualTo("George Writer"));
        Assert.That(result.Actors, Is.EqualTo("Actor 1, Actor 2"));
        Assert.That(result.Plot, Is.EqualTo("Movie Plot"));
        Assert.That(result.Language, Is.EqualTo("English"));
        Assert.That(result.Country, Is.EqualTo("USA"));
        Assert.That(result.Awards, Is.EqualTo("Won 6 Oscars. Another 48 wins & 28 nominations."));
        Assert.That(result.Poster, Is.EqualTo("http://localhost/poster1.jpg"));
        Assert.That(result.Metascore, Is.EqualTo("92"));
        Assert.That(result.Rating, Is.EqualTo("8.7"));
        Assert.That(result.Votes, Is.EqualTo("915,459"));
        Assert.That(result.ExternalID, Is.EqualTo("1"));
        Assert.That(result.Type, Is.EqualTo("movie"));
        Assert.That(result.Price, Is.EqualTo(123.5m));
    }

    [Test]
    public async Task GetMovieWithCinemaWorldNotFound_ShouldReturnNull()
    {
        _cinemaWorldServiceMock.GetMovieDetailsAsync(Arg.Any<string>()).Returns(Task.FromResult(((CinemaWorld.Payloads.MovieDetails?)null, new HttpResponseMessage(HttpStatusCode.NotFound))));

        var result = await _subject.GetMovieDetailsAsync(MovieVendor.CinemaWorld, _fixture.Create<int>().ToString());

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetMovieWithFilmWorldAndId_ShouldReturnMovieDetails()
    {
        _filmWorldServiceMock.GetMovieDetailsAsync(Arg.Any<string>()).Returns(Task.FromResult(((FilmWorld.Payloads.MovieDetails?)new(
            Title: "Movie 2",
            Year: "1996",
            Rated: "PG",
            Released: "25 May 1998",
            Runtime: "121 min",
            Genre: "Genre 1",
            Director: "George Director",
            Writer: "George Writer",
            Actors: "Actor 1, Actor 2",
            Plot: "Movie Plot",
            Language: "English",
            Country: "USA",
            Poster: "http://localhost/poster1.jpg",
            Metascore: "92",
            Rating: "8.7",
            Votes: "915,459",
            ID: "2",
            Type: "movie",
            Price: 123.5m
        ), new HttpResponseMessage(HttpStatusCode.OK))));

        var result = await _subject.GetMovieDetailsAsync(MovieVendor.FilmWorld, _fixture.Create<int>().ToString());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo("Movie 2"));
        Assert.That(result.Year, Is.EqualTo("1996"));
        Assert.That(result.Rated, Is.EqualTo("PG"));
        Assert.That(result.Released, Is.EqualTo("25 May 1998"));
        Assert.That(result.Runtime, Is.EqualTo("121 min"));
        Assert.That(result.Genre, Is.EqualTo("Genre 1"));
        Assert.That(result.Director, Is.EqualTo("George Director"));
        Assert.That(result.Writer, Is.EqualTo("George Writer"));
        Assert.That(result.Actors, Is.EqualTo("Actor 1, Actor 2"));
        Assert.That(result.Plot, Is.EqualTo("Movie Plot"));
        Assert.That(result.Language, Is.EqualTo("English"));
        Assert.That(result.Country, Is.EqualTo("USA"));
        Assert.That(result.Poster, Is.EqualTo("http://localhost/poster1.jpg"));
        Assert.That(result.Metascore, Is.EqualTo("92"));
        Assert.That(result.Rating, Is.EqualTo("8.7"));
        Assert.That(result.Votes, Is.EqualTo("915,459"));
        Assert.That(result.ExternalID, Is.EqualTo("2"));
        Assert.That(result.Type, Is.EqualTo("movie"));
        Assert.That(result.Price, Is.EqualTo(123.5m));
    }

    [Test]
    public async Task GetMovieWithFilmWorldNotFound_ShouldReturnNull()
    {
        _filmWorldServiceMock.GetMovieDetailsAsync(Arg.Any<string>()).Returns(Task.FromResult(((FilmWorld.Payloads.MovieDetails?)null, new HttpResponseMessage(HttpStatusCode.NotFound))));

        var result = await _subject.GetMovieDetailsAsync(MovieVendor.FilmWorld, _fixture.Create<int>().ToString());

        Assert.That(result, Is.Null);
    }


    [TestCase(HttpStatusCode.BadRequest, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.InternalServerError, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.BadGateway, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.ServiceUnavailable, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.GatewayTimeout, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.BadRequest, MovieVendor.FilmWorld)]
    [TestCase(HttpStatusCode.InternalServerError, MovieVendor.FilmWorld)]
    [TestCase(HttpStatusCode.BadGateway, MovieVendor.FilmWorld)]
    [TestCase(HttpStatusCode.ServiceUnavailable, MovieVendor.FilmWorld)]
    [TestCase(HttpStatusCode.GatewayTimeout, MovieVendor.FilmWorld)]
    public void GetMovie_ShouldThrow_WhenResponseIsNotSuccessful(HttpStatusCode testStatusCode, MovieVendor vendor)
    {
        if (vendor == MovieVendor.CinemaWorld)
        {
            _cinemaWorldServiceMock.GetMovieDetailsAsync(Arg.Any<string>()).Returns(Task.FromResult(((CinemaWorld.Payloads.MovieDetails?)null, new HttpResponseMessage(testStatusCode))));
        }

        if (vendor == MovieVendor.FilmWorld)
        {
            _filmWorldServiceMock.GetMovieDetailsAsync(Arg.Any<string>()).Returns(Task.FromResult(((FilmWorld.Payloads.MovieDetails?)null, new HttpResponseMessage(testStatusCode))));
        }

        Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            _ = await _subject.GetMovieDetailsAsync(vendor, _fixture.Create<int>().ToString());
        });
    }

    [Test]
    public async Task GetMovies_ShouldReturnMovies()
    {
        _cinemaWorldServiceMock.GetMoviesAsync().Returns(Task.FromResult(((CinemaWorld.Payloads.MoviesResponse?)new(
            [
                new CinemaWorld.Payloads.MovieItem("Movie 1", "1998", "1", "movie", "http://localhost/poster1.jpg"),
                new CinemaWorld.Payloads.MovieItem("Movie 2", "1999", "2", "movie", "http://localhost/poster2.jpg")
            ]), new HttpResponseMessage(HttpStatusCode.OK))));

        _filmWorldServiceMock.GetMoviesAsync().Returns(Task.FromResult(((FilmWorld.Payloads.MoviesResponse?)new(
            [
                new FilmWorld.Payloads.MovieItem("Movie 3", "1996", "3", "movie", "http://localhost/poster3.jpg"),
                new FilmWorld.Payloads.MovieItem("Movie 4", "1997", "4", "movie", "http://localhost/poster4.jpg")
            ]), new HttpResponseMessage(HttpStatusCode.OK))));

        var result = _subject.GetMoviesAsync();

        var movies = new List<MovieItem>();

        await foreach (var movie in result)
        {
            movies.Add(movie);
        }

        movies.Sort((x, y) => x.Title.CompareTo(y.Title));

        Assert.That(movies.Count, Is.EqualTo(4));
        Assert.That(movies[0].Title, Is.EqualTo("Movie 1"));
        Assert.That(movies[1].Title, Is.EqualTo("Movie 2"));
        Assert.That(movies[2].Title, Is.EqualTo("Movie 3"));
        Assert.That(movies[3].Title, Is.EqualTo("Movie 4"));
    }

    [TestCase(HttpStatusCode.BadRequest, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.InternalServerError, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.BadGateway, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.ServiceUnavailable, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.GatewayTimeout, MovieVendor.CinemaWorld)]
    [TestCase(HttpStatusCode.BadRequest, MovieVendor.FilmWorld)]
    [TestCase(HttpStatusCode.InternalServerError, MovieVendor.FilmWorld)]
    [TestCase(HttpStatusCode.BadGateway, MovieVendor.FilmWorld)]
    [TestCase(HttpStatusCode.ServiceUnavailable, MovieVendor.FilmWorld)]
    [TestCase(HttpStatusCode.GatewayTimeout, MovieVendor.FilmWorld)]
    public async Task GetMovies_ShouldReturnEmptyCollection_WhenReponseIsNotSuccessful(HttpStatusCode testStatusCode, MovieVendor vendor)
    {
        if (vendor == MovieVendor.CinemaWorld)
        {
            _cinemaWorldServiceMock.GetMoviesAsync().Returns(Task.FromResult(((CinemaWorld.Payloads.MoviesResponse?)null, new HttpResponseMessage(testStatusCode))));

            _filmWorldServiceMock.GetMoviesAsync().Returns(Task.FromResult(((FilmWorld.Payloads.MoviesResponse?)new(
                [
                    new FilmWorld.Payloads.MovieItem("Movie 3", "1996", "3", "movie", "http://localhost/poster3.jpg"),
                    new FilmWorld.Payloads.MovieItem("Movie 4", "1997", "4", "movie", "http://localhost/poster4.jpg")
                ]), new HttpResponseMessage(HttpStatusCode.OK))));
        }

        if (vendor == MovieVendor.FilmWorld)
        {
            _cinemaWorldServiceMock.GetMoviesAsync().Returns(Task.FromResult(((CinemaWorld.Payloads.MoviesResponse?)new(
                [
                    new CinemaWorld.Payloads.MovieItem("Movie 1", "1998", "1", "movie", "http://localhost/poster1.jpg"),
                    new CinemaWorld.Payloads.MovieItem("Movie 2", "1999", "2", "movie", "http://localhost/poster2.jpg")
                ]), new HttpResponseMessage(HttpStatusCode.OK))));

            _filmWorldServiceMock.GetMoviesAsync().Returns(Task.FromResult(((FilmWorld.Payloads.MoviesResponse?)null, new HttpResponseMessage(testStatusCode))));
        }

        var result = _subject.GetMoviesAsync();

        if (vendor == MovieVendor.CinemaWorld)
        {
            var movies = new List<MovieItem>();

            await foreach (var movie in result)
            {
                movies.Add(movie);
            }

            movies.Sort((x, y) => x.Title.CompareTo(y.Title));

            Assert.That(movies.Count, Is.EqualTo(2));
            Assert.That(movies[0].Title, Is.EqualTo("Movie 3"));
            Assert.That(movies[1].Title, Is.EqualTo("Movie 4"));
        }

        if (vendor == MovieVendor.FilmWorld)
        {
            var movies = new List<MovieItem>();

            await foreach (var movie in result)
            {
                movies.Add(movie);
            }

            movies.Sort((x, y) => x.Title.CompareTo(y.Title));

            Assert.That(movies.Count, Is.EqualTo(2));
            Assert.That(movies[0].Title, Is.EqualTo("Movie 1"));
            Assert.That(movies[1].Title, Is.EqualTo("Movie 2"));
        }
    }
}
