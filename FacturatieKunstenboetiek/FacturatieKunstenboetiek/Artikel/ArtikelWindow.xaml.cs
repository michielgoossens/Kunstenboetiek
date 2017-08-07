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
using System.Windows.Interop;
using Microsoft.Win32;

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for ArtikelWindow.xaml
    /// </summary>
    public partial class ArtikelWindow : Window
    {
        private int _noOfErrorsOnScreen = 0;//declare int to keep track of errors on screen
        private Artikel artikel;
        private int artikelNr;
        int count;
        private List<ArtikelAfbeelding> toegevoegdeAfbeeldingen;
        private List<ArtikelAfbeelding> verwijderdeAfbeeldingen;

        public ArtikelWindow()
        {
            InitializeComponent();
            resetArtikel();
            fillSoortCombobox();
        }
        public void resetArtikel()
        {
            artikel = new Artikel();
            count = 0;
            toegevoegdeAfbeeldingen = new List<ArtikelAfbeelding>();
            verwijderdeAfbeeldingen = new List<ArtikelAfbeelding>();
            listBoxAfbeeldingen.Items.Clear();
            grid.DataContext = artikel;
            resetId(); //set next first empty id
            tbNaam.Focus(); //focus on naam to start validation
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }
        private void AddArtikel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }
        private void AddArtikel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxAfbeeldingen.Items.Count > 0 && tbSoort.SelectedItem == null)
            {
                MessageBox.Show("Wanneer je afbeeldingen toevoegd moet je ook een soort aanduiden.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string remote;
                string local;
                using (var dbEntities = new KunstenboetiekDbEntities())
                {
                    artikel = (from a in dbEntities.Artikels.Include("artikelAfbeeldingen")
                               where a.ArtikelNr == artikelNr
                               select a).FirstOrDefault();
                    if (artikel == null)
                    {
                        if (MessageBox.Show("Ben je zeker dat je het artikel wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                        {
                            artikel = grid.DataContext as Artikel;

                            Ftp ftpClient = new Ftp(@"ftp://ftp.kunstenboetiek.be", "ftpafbeeldingen", "KunstenBoetiek...123");
                            foreach (ArtikelAfbeelding afbeelding in listBoxAfbeeldingen.Items)
                            {
                                count += 1;
                                remote = (artikel.Soort.Replace(' ', '_') + "/") + artikel.ArtikelNr + "_" + artikel.Naam.Replace(' ', '_').ToLower() + count.ToString() + ".jpg";
                                local = afbeelding.AfbeeldingLink;
                                ftpClient.upload(remote, local);
                                afbeelding.AfbeeldingLink = "http://www.kunstenboetiek.be/Images/Galerij/" + remote;

                                afbeelding.ArtikelNr = artikelNr;
                                dbEntities.ArtikelsAfbeeldingen.Add(afbeelding);
                            }

                            artikel.Verkocht = false;
                            artikel.Datum = DateTime.Now;
                            dbEntities.Artikels.Add(artikel);

                            dbEntities.SaveChanges();
                            MessageBox.Show("Het artikel is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);

                            Overal.overzichtWindow.setUpArtikels();
                            Overal.overzichtWindow.tabControlOverzicht.SelectedIndex = 2;
                            resetArtikel();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Ben je zeker dat je het artikel wilt overschrijven?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                        {
                            artikel.Naam = tbNaam.Text;
                            artikel.Kleur = tbKleur.Text;
                            artikel.Soort = tbSoort.Text;
                            artikel.Info = tbInfo.Text;
                            int indexOfEuro = tbPrijs.Text.IndexOf('€');
                            artikel.Prijs = double.Parse(tbPrijs.Text.Substring(0, indexOfEuro));

                            Ftp ftpClient = new Ftp(@"ftp://ftp.kunstenboetiek.be", "ftpafbeeldingen", "KunstenBoetiek...123");
                            foreach (ArtikelAfbeelding afbeelding in verwijderdeAfbeeldingen)
                            {
                                ArtikelAfbeelding aA = dbEntities.ArtikelsAfbeeldingen.Find(afbeelding.AfbeeldingNr);
                                if (aA != null)
                                {
                                    count -= 1;
                                    dbEntities.ArtikelsAfbeeldingen.Remove(aA);
                                }
                            }
                            foreach (ArtikelAfbeelding afbeelding in toegevoegdeAfbeeldingen)
                            {
                                count += 1;
                                remote = (artikel.Soort.Replace(' ', '_') + "/") + artikelNr + "_" + artikel.Naam.Replace(' ', '_').ToLower() + count.ToString() + ".jpg";
                                local = afbeelding.AfbeeldingLink;
                                ftpClient.upload(remote, local);
                                afbeelding.AfbeeldingLink = "http://www.kunstenboetiek.be/Images/Galerij/" + remote;


                                afbeelding.ArtikelNr = artikelNr;
                                dbEntities.ArtikelsAfbeeldingen.Add(afbeelding);
                            }

                            artikel.Datum = DateTime.Now;

                            dbEntities.SaveChanges();
                            MessageBox.Show("Het artikel is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);

                            Overal.overzichtWindow.setUpArtikels();
                            Overal.overzichtWindow.tabControlOverzicht.SelectedIndex = 2;
                            resetArtikel();
                        }
                    }
                }
            }
        }
        //check if possible to start new artikel
        private void NewArtikel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbNaam.Text != "" || tbKleur.Text != "" || tbSoort.SelectedValue != null || double.Parse(tbPrijs.Text.Substring(0, tbPrijs.Text.Length - 2)) > 0 || listBoxAfbeeldingen.Items.Count > 0;
            e.Handled = true;
        }

        //start new artikel
        private void NewArtikel_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            if (MessageBox.Show("Ben je zeker dat je een nieuw artikel wilt starten? Het huidige artikel word niet opgeslagen.", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                resetArtikel();
            }
        }
        private void resetId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                if (dbEntities.Artikels.Any())
                {
                    artikelNr = dbEntities.Artikels.Max(a => a.ArtikelNr) + 1;
                }
                else
                {
                    artikelNr = 1;
                }
                textBlockArtikelNr.Text = (artikelNr).ToString().PadLeft(Overal.padLeft, '0');
            }
        }
        private void fillSoortCombobox()
        {
            List<string> soorten = new List<string>()
            {
                "Urne",
                "Mini urne",
                "Dieren urne",
                "Andere werken"
            };

            foreach (var soort in soorten)
            {
                tbSoort.Items.Add(soort);
            }
        }
        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je een artikel wilt openen? Het huidige artikel word niet opgeslagen.", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            { 
                OpenWindow window = new OpenWindow("artikel");
            window.ShowDialog();

            if (Overal.Openen == true)
            {
                resetArtikel();
                artikel = Overal.teOpenenArtikel;
                artikelNr = Overal.teOpenenArtikel.ArtikelNr;
                var sortedAfbeeldingen =
                    from a in artikel.ArtikelAfbeeldingen
                    orderby a.File
                    select a;
                foreach (var afbeelding in sortedAfbeeldingen)
                {
                    count += 1;
                    listBoxAfbeeldingen.Items.Add(afbeelding);
                }
                grid.DataContext = artikel;
                textBlockArtikelNr.Text = (artikelNr).ToString().PadLeft(Overal.padLeft, '0');
            }


            Overal.Openen = null;
            Overal.teOpenenArtikel = null;
            }
        }
        private void buttonAfbeeldingToevoegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "Afbeelding";
                dlg.DefaultExt = ".jpg";
                dlg.Filter = "JPEG afbeelding |*.jpg";
                if (dlg.ShowDialog() == true)
                {
                    ArtikelAfbeelding afbeelding = new ArtikelAfbeelding { AfbeeldingLink = dlg.FileName, ArtikelNr = artikelNr };
                    toegevoegdeAfbeeldingen.Add(afbeelding);
                    listBoxAfbeeldingen.Items.Add(afbeelding);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("openen mislukt : " + ex.Message);
            }
            tbNaam.Focus();
        }

        private void buttonAfbeeldingVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxAfbeeldingen.SelectedItem == null)
            {
                MessageBox.Show("Gelieve eerst in de tabel te selecteren welke afbeelding je wilt verwijderen.", "Verwijderen", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ArtikelAfbeelding afbeelding = listBoxAfbeeldingen.SelectedItem as ArtikelAfbeelding;
                toegevoegdeAfbeeldingen.Remove(afbeelding);
                verwijderdeAfbeeldingen.Add(afbeelding);
                listBoxAfbeeldingen.Items.Remove(afbeelding);
            }
            tbNaam.Focus();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je het venster wilt sluiten? Het huidige artikel word niet opgeslagen.", "Close Application", MessageBoxButton.YesNo) == MessageBoxResult.No)
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
