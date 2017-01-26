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
        private int _noOfErrorsOnScreen = 0; //declare count of errors on screen
        private Artikel _artikel;//declare object for artikel
        private int artikelNr;//declare int for artikelNr
        public ArtikelWindow()
        {
            Overal.teOpenenArtikel = null;
            InitializeComponent();
            resetArtikel(); //clear grid with new artikel
            fillSoortCombobox(); //fill combobox with all possible types
        }

        //Count errors on screen
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        //new empty artikel to fill grid
        private void resetArtikel()
        {
            _artikel = new Artikel();
            grid.DataContext = _artikel; //fill grid with new artikel
            resetId(); //reset id with next AI
            listBoxAfbeeldingen.Items.Clear();
            tbNaam.Focus(); //focus textblock naam to start validation
        }

        //reset textblock for id
        private void resetId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                //if allready artikels in db use the next AI for artikelNr
                if (dbEntities.Artikels.Any())
                {
                    artikelNr = dbEntities.Artikels.Max(a => a.ArtikelNr) + 1;
                }
                //else use 1 as artikelNr
                else
                {
                    artikelNr = 1;
                }
                textBlockArtikelNr.Text = artikelNr.ToString().PadLeft(Overal.padLeft, '0');

            }
        }

        //fill combobox with all posible types
        private void fillSoortCombobox()
        {
            List<string> Soorten = new List<string>()
            {
                "Urne",
                "Mini urne",
                "Dieren urne",
                "Andere werken"
            };

            foreach (var soort in Soorten)
            {
                tbSoort.Items.Add(soort);
            }
        }

        //if text in tb prijs changes to "", reset the tb
        private void tbPrijs_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPrijs.Text == "")
                tbPrijs.Text = null;
        }

        //check if possible to start new artikel
        private void NewArtikel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbNaam.Text != "" || tbKleur.Text != "" || tbSoort.SelectedValue != null || double.Parse(tbPrijs.Text.Substring(0, tbPrijs.Text.Length - 2)) > 0;
            e.Handled = true;
        }

        //start new artikel
        private void NewArtikel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je een nieuw artikel wilt starten?", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                Overal.teOpenenArtikel = null;
                resetArtikel();
            }
        }
        //check if it is possible to save artikel
        private void AddArtikel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }
        //save the artikel
        private void AddArtikel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                var a = dbEntities.Artikels.Find(artikelNr);
                if (a == null)
                {
                    if (MessageBox.Show("Ben je zeker dat je het artikel wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        Artikel artikel = grid.DataContext as Artikel;
                        artikel.Verkocht = false;

                        Ftp ftpClient = new Ftp(@"ftp://ftp.kunstenboetiek.be/test/", "kunstenboetiek.be", "m.b.v.");
                        int count = 0;
                        foreach (var afbeelding in listBoxAfbeeldingen.Items)
                        {
                            count += 1;
                            ArtikelAfbeelding aA = afbeelding as ArtikelAfbeelding;
                            string remote;
                            string local;
                            if (aA.AfbeeldingLink.Substring(0, 7) != "http://")
                            {
                                remote = artikel.Naam.Replace(' ', '_').ToLower() + count.ToString() + ".jpg";
                                local = aA.AfbeeldingLink;
                                ftpClient.upload(remote, local);
                                aA.AfbeeldingLink = "http://www.kunstenboetiek.be/test/" + remote;
                            }
                            aA.ArtikelNr = artikelNr;
                            dbEntities.ArtikelsAfbeeldingen.Add(aA);
                        }
                        artikel.Datum = DateTime.Now;
                        dbEntities.Artikels.Add(artikel);
                        dbEntities.SaveChanges();
                        MessageBox.Show("Het artikel is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);

                        resetArtikel();
                    }
                }
                else
                {
                    if (MessageBox.Show("Ben je zeker dat je het artikel wilt overschrijven?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        a.Naam = tbNaam.Text;
                        a.Kleur = tbKleur.Text;
                        a.Soort = tbSoort.Text;
                        int indexOfEuro = tbPrijs.Text.IndexOf('€');
                        a.Prijs = double.Parse(tbPrijs.Text.Substring(0, indexOfEuro));
                        if (a.Verkocht != true)
                        {
                            a.Verkocht = false;
                        }
                        List<ArtikelAfbeelding> artikelAfbeeldingen = (from ab in dbEntities.ArtikelsAfbeeldingen
                                                                       where ab.ArtikelNr == artikelNr
                                                                       select ab).ToList();

                        foreach (var afbeelding in artikelAfbeeldingen)
                        {
      
                            dbEntities.ArtikelsAfbeeldingen.Remove(afbeelding);
                        }
                        Ftp ftpClient = new Ftp(@"ftp://ftp.kunstenboetiek.be/test/", "kunstenboetiek.be", "m.b.v.");
                        int count = 0;
                        foreach (var afbeelding in listBoxAfbeeldingen.Items)
                        {
                            count += 1;
                            ArtikelAfbeelding aA = afbeelding as ArtikelAfbeelding;
                            string remote;
                            string local;
                            if (aA.AfbeeldingLink.Substring(0, 7) != "http://")
                            {
                                remote = a.Naam.Replace(' ', '_').ToLower() + count.ToString() + ".jpg";
                                local = aA.AfbeeldingLink;
                                ftpClient.upload(remote, local);
                                aA.AfbeeldingLink = "http://www.kunstenboetiek.be/test/" + remote;
                            } 
                            aA.ArtikelNr = artikelNr;
                            dbEntities.ArtikelsAfbeeldingen.Add(aA);
                        }
                        a.Info = tbInfo.Text;
                        a.Datum = DateTime.Now;
                        dbEntities.SaveChanges();
                        MessageBox.Show("Het artikel is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);

                        resetArtikel();
                    }
                }
            }
        }
        //open an existing artikel
        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow window = new OpenWindow("artikel");
            window.ShowDialog();

            if (Overal.Openen == true)
            {
                resetArtikel();
                _artikel = Overal.teOpenenArtikel;
                artikelNr = Overal.teOpenenArtikel.ArtikelNr;
                grid.DataContext = _artikel;
                textBlockArtikelNr.Text = artikelNr.ToString().PadLeft(Overal.padLeft, '0');
                using (var dbEntities = new KunstenboetiekDbEntities())
                {
                    Artikel artikel = new Artikel();
                    List<ArtikelAfbeelding> artikelAfbeeldingen = (from aA in dbEntities.ArtikelsAfbeeldingen
                                                        where aA.ArtikelNr == artikelNr
                                                        select aA).ToList();
                    foreach (var aA in artikelAfbeeldingen)
                    {
                        listBoxAfbeeldingen.Items.Add(aA);
                    }
                }
            }
            Overal.Openen = null;
        }
        //ask if its ok to close the window
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je het venster wilt sluiten?", "Close Application", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        //close the window and open mainwindow
        private void Window_Closed(object sender, EventArgs e)
        {
            Window main = new MainWindow();
            main.Show();
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
                    ArtikelAfbeelding artikelAfbeelding = new ArtikelAfbeelding { AfbeeldingLink = dlg.FileName, ArtikelNr = artikelNr };
                    listBoxAfbeeldingen.Items.Add(artikelAfbeelding);
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
                listBoxAfbeeldingen.Items.Remove(listBoxAfbeeldingen.SelectedItem);
            }
            tbNaam.Focus();
        }

        private void listBoxAfbeeldingen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxAfbeeldingen.SelectedItem != null)
            {
                AfbeeldingWindow window = new AfbeeldingWindow((listBoxAfbeeldingen.SelectedItem as ArtikelAfbeelding).AfbeeldingLink);
                window.Show();
            }
        }
    }
}
