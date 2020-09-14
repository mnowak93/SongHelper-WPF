﻿
namespace SpotifyHandlingLibrary.Models
{
    public class SpotifyToken : ISpotifyToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}
