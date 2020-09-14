using SpotifyHandlingLibrary.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SpotifyHandlingLibrary
{
    public class SpotifyApiHandling : ISpotifyApiHandling
    {
        ISpotifyToken _spotifyToken;

        public SpotifyApiHandling(ISpotifyToken spotifyToken)
        {
            _spotifyToken = spotifyToken;
        }

        //getting acces token
        public string GetAccessToken()
        {
            //SpotifyToken token = new SpotifyToken();
            string ur15 = "https://accounts.spotify.com/api/token";

            //Put here your spotify Client ID and Secret
            string clientid = "***your Cient ID***";
            string clientsecret = "***your Client Secret***";

            var encodeClientIDandSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", clientid, clientsecret)));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ur15);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("Authorization: Basic " + encodeClientIDandSecret);

            var request = ("grant_type=client_credentials");
            byte[] req_bytes = Encoding.ASCII.GetBytes(request);
            webRequest.ContentLength = req_bytes.Length;

            Stream strm = webRequest.GetRequestStream();
            strm.Write(req_bytes, 0, req_bytes.Length);
            strm.Close();

            HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
            String json = "";
            using (Stream respStr = resp.GetResponseStream())
            {
                using (StreamReader rdr = new StreamReader(respStr, Encoding.UTF8))
                {
                    json = rdr.ReadToEnd();
                    rdr.Close();
                }
            }

            _spotifyToken = JsonConvert.DeserializeObject<SpotifyToken>(json);
            return _spotifyToken.access_token;
        }

        public string GetTrackInfo(string url)
        {
            string WebResponse = string.Empty;
            var myToken = GetAccessToken();
            try
            {
                HttpClient hc = new HttpClient();
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + myToken);

                var task = hc.SendAsync(request).ContinueWith((taskWithMsg) =>
                {
                    var response = taskWithMsg.Result;
                    var jsonTask = response.Content.ReadAsStringAsync();
                    WebResponse = jsonTask.Result;
                });
                task.Wait();
            }
            catch (WebException ex)
            {
                throw new WebException("A handled exception just occurred: " + ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                throw new TaskCanceledException("A handled exception just occurred: " + ex.Message);
            }
            return WebResponse;
        }
    }
}
