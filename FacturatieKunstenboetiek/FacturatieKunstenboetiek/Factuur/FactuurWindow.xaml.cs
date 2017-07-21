using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for FactuurWindow.xaml
    /// </summary>
    public partial class FactuurWindow : Window
    {
        private int _noOfErrorsOnScreen = 0;//declare int to keep track of errors on screen
        private Factuur factuur;
        private int factuurNr;
        private List<FactuurRegel> toegevoegdeRegels;
        private List<FactuurRegel> verwijderdeRegels;
        private double totaalExclBtw;
        private double totaalInclBtw;
        private bool opgeslagen;

        public FactuurWindow()
        {
            InitializeComponent();
            fillKlantAutoCompleteBox();
            resetFactuur();
        }
        public void resetFactuur()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (var artikel in dbEntities.Artikels)
                {
                    dbEntities.Entry(artikel).State = System.Data.Entity.EntityState.Detached;
                }
            }
            toegevoegdeRegels = new List<FactuurRegel>();
            verwijderdeRegels = new List<FactuurRegel>();
            fillArtikelAutoCompleteBox();
            factuur = new Factuur();
            tbKlant.Text = "";
            tbFactuurRegels.Items.Clear();
            totaalExclBtw = 0;
            totaalInclBtw = 0;
            grid.DataContext = factuur;
            labelExclBtw.Content = Math.Round(totaalExclBtw, 2).ToString() + " €";
            labelInclBtw.Content = Math.Round(totaalInclBtw, 2).ToString() + " €";
            resetId(); //set next first empty id
            tbKlant.Focus(); //focus on naam to start validation
            opgeslagen = false;
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
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
                foreach (FactuurRegel regel in tbFactuurRegels.Items)
                {
                    dbEntities.Artikels.Attach(regel.Artikel);
                }
                factuur = (from f in dbEntities.Facturen.Include("factuurRegels")
                                   where f.FactuurNr == factuurNr
                                   select f).FirstOrDefault();
                if (factuur == null)
                {
                    if (MessageBox.Show("Ben je zeker dat je het factuur wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        factuur = new Factuur();
                        factuur.KlantNr = (tbKlant.SelectedItem as Klant).KlantNr;
                        factuur.Datum = tbDatum.Text;

                        foreach (FactuurRegel regel in tbFactuurRegels.Items)
                        {
                            regel.Artikel.Verkocht = true;
                            factuur.FactuurRegels.Add(regel);
                        }

                        dbEntities.Facturen.Add(factuur);

                        dbEntities.SaveChanges();
                        MessageBox.Show("Het factuur is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
                        opgeslagen = true;
                        Overal.overzichtWindow.setUpFacturen();
                        Overal.overzichtWindow.tabControlOverzicht.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (MessageBox.Show("Ben je zeker dat je het factuur wilt overschrijven?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        factuur.KlantNr = (tbKlant.SelectedItem as Klant).KlantNr;
                        factuur.Datum = tbDatum.Text;

                        foreach (FactuurRegel regel in verwijderdeRegels)
                        {
                            FactuurRegel r = dbEntities.FactuurRegels.Find(regel.RegelNr);
                            if (r != null)
                            {
                                regel.Artikel.Verkocht = false;
                                dbEntities.FactuurRegels.Remove(r);
                            }
                        }
                        foreach (FactuurRegel regel in toegevoegdeRegels)
                        {
                            regel.FactuurNr = factuurNr;
                            regel.Artikel.Verkocht = true;
                            dbEntities.FactuurRegels.Add(regel);
                        }

                        dbEntities.SaveChanges();
                        MessageBox.Show("Het factuur is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
                        opgeslagen = true;
                        Overal.overzichtWindow.setUpFacturen();
                        Overal.overzichtWindow.tabControlOverzicht.SelectedIndex = 0;
                    }
                }
            }
        }
        private void NewFactuur_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbKlant.Text != string.Empty || tbDatum.Text != string.Empty || tbFactuurRegels.Items.Count != 0;
            e.Handled = true;
        }

        private void NewFactuur_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (opgeslagen)
            {
            if (MessageBox.Show("Ben je zeker dat je een nieuw factuur wilt starten?", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                resetFactuur();
            }
            }
            else
            {
                if (MessageBox.Show("Ben je zeker dat je een nieuw factuur wilt starten? De huidige factuur is nog niet opgeslagen", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    resetFactuur();
                }
            }
        }
        private void resetId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                if (dbEntities.Facturen.Any())
                {
                    factuurNr = dbEntities.Facturen.Max(f => f.FactuurNr) + 1;
                }
                else
                {
                    factuurNr = 1;
                }
                textBlockFactuurNr.Text = (factuurNr).ToString().PadLeft(Overal.padLeft, '0');
            }
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
            List<Artikel> autoCompleteArtikel = new List<Artikel>();
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (var a in dbEntities.Artikels)
                {
                    if (a.Verkocht != true)
                    {
                        autoCompleteArtikel.Add(a);
                    }
                }
                tbArtikel.ItemsSource = autoCompleteArtikel;
                tbArtikel.ValueMemberPath = "zoekArtikel";
                tbArtikel.FilterMode = AutoCompleteFilterMode.Contains;
                tbArtikel.ItemTemplate = Overal.ZoekLayout("artikel");
            }
        }
        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            if (opgeslagen)
            {
                if (MessageBox.Show("Ben je zeker dat je een factuur wilt openen?", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    Open();
                }
            }
            else if (MessageBox.Show("Ben je zeker dat je een factuur wilt openen? De huidige factuur is nog niet opgeslagen.", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                Open();
            }
        }
        private void Open()
        {
            OpenWindow window = new OpenWindow("factuur");
            window.ShowDialog();

            if (Overal.Openen == true)
            {
                resetFactuur();
                factuur = Overal.teOpenenFactuur;
                factuurNr = Overal.teOpenenFactuur.FactuurNr;
                grid.DataContext = factuur;
                var sortedFactuurRegels =
                    from f in factuur.FactuurRegels
                    orderby f.RegelNr
                    select f;
                using (var dbEntities = new KunstenboetiekDbEntities())
                {
                    foreach (var regel in sortedFactuurRegels)
                    {
                        tbFactuurRegels.Items.Add(regel);
                        totaalExclBtw += regel.Artikel.Prijs;
                        totaalInclBtw = totaalExclBtw * 1.06;
                        labelExclBtw.Content = Math.Round(totaalExclBtw, 2).ToString() + " €";
                        labelInclBtw.Content = Math.Round(totaalInclBtw, 2).ToString() + " €";
                    }
                }
                textBlockFactuurNr.Text = (factuurNr).ToString().PadLeft(Overal.padLeft, '0');
                opgeslagen = true;
            }


            Overal.Openen = null;
            Overal.teOpenenArtikel = null;
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
                    int artikelNr = (tbArtikel.SelectedItem as Artikel).ArtikelNr;
                    Artikel artikel = (from a in dbEntities.Artikels
                                       where a.ArtikelNr == artikelNr
                                       select a).FirstOrDefault();
                    FactuurRegel regel;
                    if (artikel == null)
                    {
                        MessageBox.Show("Het artikel dat je probeert toe te voegen bestaat niet.", "Toevoegen", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        regel = new FactuurRegel();
                        regel.ArtikelNr = artikelNr;
                        regel.Artikel = artikel;
                        tbFactuurRegels.Items.Add(regel);
                        toegevoegdeRegels.Add(regel);
                        totaalExclBtw += artikel.Prijs;
                        totaalInclBtw = totaalExclBtw * 1.06;
                        labelExclBtw.Content = Math.Round(totaalExclBtw, 2).ToString() + " €";
                        labelInclBtw.Content = Math.Round(totaalInclBtw, 2).ToString() + " €";
                        tbArtikel.Text = string.Empty;
                        opgeslagen = false;
                        
                    }
                }
            }
        }

        private void buttonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            FactuurRegel regel = (FactuurRegel)tbFactuurRegels.SelectedItem;
            if (regel != null)
            {
                totaalExclBtw -= regel.Artikel.Prijs;
                totaalInclBtw = totaalExclBtw * 1.06;
                toegevoegdeRegels.Remove(regel);
                verwijderdeRegels.Add(regel);
                tbFactuurRegels.Items.Remove(regel);
                labelExclBtw.Content = Math.Round(totaalExclBtw, 2).ToString() + " €";
                labelInclBtw.Content = Math.Round(totaalInclBtw, 2).ToString() + " €";
                opgeslagen = false;
            }
            else
            {
                MessageBox.Show("Gelieve eerst in de tabel te selecteren welk artikel je wilt verwijderen.", "Verwijderen", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (opgeslagen)
            {
            if (MessageBox.Show("Ben je zeker dat je het venster wilt sluiten?", "Afsluiten", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            }
            else
            {
                if (MessageBox.Show("Ben je zeker dat je het venster wilt sluiten? De huidige factuur is nog niet opgeslagen.", "Afsluiten", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Window main = new MainWindow();
            main.Show();
        }
        private void PrintFactuur_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbKlant.Text != string.Empty && tbDatum.Text != string.Empty;
        }
        private void PrintFactuur_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (opgeslagen == true)
            {
                PrintWindow preview = new PrintWindow();
                Print print = new Print();
                preview.Owner = this;
                preview.AfdrukDocument = print.Preview(factuur);
                preview.ShowDialog();
            }
            else
            {
                if (MessageBox.Show("Gelieve het factuur eerst op te slagen voor dat je gaat printen.", "Printen", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    AddFactuur_Executed(sender, e);
                }
            }
        }

        private void tbDatum_TextChanged(object sender, TextChangedEventArgs e)
        {
            opgeslagen = false;
        }

        private void tbKlant_TextChanged(object sender, RoutedEventArgs e)
        {
            opgeslagen = false;
        }
    }
}
