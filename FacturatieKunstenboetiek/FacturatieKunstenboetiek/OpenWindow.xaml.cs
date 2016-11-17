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
using System.Text.RegularExpressions;

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for OpenWindow.xaml
    /// </summary>
    public partial class OpenWindow : Window
    {
        string TeOpenen;
        public OpenWindow(string teOpenen)
        {
            InitializeComponent();
            TeOpenen = teOpenen;
            textBlockTeOpenen.Text = TeOpenen;
            fillAutoCompleteBox();
        }

        public void fillAutoCompleteBox()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                if (TeOpenen == "klant")
                {
                    List<Klant> autoCompleteKlant = new List<Klant>();
                    foreach (var k in dbEntities.Klanten)
                    {
                        autoCompleteKlant.Add(k);
                    }
                    autoCompleteBox.ItemsSource = autoCompleteKlant;
                    autoCompleteBox.ValueMemberPath = "zoekKlant";
                    autoCompleteBox.FilterMode = AutoCompleteFilterMode.Contains;
                    autoCompleteBox.ItemTemplate = Overal.ZoekLayout("klant");

                }
                else if (TeOpenen == "artikel")
                {
                    List<Artikel> autoCompleteArtikel = new List<Artikel>();
                    foreach (var a in dbEntities.Artikels)
                    {
                        autoCompleteArtikel.Add(a);
                    }
                    autoCompleteBox.ItemsSource = autoCompleteArtikel;
                    autoCompleteBox.ValueMemberPath = "zoekArtikel";
                    autoCompleteBox.FilterMode = AutoCompleteFilterMode.Contains;
                    autoCompleteBox.ItemTemplate = Overal.ZoekLayout("artikel");
                }
                else if (TeOpenen == "factuur")
                {
                    List<Factuur> autoCompleteFactuur = new List<Factuur>();
                    foreach (var f in dbEntities.Facturen)
                    {
                        Klant klant = dbEntities.Klanten.Find(f.KlantNr);
                        dbEntities.Klanten.Attach(klant);
                        autoCompleteFactuur.Add(f);

                    }
                    autoCompleteBox.ItemsSource = autoCompleteFactuur;
                    autoCompleteBox.ValueMemberPath = "zoekFactuur";
                    autoCompleteBox.FilterMode = AutoCompleteFilterMode.Contains;
                    autoCompleteBox.ItemTemplate = Overal.ZoekLayout("factuur");
                }
            }
        }
        private void ButtonAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            Overal.Openen = false;
            this.Close();
        }

        private void buttonOpenen_Click(object sender, RoutedEventArgs e)
        {
            if (TeOpenen == "klant")
            {
                Klant klant = (Klant)autoCompleteBox.SelectedItem;
                if (klant != null)
                {
                    Overal.Openen = true;
                    Overal.teOpenenKlant = klant;
                    this.Close();
                }
                else { MessageBox.Show("De klant die je probeert te openen bestaat niet.", "Openen", MessageBoxButton.OK, MessageBoxImage.Information); }
            }
            if (TeOpenen == "artikel")
            {
                Artikel artikel = (Artikel)autoCompleteBox.SelectedItem;
                if (artikel != null)
                {
                    Overal.Openen = true;
                    Overal.teOpenenArtikel = artikel;
                    this.Close();
                }
                else { MessageBox.Show("Het artikel dat je probeert te openen bestaat niet.", "Openen", MessageBoxButton.OK, MessageBoxImage.Information); }
            }
            if (TeOpenen == "factuur")
            {
                Factuur factuur = (Factuur)autoCompleteBox.SelectedItem;
                if (factuur != null)
                {
                    Overal.Openen = true;
                    Overal.teOpenenFactuur = factuur;
                    this.Close();
                }
                else { MessageBox.Show("Het factuur dat je probeert te openen bestaat niet.", "Openen", MessageBoxButton.OK, MessageBoxImage.Information); }
            }
        }

        private void autoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            string tekst = autoCompleteBox.Text;
            tekst = regex.Replace(tekst, " ");

            autoCompleteBox.Text = tekst;
        }

        
    }
}
