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
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Interop;

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for KlantWindow.xaml
    /// </summary>
    public partial class KlantWindow : Window
    {
        private int _noOfErrorsOnScreen = 0;
        private Klant _klant;
        public KlantWindow()
        {
            InitializeComponent();
            startUp();
            fillLandCombobox();
        }
        public void startUp()
        {
            _klant = new Klant();
            grid.DataContext = _klant;
            setId();
            Overal.Openen = null;
            tbVoornaam.Focus();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void AddKlant_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void AddKlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                var k = dbEntities.Klanten.Find(int.Parse(textBlockKlantNr.Text));
                if (k == null)
                {
                    if (MessageBox.Show("Ben je zeker dat je de klant wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        Klant klant = grid.DataContext as Klant;
                        dbEntities.Klanten.Add(klant);
                        dbEntities.SaveChanges();

                        startUp();
                    }
                }
                else
                {
                    if (MessageBox.Show("Ben je zeker dat je de klant wilt overschrijven?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        k.Voornaam = tbVoornaam.Text;
                        k.Familienaam = tbFamilienaam.Text;
                        k.Straat = tbStraat.Text;
                        k.HuisNr = tbHuisNr.Text;
                        k.Postcode = tbPostcode.Text;
                        k.Gemeente = tbGemeente.Text;
                        k.Land = tbLand.Text;
                        k.Telefoon = tbTelefoon.Text;
                        k.Email = tbEmail.Text;
                        k.BtwNr = tbBtwNr.Text;
                        dbEntities.SaveChanges();

                        startUp();
                    }
                }
            }
        }

        private void NewKlant_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbFamilienaam.Text != "" || tbVoornaam.Text != "" || tbStraat.Text != "" || tbHuisNr.Text != "" || tbPostcode.Text != "" || tbGemeente.Text != "" || tbLand.SelectedValue != null || tbTelefoon.Text != "" || tbEmail.Text != "" || tbBtwNr.Text != "";
            e.Handled = true;
        }

        private void NewKlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je een nieuwe klant wilt starten?", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                startUp();
            }
        }

        private void setId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                int maxKlantId;
                if (dbEntities.Klanten.Any())
                {
                    maxKlantId = Convert.ToInt32(dbEntities.Database.SqlQuery<decimal>("Select IDENT_CURRENT ('Klanten')", new object[0]).FirstOrDefault());
                }
                else
                {
                    maxKlantId = 0;
                }
                textBlockKlantNr.Text = (maxKlantId + 1).ToString().PadLeft(Overal.padLeft, '0');
            }
        }

        private void fillLandCombobox()
        {
            List<string> Landen = new List<string>()
            {
                "Oostenrijk",
                "België",
                "Bulgarije",
                "Cyprus",
                "Tsjechië",
                "Duitsland",
                "Denemarken",
                "Estland",
                "Griekenland",
                "Spanje",
                "Finland",
                "Frankrijk",
                "Verenigd Koninkrijk",
                "Kroatië",
                "Hongarije",
                "Ierland",
                "Italië",
                "Litouwen",
                "Luxemburg",
                "Letland",
                "Malta",
                "Nederland",
                "Polen",
                "Portugal",
                "Roemenië",
                "Zweden",
                "Slovenië",
                "Slowakije"
            };

            Landen = Landen.OrderBy(c => c).ToList();

            foreach (var land in Landen)
            {
                tbLand.Items.Add(land);
            }
        }

        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow window = new OpenWindow("klant");
            window.ShowDialog();

            if (Overal.Openen == true)
            {
                _klant = Overal.teOpenenKlant;

                grid.DataContext = _klant;
                textBlockKlantNr.Text = _klant.KlantNr.ToString().PadLeft(Overal.padLeft, '0');
            }


            Overal.Openen = null;
            Overal.teOpenenKlant = null;
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
            Overal.teOpenenKlant = null;
            Overal.Openen = null;
            Window main = new MainWindow();
            main.Show();
        }
    }
}