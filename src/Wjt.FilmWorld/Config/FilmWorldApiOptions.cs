namespace Wjt.FilmWorld.Config;

public class FilmWorldApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public int TimeoutInMilliseconds { get; set; } = 10000;
}