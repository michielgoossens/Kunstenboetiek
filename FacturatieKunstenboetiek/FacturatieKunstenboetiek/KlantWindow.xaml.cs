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

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for KlantWindow.xaml
    /// </summary>
    public partial class KlantWindow : Window
    {
        private int _noOfErorsOnScreen = 0;
        private Klant _klant = new Klant();
        public KlantWindow()
        {
            InitializeComponent();
            grid.DataContext = _klant;
            setId();
            fillLandCombobox();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErorsOnScreen++;
            else
                _noOfErorsOnScreen--;
        }

        private void AddKlant_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErorsOnScreen == 0;
            e.Handled = true;
        }

        private void AddKlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Klant klant = grid.DataContext as Klant;
            // write code here to add Customer

            // reset UI
            NewKlant_Executed(sender, e);
            e.Handled = true;

        }

        private void NewKlant_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tbFamilienaam.Text != "" || tbVoornaam.Text != "" || tbStraat.Text != "" || tbHuisNr.Text != "" || tbPostcode.Text != "" || tbGemeente.Text != "";
            e.Handled = true;
        }

        private void NewKlant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _klant = new Klant();
            grid.DataContext = _klant;
            setId();
        }

        private void setId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                var LaatsteKlant = ((from klant in dbEntities.Klanten
                                    select klant).ToList()).LastOrDefault();
                if (LaatsteKlant != null)
                {
                    textBlockKlantNr.Text = (LaatsteKlant.KlantNr + 1).ToString();
                }
                else
                {
                    textBlockKlantNr.Text = "001";
                }
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
    }
}
