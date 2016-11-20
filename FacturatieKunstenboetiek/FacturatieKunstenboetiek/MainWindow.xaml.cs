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
using System.Net;

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OverzichtWindow window = new OverzichtWindow();
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            window.Show();
        }

        public void Initialize()
        {
            if (!CheckForInternetConnection())
            {
                if (MessageBox.Show("Gelieve te verbinden met het internet.", "Kunstenboetiek", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Initialize();
                }
                else
                {
                    this.Close();
                }
            }
        }

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void buttonFactuur_Click(object sender, RoutedEventArgs e)
        {
            Window factuur = new FactuurWindow();
            factuur.Show();
            this.Close();
        }

        private void buttonKlant_Click(object sender, RoutedEventArgs e)
        {
            Window klant = new KlantWindow();
            klant.Show();
            this.Close();
        }

        private void buttonArtikel_Click(object sender, RoutedEventArgs e)
        {
            Window artikel = new ArtikelWindow();
            artikel.Show();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
