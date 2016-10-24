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

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for KlantWindow.xaml
    /// </summary>
    public partial class KlantWindow : Window
    {
        private int _noOfErrorsOnScreen = 0;
        private Klant _klant = new Klant();
        private int padding = 3;
        public KlantWindow()
        {
            InitializeComponent();
            grid.DataContext = _klant;
            setId();
            (Application.Current as FacturatieKunstenboetiek.App).Openen = null;
            fillLandCombobox();
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
            if (MessageBox.Show("Ben je zeker dat je de klant wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                Klant klant = grid.DataContext as Klant;

                using (var dbEntities = new KunstenboetiekDbEntities())
                {
                    dbEntities.Klanten.Add(klant);
                    dbEntities.SaveChanges();
                }

                _klant = new Klant();
                grid.DataContext = _klant;
                setId();
                (Application.Current as FacturatieKunstenboetiek.App).Openen = null;
            }
        }

        private void NewKlant_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbFamilienaam.Text != "" || tbVoornaam.Text != "" || tbStraat.Text != "" || tbHuisNr.Text != "" || tbPostcode.Text != "" || tbGemeente.Text != "";
            e.Handled = true;
        }

        private void NewKlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je een nieuwe klant wilt starten?", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                _klant = new Klant();
                grid.DataContext = _klant;
                setId();
                (Application.Current as FacturatieKunstenboetiek.App).Openen = null;
            }
        }

        private void setId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                int maxKlantId = Convert.ToInt32(dbEntities.Database.SqlQuery<decimal>("Select IDENT_CURRENT ('Klanten')", new object[0]).FirstOrDefault());

                textBlockKlantNr.Text = (maxKlantId + 1).ToString().PadLeft(padding, '0');
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

            if ((Application.Current as FacturatieKunstenboetiek.App).Openen == true)
            {
                _klant = (Application.Current as FacturatieKunstenboetiek.App).teOpenenKlant;
                if (_klant == null)
                {
                    MessageBox.Show("De klant die je probeert te laden bestaat niet.", "Openen", MessageBoxButton.OK, MessageBoxImage.Information);
                    menuItemOpen_Click(sender, e);
                }
                else
                {
                    grid.DataContext = _klant;
                    textBlockKlantNr.Text = _klant.KlantNr.ToString().PadLeft(padding, '0');
                }
            }


            (Application.Current as FacturatieKunstenboetiek.App).Openen = null;
        }
    }
}