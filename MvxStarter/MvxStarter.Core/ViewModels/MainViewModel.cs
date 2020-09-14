using MvvmCross.ViewModels;
using MvvmCross.Commands;
using System;
using System.Diagnostics;
using YoutubeHandlingLibrary;
using Autofac;
using SpotifyHandlingLibrary;
using SpotifyHandlingLibrary.Models;
using Newtonsoft.Json;

namespace MvxStarter.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public MainViewModel()
        {
            LyricsButtonActionCommand = new MvxCommand(LyricsButtonAction);
            ArtistButtonActionCommand = new MvxCommand(ArtistButtonAction);
            YoutubeButtonActionCommand = new MvxCommand(YoutubeButtonAction);
            SpotifyButtonActionCommand = new MvxCommand(SpotifyButtonAction);
        }

        //Methods for buttons
        public IMvxCommand LyricsButtonActionCommand { get; set; }
        public IMvxCommand ArtistButtonActionCommand { get; set; }
        public IMvxCommand YoutubeButtonActionCommand { get; set; }
        public IMvxCommand SpotifyButtonActionCommand { get; set; }

        public void LyricsButtonAction()
        {
            string link = "https://genius.com/" + ArtistText.Replace(" ", "-") + "-" + SongNameText.Replace(" ", "-").Replace("'", "") + "-lyrics";
            var psi = new ProcessStartInfo
            {
                FileName = link,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }        

        public void ArtistButtonAction()
        {
            string link = "https://www.allmusic.com/search/all/" + ArtistText.Replace(" ", "%20");
            var psi = new ProcessStartInfo
            {
                FileName = link,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }

        public void YoutubeButtonAction()
        {
            string text = ArtistText + " " + SongNameText;
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IYotubeApiHandling>();
                app.Search(text);
            }
        }

        public void SpotifyButtonAction()
        {
            string link = string.Format("https://api.spotify.com/v1/search?q={0} {1}&type=track&market=US&limit=10&offset=0", SongNameText, ArtistText);

            var container = ContainerConfig.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ISpotifyApiHandling>();
                var jsonData = app.GetTrackInfo(link);
                Rootobject lst = JsonConvert.DeserializeObject<Rootobject>(jsonData);

                if (lst.tracks.total > 0)
                {
                    link = lst.tracks.items[0].external_urls.spotify;
                    var psi = new ProcessStartInfo
                    {
                        FileName = link,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(psi);
                }
                else
                {
                    ArtistText = "Brak utworu";
                    SongNameText = String.Empty;
                }
            }
        }


        //Methods to prevent of using buttons when the artist and song name is empty
        public bool CanUseButtons => ArtistText?.Length > 0 && SongNameText?.Length > 0;
        public bool CanUseArtistButton => ArtistText?.Length > 0;

        //Properties for Artist and Song texts
        private string _artistText;

        public string ArtistText
        {
            get { return _artistText; }
            set 
            { 
                SetProperty(ref _artistText, value);
                RaisePropertyChanged(() => CanUseButtons);
                RaisePropertyChanged(() => CanUseArtistButton);
            }
        }

        private string _songNameText;

        public string SongNameText
        {
            get { return _songNameText; }
            set 
            { 
                SetProperty(ref _songNameText, value);
                RaisePropertyChanged(() => CanUseButtons);
            }
        }
        
    }
}
