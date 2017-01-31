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
        bool isClosedByButton;
        public MainWindow()
        {
            InitializeComponent();
            CheckDatabase();
            isClosedByButton = false;
        }

        public void CheckDatabase()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                if (!dbEntities.Database.Exists())
                {
                    MessageBox.Show("De database word niet gevonden. (extra info als database online is)", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    Overal.overzichtWindow.Show();
                }
            }
        }

        private void buttonFactuur_Click(object sender, RoutedEventArgs e)
        {
            Window factuur = new FactuurWindow();
            factuur.Show();
            isClosedByButton = true;
            this.Close();
        }

        private void buttonKlant_Click(object sender, RoutedEventArgs e)
        {
            Window klant = new KlantWindow();
            klant.Show();
            isClosedByButton = true;
            this.Close();
        }

        private void buttonArtikel_Click(object sender, RoutedEventArgs e)
        {
            Window artikel = new ArtikelWindow();
            artikel.Show();
            isClosedByButton = true;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Overal.overzichtWindow != null && isClosedByButton)
            {
                Overal.overzichtWindow.Hide();
            }
            else
            {
                Overal.overzichtWindow.Close();
            }
        }
    }
}
