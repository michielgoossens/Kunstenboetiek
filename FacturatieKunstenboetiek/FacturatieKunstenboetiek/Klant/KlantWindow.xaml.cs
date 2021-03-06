﻿using System;
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
        private int _noOfErrorsOnScreen = 0;//declare int to keep track of errors on screen
        private Klant klant;//declare klant to work with
        private int klantNr;//declare klantNr to work with
        public KlantWindow()
        {
            InitializeComponent();
            resetKlant();
            fillLandCombobox();
        }
        //clear grid and add new klant to it datacontext
        public void resetKlant()
        {
            klant = new Klant();
            grid.DataContext = klant;
            resetId(); //set next first empty id
            tbVoornaam.Focus(); //focus on voornaam to start validation
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
                klant = dbEntities.Klanten.Find(klantNr);
                if (klant == null)
                {
                    if (MessageBox.Show("Ben je zeker dat je de klant wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        klant = grid.DataContext as Klant;
                        dbEntities.Klanten.Add(klant);
                        dbEntities.SaveChanges();
                        MessageBox.Show("De klant is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);

                        Overal.overzichtWindow.setUpKlanten();
                        Overal.overzichtWindow.tabControlOverzicht.SelectedIndex = 1;
                        resetKlant();
                    }
                }
                else
                {
                    if (MessageBox.Show("Ben je zeker dat je de klant wilt overschrijven?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        klant.Voornaam = tbVoornaam.Text;
                        klant.Familienaam = tbFamilienaam.Text;
                        klant.Straat = tbStraat.Text;
                        klant.HuisNr = tbHuisNr.Text;
                        klant.Postcode = tbPostcode.Text;
                        klant.Gemeente = tbGemeente.Text;
                        klant.Land = tbLand.Text;
                        klant.Telefoon = tbTelefoon.Text;
                        klant.Email = tbEmail.Text;
                        klant.BtwNr = tbBtwNr.Text;
                        dbEntities.SaveChanges();
                        MessageBox.Show("De klant is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);

                        Overal.overzichtWindow.setUpKlanten();
                        Overal.overzichtWindow.tabControlOverzicht.SelectedIndex = 1;
                        resetKlant();
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
            if (MessageBox.Show("Ben je zeker dat je een nieuwe klant wilt starten? De huidige klant word niet opgeslagen.", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                resetKlant();
            }
        }

        private void resetId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                if (dbEntities.Klanten.Any())
                {
                    klantNr = dbEntities.Klanten.Max(k => k.KlantNr) + 1;
                }
                else
                {
                    klantNr = 1;
                }
                textBlockKlantNr.Text = (klantNr).ToString().PadLeft(Overal.padLeft, '0');
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
            if (MessageBox.Show("Ben je zeker dat je een klant wilt openen? De huidige klant word niet opgeslagen.", "Close Application", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                OpenWindow window = new OpenWindow("klant");
            window.ShowDialog();

            if (Overal.Openen == true)
            {
                resetKlant();
                klant = Overal.teOpenenKlant;
                klantNr = Overal.teOpenenKlant.KlantNr;

                grid.DataContext = klant;
                textBlockKlantNr.Text = (klantNr).ToString().PadLeft(Overal.padLeft, '0');
            }


            Overal.Openen = null;
            Overal.teOpenenKlant = null;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je het venster wilt sluiten? De huidige klant word niet opgeslagen.", "Close Application", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Window main = new MainWindow();
            main.Show();
        }

        //update tbfamilienaam
        private void tbVoornaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbFamilienaam.Text == string.Empty)
            {
                tbFamilienaam.Text = "";
            }
        }
        //update tbvoornaam
        private void tbFamilienaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbVoornaam.Text == string.Empty)
            {
                tbVoornaam.Text = "";
            }
        }
    }
}