namespace Wjt.Test.FilmWorld;

using System.Net;

public class MockHttpMessageHandler() : HttpMessageHandler
{
    public HttpContent Content { get; set; } = new StringContent(string.Empty);

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage
        {
            StatusCode = StatusCode,
            Content = Content
        });
    }
}

