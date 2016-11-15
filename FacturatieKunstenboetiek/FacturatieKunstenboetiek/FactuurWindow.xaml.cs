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
    /// Interaction logic for FactuurWindow.xaml
    /// </summary>
    public partial class FactuurWindow : Window
    {
        private int _noOfErrorsOnScreen = 0;
        private Factuur _factuur;
        private double totaalExclBtw = 0;
        private double totaalInclBtw = 0;

        public FactuurWindow()
        {
            InitializeComponent();
            startUp();
            labelExclBtw.Content = totaalExclBtw;
            labelInclBtw.Content = totaalInclBtw;
            fillKlantAutoCompleteBox();
            fillArtikelAutoCompleteBox();
            tbKlant.Focus();
        }
        private void fillKlantAutoCompleteBox()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                List<Klant> autoCompleteKlant = new List<Klant>();
                foreach (var k in dbEntities.Klanten)
                {
                    autoCompleteKlant.Add(k);
                }
                tbKlant.ItemsSource = autoCompleteKlant;
                tbKlant.ValueMemberPath = "zoekKlant";
                tbKlant.FilterMode = AutoCompleteFilterMode.Contains;
                tbKlant.ItemTemplate = (Application.Current as FacturatieKunstenboetiek.App).klantLayout();
            }
        }
        private void fillArtikelAutoCompleteBox()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                List<Artikel> autoCompleteArtikel = new List<Artikel>();
                foreach (var a in dbEntities.Artikels)
                {
                    autoCompleteArtikel.Add(a);
                }
                tbArtikel.ItemsSource = autoCompleteArtikel;
                tbArtikel.ValueMemberPath = "zoekArtikel";
                tbArtikel.FilterMode = AutoCompleteFilterMode.Contains;
                tbArtikel.ItemTemplate = (Application.Current as FacturatieKunstenboetiek.App).ArtikelLayout();
            }
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }
        private void startUp()
        {
            _factuur = new Factuur();
            grid.DataContext = _factuur;
            setId();
            (Application.Current as FacturatieKunstenboetiek.App).Openen = null;
            (Application.Current as FacturatieKunstenboetiek.App).Opgeslagen = null;
        }
        private void setId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                int maxFactuurId;
                if (dbEntities.Facturen.Any())
                {
                    maxFactuurId = Convert.ToInt32(dbEntities.Database.SqlQuery<decimal>("Select IDENT_CURRENT ('Facturen')", new object[0]).FirstOrDefault());
                }
                else
                {
                    maxFactuurId = 0;
                }
                textBlockFactuurNr.Text = (maxFactuurId + 1).ToString().PadLeft((Application.Current as FacturatieKunstenboetiek.App).padLeft, '0');
            }
        }

        private void buttonToevoegen_Click(object sender, RoutedEventArgs e)
        {
            if (tbArtikel.Text == string.Empty)
            {
                MessageBox.Show("Gelieve eerst een artikel te selecteren.", "Toevoegen", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                using (var dbEntities = new KunstenboetiekDbEntities())
                {
                    Artikel artikel = (Artikel)tbArtikel.SelectedItem;
                    if (artikel == null)
                    {
                        MessageBox.Show("Het artikel dat je probeert toe te voegen bestaat niet.", "Toevoegen", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        tbFactuurRegels.Items.Add(artikel);
                        totaalExclBtw += artikel.Prijs;
                        labelExclBtw.Content = "€ " + totaalExclBtw;
                        totaalInclBtw += artikel.prijsInclBtw;
                        labelInclBtw.Content = "€ " + totaalInclBtw;
                        tbArtikel.Text = string.Empty;

                    }
                }
            }
        }

        private void buttonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            tbFactuurRegels.Items.Remove(tbFactuurRegels.SelectedItem);
        }
        public DataTemplate klantLayout()
        {
            double sz1 = 70;
            double fontSize = 10;
            DataTemplate layout = new DataTemplate();
            layout.DataType = typeof(Artikel);

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "ArtikelStackPanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory Naam = new FrameworkElementFactory(typeof(Label));
            Naam.SetBinding(Label.ContentProperty, new Binding("Naam"));
            Naam.SetValue(Label.WidthProperty, sz1);
            Naam.SetValue(Label.FontSizeProperty, fontSize);
            stackPanel.AppendChild(Naam);

            FrameworkElementFactory PrijsExclBtw = new FrameworkElementFactory(typeof(Label));
            PrijsExclBtw.SetBinding(Label.ContentProperty, new Binding("Prijs"));
            PrijsExclBtw.SetValue(Label.FontStyleProperty, FontStyles.Italic);
            PrijsExclBtw.SetValue(Label.ForegroundProperty, Brushes.DarkGray);
            stackPanel.AppendChild(PrijsExclBtw);

            FrameworkElementFactory PrijsInclBtw = new FrameworkElementFactory(typeof(Label));
            PrijsInclBtw.SetBinding(Label.ContentProperty, new Binding("PrijsInclBtw"));
            PrijsInclBtw.SetValue(Label.FontStyleProperty, FontStyles.Italic);
            PrijsInclBtw.SetValue(Label.ForegroundProperty, Brushes.DarkGray);
            stackPanel.AppendChild(PrijsInclBtw);

            layout.VisualTree = stackPanel;

            return layout;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je het venster wil sluiten?", "Close Application", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Window main = new MainWindow();
            main.Show();
        }
    }
}
