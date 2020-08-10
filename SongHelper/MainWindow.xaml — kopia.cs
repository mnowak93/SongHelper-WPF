using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SongHelper
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        //Open lyrics on genius in browser
        private void lyricsButton_Click(object sender, RoutedEventArgs e)
        {
            string link = "https://genius.com/";
            if (songText.Text == "" && artistText.Text != "") link += "artists/";
            if (artistText.Text != "") link += artistText.Text.Replace(" ", "-");
            if (songText.Text != "") link += "-" + songText.Text.Replace(" ", "-").Replace("'", "") + "-lyrics";
            System.Diagnostics.Process.Start(link);
        }

        //Open information about band/info on wikipedia in browser
        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            string link = "https://en.wikipedia.org/wiki/" + artistText.Text.Replace(" ", "_");
            System.Diagnostics.Process.Start(link);
        }

        //Open video on YouTube
        private void videoButton_Click(object sender, RoutedEventArgs e)
        {
            string link = "https://www.youtube.com/results?search_query=" + artistText.Text.Replace(" ", "+") + "+" + songText.Text.Replace(" ", "+") + "+official+video";
            System.Diagnostics.Process.Start(link);
        }
    }
}
