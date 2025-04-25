namespace Wjt.CinemaWorld.Config;

public class CinemaWorldApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public int TimeoutInMilliseconds { get; set; } = 200;
}