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
using System.Windows.Shapes;

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for OpenWindow.xaml
    /// </summary>
    public partial class OpenWindow : Window
    {
        public OpenWindow(string teOpenen)
        {
            InitializeComponent();
            textBlockTeOpenen.Text = teOpenen;
        }

        private void ButtonAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as FacturatieKunstenboetiek.App).Openen = false;
            this.Close();
        }

        private void buttonOpenen_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxTeOpenen.Text != "")
            {
                if (textBlockTeOpenen.Text == "klant")
                {
                    using (var dbEntities = new KunstenboetiekDbEntities())
                    {
                        var klant = (from k in dbEntities.Klanten
                                     where k.KlantNr == int.Parse(textBlockTeOpenen.Text)
                                     select k).FirstOrDefault();
                        (Application.Current as FacturatieKunstenboetiek.App).teOpenenKlant = klant;
                    }
                }
                (Application.Current as FacturatieKunstenboetiek.App).Openen = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Gelieve een naam of nummer te geven!", "Openen", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
