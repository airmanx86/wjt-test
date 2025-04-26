namespace Wjt.Test.CinemaWorld;

using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Wjt.CinemaWorld;

public class CinemaWorldServiceTests
{
    private MockHttpMessageHandler _httpMessageHandlerMock;

    private CinemaWorldService _subject;

    [SetUp]
    public void Setup()
    {
        _httpMessageHandlerMock = new MockHttpMessageHandler();

        var httpClient = new HttpClient(_httpMessageHandlerMock)
        {
            BaseAddress = new Uri("http://localhost")
        };

        _subject = new CinemaWorldService(httpClient);
    }

    [TearDown]
    public void TearDown()
    {
        _httpMessageHandlerMock.Dispose();
    }

    [Test]
    public async Task GetMovies_ShouldReturnMovies()
    {
        var jsonPayload = @"
        {
            ""Movies"": [
                {
                    ""Title"": ""Movie 1"",
                    ""Year"": ""1998"",
                    ""ID"": ""1"",
                    ""Genre"": ""Genre 1"",
                    ""Poster"": ""http://localhost/poster1.jpg""
                },
                {
                    ""Title"": ""Movie 2"",
                    ""Year"": ""1999"",
                    ""ID"": ""2"",
                    ""Genre"": ""Genre 2"",
                    ""Poster"": ""http://localhost/poster2.jpg""
                }
            ]
        }";

        var responseContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        _httpMessageHandlerMock.Content = responseContent;
        _httpMessageHandlerMock.StatusCode = HttpStatusCode.OK;

        var (result, response) = await _subject.GetMoviesAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Movies, Is.Not.Null);
        Assert.That(result.Movies.Count, Is.EqualTo(2));
        Assert.That(result.Movies[0].Title, Is.EqualTo("Movie 1"));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public async Task GetMovies_ShouldReturnNull_WhenResponseIsNotSuccessful(HttpStatusCode testStatusCode)
    {
        _httpMessageHandlerMock.StatusCode = testStatusCode;

        var (result, response) = await _subject.GetMoviesAsync();

        Assert.That(result, Is.Null);
        Assert.That(response.StatusCode, Is.EqualTo(testStatusCode));
    }

    [Test]
    public async Task GetMovieWithId_ShouldReturnMovieDetails()
    {
        var jsonPayload = @"
        {
            ""Title"": ""Star Wars: Episode IV - A New Hope"",
            ""Year"": ""1977"",
            ""Rated"": ""PG"",
            ""Released"": ""25 May 1977"",
            ""Runtime"": ""121 min"",
            ""Genre"": ""Action, Adventure, Fantasy"",
            ""Director"": ""George Director"",
            ""Writer"": ""George Writer"",
            ""Actors"": ""Mark Hamill, Harrison Ford, Carrie Fisher, Peter Cushing"",
            ""Plot"": ""Movie Plot"",
            ""Language"": ""English"",
            ""Country"": ""USA"",
            ""Awards"": ""Won 6 Oscars. Another 48 wins & 28 nominations."",
            ""Poster"": ""http://localhost/poster1.jpg"",
            ""Metascore"": ""92"",
            ""Rating"": ""8.7"",
            ""Votes"": ""915,459"",
            ""ID"": ""cw0076759"",
            ""Type"": ""movie"",
            ""Price"": ""123.5""
        }";

        var responseContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        _httpMessageHandlerMock.Content = responseContent;
        _httpMessageHandlerMock.StatusCode = HttpStatusCode.OK;

        var (result, response) = await _subject.GetMovieDetailsAsync("1");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo("Star Wars: Episode IV - A New Hope"));
        Assert.That(result.Year, Is.EqualTo("1977"));
        Assert.That(result.Rated, Is.EqualTo("PG"));
        Assert.That(result.Released, Is.EqualTo("25 May 1977"));
        Assert.That(result.Runtime, Is.EqualTo("121 min"));
        Assert.That(result.Genre, Is.EqualTo("Action, Adventure, Fantasy"));
        Assert.That(result.Director, Is.EqualTo("George Director"));
        Assert.That(result.Writer, Is.EqualTo("George Writer"));
        Assert.That(result.Actors, Is.EqualTo("Mark Hamill, Harrison Ford, Carrie Fisher, Peter Cushing"));
        Assert.That(result.Plot, Is.EqualTo("Movie Plot"));
        Assert.That(result.Language, Is.EqualTo("English"));
        Assert.That(result.Country, Is.EqualTo("USA"));
        Assert.That(result.Awards, Is.EqualTo("Won 6 Oscars. Another 48 wins & 28 nominations."));
        Assert.That(result.Poster, Is.EqualTo("http://localhost/poster1.jpg"));
        Assert.That(result.Metascore, Is.EqualTo("92"));
        Assert.That(result.Rating, Is.EqualTo("8.7"));
        Assert.That(result.Votes, Is.EqualTo("915,459"));
        Assert.That(result.ID, Is.EqualTo("cw0076759"));
        Assert.That(result.Type, Is.EqualTo("movie"));
        Assert.That(result.Price, Is.EqualTo(123.5m));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public async Task GetMovieWithId_ShouldReturnNull_WhenResponseIsNotSuccessful(HttpStatusCode testStatusCode)
    {
        _httpMessageHandlerMock.StatusCode = testStatusCode;

        var (result, response) = await _subject.GetMovieDetailsAsync("1");

        Assert.That(result, Is.Null);
        Assert.That(response.StatusCode, Is.EqualTo(testStatusCode));
    }
}
