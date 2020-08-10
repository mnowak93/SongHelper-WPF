using System;
using System.Windows;
using Newtonsoft.Json;

namespace SongHelper
{
    public partial class MainWindow : Window
    {
        //getting screen resolution
        public int screenWidth = (int)SystemParameters.PrimaryScreenWidth;
        public int screenHeight = (int)SystemParameters.PrimaryScreenHeight;

        public MainWindow()
        {
            InitializeComponent();
        }

        //Open lyrics on genius in browser after button click
        private void lyricsButton_Click(object sender, RoutedEventArgs e)
        {
            string link = "https://genius.com/";
            if (songText.Text == "" && artistText.Text != "") link += "artists/";
            if (artistText.Text != "") link += artistText.Text.Replace(" ", "-");
            if (songText.Text != "") link += "-" + songText.Text.Replace(" ", "-").Replace("'", "") + "-lyrics";
            System.Diagnostics.Process.Start(link);
        }

        //Open information about band/artist on allmusic in browser after button click
        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            string link = "https://www.allmusic.com/search/all/" + artistText.Text.Replace(" ", "_");
            System.Diagnostics.Process.Start(link);            
            MouseSimulation.LeftMouseClick(Convert.ToInt32(screenWidth * 0.7), Convert.ToInt32(screenHeight * 0.35), 1000);
        }

        //Open video on YouTube in browser after button click
        private void videoButton_Click(object sender, RoutedEventArgs e)
        {
            string text = artistText.Text + " " + songText.Text;
            YotubeApiHandling ytHandler = new YotubeApiHandling();
            ytHandler.Search(text);
        }

        //playing song on spotify after button click
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            string artist = artistText.Text;
            string song = songText.Text;            
            string link = string.Format("https://api.spotify.com/v1/search?q={0} {1}&type=track&market=US&limit=10&offset=0", song, artist);

            var jsonData = SpotifyApiHandling.GetTrackInfo(link);
            Rootobject lst = JsonConvert.DeserializeObject<Rootobject>(jsonData);

            if (lst.tracks.total > 0)
            {
                link = lst.tracks.items[0].external_urls.spotify;
                System.Diagnostics.Process.Start(link);
            }
            else
            {
                Popup popup = new Popup();
                popup.Show();
            }
        }        
    }
}