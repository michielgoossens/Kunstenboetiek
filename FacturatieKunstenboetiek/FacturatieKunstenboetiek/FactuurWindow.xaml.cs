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
        private double totaalExclBtw;
        private double totaalInclBtw;
        private bool opgeslagen;

        public FactuurWindow()
        {
            InitializeComponent();
            startUp();
            fillKlantAutoCompleteBox();
            fillArtikelAutoCompleteBox();
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
                tbKlant.ItemTemplate = Overal.ZoekLayout("klant");
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
                tbArtikel.ItemTemplate = Overal.ZoekLayout("artikel");
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
            tbKlant.Text = string.Empty;
            tbFactuurRegels.Items.Clear();
            totaalExclBtw = 0;
            totaalInclBtw = 0;
            labelExclBtw.Content = totaalExclBtw;
            labelInclBtw.Content = totaalInclBtw;
            Overal.Openen = null;
            opgeslagen = false;
            tbKlant.Focus();
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
                textBlockFactuurNr.Text = (maxFactuurId + 1).ToString().PadLeft(Overal.padLeft, '0');
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
                        labelExclBtw.Content = totaalExclBtw + " €";
                        totaalInclBtw += artikel.prijsInclBtw;
                        labelInclBtw.Content = totaalInclBtw + " €";
                        tbArtikel.Text = string.Empty;

                    }
                }
            }
        }

        private void buttonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            Artikel artikel = tbFactuurRegels.SelectedItem as Artikel;
            totaalExclBtw -= artikel.Prijs;
            totaalInclBtw = totaalExclBtw * 1.06;
            tbFactuurRegels.Items.Remove(tbFactuurRegels.SelectedItem);
            labelExclBtw.Content = totaalExclBtw;
            labelInclBtw.Content = totaalInclBtw;
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
            if (MessageBox.Show("Ben je zeker dat je het venster wilt sluiten?", "Close Application", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Window main = new MainWindow();
            main.Show();
        }

        private void AddFactuur_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void AddFactuur_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                var f = dbEntities.Facturen.Find(int.Parse(textBlockFactuurNr.Text));
                Klant klant = tbKlant.SelectedItem as Klant;
                if (f == null)
                {
                    if (MessageBox.Show("Ben je zeker dat je het factuur wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        Factuur factuur = new Factuur();
                        dbEntities.Klanten.Attach(klant);
                        factuur.Klant = klant;
                        factuur.Datum = tbDatum.Text;

                        foreach (var regel in tbFactuurRegels.Items)
                         {
                             FactuurRegel fRegel = new FactuurRegel();
                             Artikel artikel = regel as Artikel;
                             if (artikel != null)
                             {
                                dbEntities.Artikels.Attach(artikel);
                                 fRegel.Artikel = artikel;
                             }
                             factuur.FactuurRegels.Add(fRegel);
                         }
                        dbEntities.Facturen.Add(factuur);
                        dbEntities.SaveChanges();
                        opgeslagen = true;
                        MessageBox.Show("Het factuur is goed opgeslagen", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    f.Klant = klant;
                    f.Datum = tbDatum.Text;

                    //oude regels verwijderen en nieuwe toevoegen

                    dbEntities.SaveChanges();
                    opgeslagen = true;
                    MessageBox.Show("Het factuur is goed opgeslagen", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void NewFactuur_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbKlant.Text != string.Empty  || tbDatum.Text != string.Empty || tbFactuurRegels.Items.Count != 0;
            e.Handled = true;
        }

        private void NewFactuur_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je een nieuw formuleer wilt starten?", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                startUp();
            }
        }

        private void PrintFactuur_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbKlant.Text != string.Empty && tbDatum.Text != string.Empty;
        }

        private void PrintFactuur_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (opgeslagen == true)
            {
                //print window openen
            }
            else
            {
                if (MessageBox.Show("Gelieve het factuur eerst op te slagen voor dat je gaat printen.", "Printen",MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    AddFactuur_Executed(sender, e);
                }
            }
        }

        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow window = new OpenWindow("factuur");
            window.ShowDialog();

            if (Overal.Openen == true)
            {
                _factuur = Overal.teOpenenFactuur;
                grid.DataContext = _factuur;
                textBlockFactuurNr.Text = _factuur.FactuurNr.ToString().PadLeft(Overal.padLeft, '0');
                using (var dbEntities = new KunstenboetiekDbEntities())
                {
                    Artikel artikel = new Artikel();
                    List<FactuurRegel> factuurregels = (from regel in dbEntities.FactuurRegels
                                                        where regel.FactuurNr == _factuur.FactuurNr
                                                        select regel).ToList();
                    foreach (var regel in factuurregels)
                    {
                        artikel = dbEntities.Artikels.Find(regel.ArtikelNr);
                        totaalExclBtw += artikel.Prijs;
                        tbFactuurRegels.Items.Add(artikel);
                    }
                    totaalInclBtw = totaalExclBtw * 1.06;
                }
                labelExclBtw.Content = totaalExclBtw;
                labelInclBtw.Content = totaalInclBtw;
                opgeslagen = true;
            }

            Overal.teOpenenFactuur = null;
            Overal.Openen = null;
        }
    }
}
