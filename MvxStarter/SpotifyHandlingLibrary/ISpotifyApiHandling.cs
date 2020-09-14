namespace SpotifyHandlingLibrary
{
    public interface ISpotifyApiHandling
    {
        string GetAccessToken();
        string GetTrackInfo(string url);
    }
}