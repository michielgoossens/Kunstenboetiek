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

            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                if (teOpenen == "klant")
                {
                    List<Klant> autoCompleteKlant = new List<Klant>();
                    foreach (var k in dbEntities.Klanten)
                    {
                        autoCompleteKlant.Add(k);
                    }
                    autoCompleteBox.ItemsSource = autoCompleteKlant;
                    autoCompleteBox.ValueMemberPath = "zoekKlant";
                    autoCompleteBox.FilterMode = AutoCompleteFilterMode.Contains;
                    autoCompleteBox.ItemTemplate = klantLayout();

                }
                else if (teOpenen == "artikel")
                {
                    List<Artikel> autoCompleteArtikel = new List<Artikel>();
                    foreach (var a in dbEntities.Artikels)
                    {
                        autoCompleteArtikel.Add(a);
                    }
                    autoCompleteBox.ItemsSource = autoCompleteArtikel;
                    autoCompleteBox.ValueMemberPath = "zoekArtikel";
                    autoCompleteBox.FilterMode = AutoCompleteFilterMode.Contains;
                    autoCompleteBox.ItemTemplate = ArtikelLayout();
                }
            }
        }

        private void ButtonAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as FacturatieKunstenboetiek.App).Openen = false;
            this.Close();
        }

        private void buttonOpenen_Click(object sender, RoutedEventArgs e)
        {
            int index = autoCompleteBox.Text.LastIndexOf(" ");
            if (index != -1)
            {
                string idTeOpenen = autoCompleteBox.Text.Substring(index);
                int id;
                if (int.TryParse(idTeOpenen, out id))
                {
                    using (var dbEntities = new KunstenboetiekDbEntities())
                    {
                        if (TeOpenen == "klant")
                        {
                            Klant klant = dbEntities.Klanten.Find(id);
                            if (klant != null)
                            {
                                (Application.Current as FacturatieKunstenboetiek.App).teOpenenKlant = klant;
                                (Application.Current as FacturatieKunstenboetiek.App).Openen = true;
                                this.Close();
                            } else { MessageBox.Show("De klant die je wilt openen bestaat niet.", "Openen", MessageBoxButton.OK, MessageBoxImage.Information); }
                        }
                        else if (TeOpenen == "artikel")
                        {
                            Artikel artikel = dbEntities.Artikels.Find(id);
                            if (artikel != null)
                            {
                                (Application.Current as FacturatieKunstenboetiek.App).teOpenenArtikel = artikel;
                                (Application.Current as FacturatieKunstenboetiek.App).Openen = true;
                                this.Close();
                            }
                            else { MessageBox.Show("Het artikel dat je wilt openen bestaat niet.", "Openen", MessageBoxButton.OK, MessageBoxImage.Information); }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Gelieve eerst een " + TeOpenen + " te selecteren.", "Openen", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } else { MessageBox.Show("Item niet gevonden.", "Openen", MessageBoxButton.OK, MessageBoxImage.Information); }
        }

        private void autoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            autoCompleteBox.Text = regex.Replace(autoCompleteBox.Text, " ");
        }

        private DataTemplate klantLayout()
        {
            double sz1 = 80;
            double sz2 = 130;
            DataTemplate layout = new DataTemplate();
            layout.DataType = typeof(Klant);

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "klantStackPanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory Voornaam = new FrameworkElementFactory(typeof(Label));
            Voornaam.SetBinding(Label.ContentProperty, new Binding("Voornaam"));
            Voornaam.SetValue(Label.WidthProperty, sz1);
            stackPanel.AppendChild(Voornaam);

            FrameworkElementFactory Familienaam = new FrameworkElementFactory(typeof(Label));
            Familienaam.SetBinding(Label.ContentProperty, new Binding("Familienaam"));
            Familienaam.SetValue(Label.WidthProperty, sz2);
            stackPanel.AppendChild(Familienaam);

            FrameworkElementFactory KlantNr = new FrameworkElementFactory(typeof(Label));
            KlantNr.SetBinding(Label.ContentProperty, new Binding("KlantNr"));
            KlantNr.SetValue(Label.FontStyleProperty, FontStyles.Italic);
            KlantNr.SetValue(Label.ForegroundProperty, Brushes.DarkGray);
            stackPanel.AppendChild(KlantNr);

            layout.VisualTree = stackPanel;

            return layout;
        }

        private DataTemplate ArtikelLayout()
        {
            double sz = 210;
            DataTemplate layout = new DataTemplate();
            layout.DataType = typeof(Klant);

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "klantStackPanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory Naam = new FrameworkElementFactory(typeof(Label));
            Naam.SetBinding(Label.ContentProperty, new Binding("Naam"));
            Naam.SetValue(Label.WidthProperty, sz);
            stackPanel.AppendChild(Naam);

            FrameworkElementFactory ArtikelNr = new FrameworkElementFactory(typeof(Label));
            ArtikelNr.SetBinding(Label.ContentProperty, new Binding("ArtikelNr"));
            ArtikelNr.SetValue(Label.FontStyleProperty, FontStyles.Italic);
            ArtikelNr.SetValue(Label.ForegroundProperty, Brushes.DarkGray);
            stackPanel.AppendChild(ArtikelNr);

            layout.VisualTree = stackPanel;

            return layout;
        }
    }
}
