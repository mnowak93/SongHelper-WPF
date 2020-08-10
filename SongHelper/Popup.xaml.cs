using System.Windows;

namespace SongHelper
{
    /// <summary>
    /// Logika interakcji dla klasy Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {
        public Popup()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
