using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace SongHelper
{
    public class YotubeApiHandling : IYotubeApiHandling
    {
        [STAThread]
        public async void Search(string text)
        {
            try
            {
                await Run(text);
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }

        private async Task Run(string searchText)
        {
            string result;

            //Adding API Key
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                //Put here your Youtube Api Key
                ApiKey = "***your api key***",
                ApplicationName = this.GetType().ToString()
            });

            //Searching
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = searchText;
            searchListRequest.MaxResults = 1;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0}", searchResult.Id.VideoId));
                        break;
                }
            }

            //opening url
            result = "https://www.youtube.com/watch?v=" + videos[0];
            System.Diagnostics.Process.Start(result);
        }
    }
}