namespace SongHelper
{
    public interface ISpotifyApiHandling
    {
        string GetAccessToken();
        string GetTrackInfo(string url);
    }
}