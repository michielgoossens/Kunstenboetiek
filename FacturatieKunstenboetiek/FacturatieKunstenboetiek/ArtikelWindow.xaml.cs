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

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for ArtikelWindow.xaml
    /// </summary>
    public partial class ArtikelWindow : Window
    {
        private int _noOfErrorsOnScreen = 0;
        private Artikel _artikel;
        public ArtikelWindow()
        {
            InitializeComponent();
            startUp();
            fillSoortCombobox();
            tbNaam.Focus();
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
            _artikel = new Artikel();
            grid.DataContext = _artikel;
            setId();
            (Application.Current as FacturatieKunstenboetiek.App).Openen = null;
            (Application.Current as FacturatieKunstenboetiek.App).Opgeslagen = null;
        }

        private void setId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                int maxArtikelId;
                if (dbEntities.Artikels.Any())
                {
                    maxArtikelId = Convert.ToInt32(dbEntities.Database.SqlQuery<decimal>("Select IDENT_CURRENT ('Artikels')", new object[0]).FirstOrDefault());
                }
                else
                {
                    maxArtikelId = 0;
                }
                textBlockArtikelNr.Text = (maxArtikelId + 1).ToString().PadLeft((Application.Current as FacturatieKunstenboetiek.App).padLeft, '0');

            }
        }

        private void fillSoortCombobox()
        {
            List<string> Soorten = new List<string>()
            {
                "Urne",
                "Mini-urne",
                "Andere werken"
            };

            foreach (var soort in Soorten)
            {
                tbSoort.Items.Add(soort);
            }
        }

        private void tbPrijs_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPrijs.Text == "")
                tbPrijs.Text = null;
        }

        private void NewArtikel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        { 
            e.CanExecute = tbNaam.Text != "" || tbKleur.Text != "" || tbSoort.SelectedValue != null || tbPrijs.Text != "";
            e.Handled = true;
        }

        private void NewArtikel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je een nieuw artikel wilt starten?", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                startUp();
            }
        }
        private void AddArtikel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void AddArtikel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                var a = dbEntities.Artikels.Find(int.Parse(textBlockArtikelNr.Text));
                if (a == null)
                {
                    if (MessageBox.Show("Ben je zeker dat je het artikel wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        Artikel artikel = grid.DataContext as Artikel;
                        dbEntities.Artikels.Add(artikel);
                        dbEntities.SaveChanges();
                        startUp();
                    }
                }
                else
                {
                    if (MessageBox.Show("Ben je zeker dat je het artikel wilt overschrijven?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        a.Naam = tbNaam.Text;
                        a.Kleur = tbKleur.Text;
                        a.Soort = tbSoort.Text;
                        a.Prijs = double.Parse(tbPrijs.Text);
                        dbEntities.SaveChanges();
                        startUp();
                    }
                }
            }
        }
        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow window = new OpenWindow("artikel");
            window.ShowDialog();

            if ((Application.Current as FacturatieKunstenboetiek.App).Openen == true)
            {
                _artikel = (Application.Current as FacturatieKunstenboetiek.App).teOpenenArtikel;
                grid.DataContext = _artikel;
                textBlockArtikelNr.Text = _artikel.ArtikelNr.ToString().PadLeft((Application.Current as FacturatieKunstenboetiek.App).padLeft, '0');
            }

            (Application.Current as FacturatieKunstenboetiek.App).Openen = null;
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
