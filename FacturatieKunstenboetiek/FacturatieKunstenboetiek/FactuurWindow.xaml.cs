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
        private int factuurNr;
        private List<FactuurRegel> _factuurregels;
        private List<int> frNr;
        private double totaalExclBtw;
        private double totaalInclBtw;
        private bool opgeslagen;

        public FactuurWindow()
        {
            InitializeComponent();
            resetFactuur();
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
        private void resetFactuur()
        {
            _factuur = new Factuur();
            _factuurregels = new List<FactuurRegel>();
            frNr = new List<int>();
            grid.DataContext = _factuur;
            resetId();
            tbKlant.Text = string.Empty;
            tbFactuurRegels.Items.Clear();
            totaalExclBtw = 0;
            totaalInclBtw = 0;
            labelExclBtw.Content = totaalExclBtw + " €";
            labelInclBtw.Content = totaalInclBtw + " €";
            opgeslagen = false;
            tbKlant.Focus();
        }
        private void resetId()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                if (dbEntities.Facturen.Any())
                {
                    factuurNr = dbEntities.Facturen.Max(f => f.FactuurNr) + 1; ;
                }
                else
                {
                    factuurNr = 1;
                }
                textBlockFactuurNr.Text = factuurNr.ToString().PadLeft(Overal.padLeft, '0');
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
                    Artikel artikel = tbArtikel.SelectedItem as Artikel;
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

                        opgeslagen = false;
                    }
                }
            }
        }

        private void buttonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            Artikel artikel = (Artikel)tbFactuurRegels.SelectedItem;
            if (artikel != null)
            {
                totaalExclBtw -= artikel.Prijs;
                totaalInclBtw = totaalExclBtw * 1.06;
                tbFactuurRegels.Items.Remove(tbFactuurRegels.SelectedItem);
                labelExclBtw.Content = totaalExclBtw;
                labelInclBtw.Content = totaalInclBtw;

                opgeslagen = false;
            }
            else
            {
                MessageBox.Show("Gelieve eerst in de tabel te selecteren welk artikel je wilt verwijderen.", "Verwijderen", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
                Factuur factuur = dbEntities.Facturen.Find(int.Parse(textBlockFactuurNr.Text));
                if (factuur != null)
                {
                    if (MessageBox.Show("Ben je zeker dat je het factuur wilt overschrijven?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        factuur.KlantNr = (tbKlant.SelectedItem as Klant).KlantNr;
                        factuur.Datum = tbDatum.Text;

                        List<FactuurRegel> factuurRegels = (from r in dbEntities.FactuurRegels
                                                            where r.FactuurNr == factuur.FactuurNr
                                                            select r).ToList();

                        foreach (var regel in factuurRegels)
                        {
                            dbEntities.FactuurRegels.Remove(regel);
                            Artikel artikel = dbEntities.Artikels.Find(regel.ArtikelNr);
                            artikel.Verkocht = false;
                        }
                        foreach (var artikel in tbFactuurRegels.Items)
                        {
                            FactuurRegel r = new FactuurRegel();
                            Artikel a = artikel as Artikel;
                            Artikel ar = dbEntities.Artikels.Find(a.ArtikelNr);
                            ar.Verkocht = true;
                            r.ArtikelNr = ar.ArtikelNr;
                            factuur.FactuurRegels.Add(r);
                        }
                        dbEntities.SaveChanges();
                        opgeslagen = true;
                        MessageBox.Show("Het factuur is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (MessageBox.Show("Ben je zeker dat je het factuur wilt opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        Factuur f = new Factuur();
                        f.KlantNr = (tbKlant.SelectedItem as Klant).KlantNr;
                        f.Datum = tbDatum.Text;

                        foreach (var artikel in tbFactuurRegels.Items)
                        {
                            FactuurRegel r = new FactuurRegel();
                            Artikel a = artikel as Artikel;
                            Artikel ar = dbEntities.Artikels.Find(a.ArtikelNr);
                            ar.Verkocht = true;
                            r.ArtikelNr = ar.ArtikelNr;
                            f.FactuurRegels.Add(r);
                        }
                        dbEntities.Facturen.Add(f);
                        dbEntities.SaveChanges();
                        opgeslagen = true;
                        MessageBox.Show("Het factuur is goed opgeslagen.", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (MessageBox.Show("Ben je zeker dat je een nieuw formuleer wilt starten?", "Nieuw", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                resetFactuur();
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
                PrintWindow preview = new PrintWindow();
                preview.Owner = this;
                preview.AfdrukDocument = StelAfdrukSamen();
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

        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow window = new OpenWindow("factuur");
            window.ShowDialog();

            if (Overal.Openen == true)
            {
                resetFactuur();
                _factuur = Overal.teOpenenFactuur;
                factuurNr = Overal.teOpenenFactuur.FactuurNr;
                grid.DataContext = _factuur;
                textBlockFactuurNr.Text = factuurNr.ToString().PadLeft(Overal.padLeft, '0');
                using (var dbEntities = new KunstenboetiekDbEntities())
                {
                    Artikel artikel = new Artikel();
                    List<FactuurRegel> factuurregels = (from regel in dbEntities.FactuurRegels
                                                        where regel.FactuurNr == factuurNr
                                                        select regel).ToList();
                    foreach (var regel in factuurregels)
                    {
                        artikel = dbEntities.Artikels.Find(regel.ArtikelNr);
                        totaalExclBtw += artikel.Prijs;
                        tbFactuurRegels.Items.Add(artikel);
                    }
                    totaalInclBtw = totaalExclBtw * 1.06;
                }
                labelExclBtw.Content = totaalExclBtw + " €";
                labelInclBtw.Content = totaalInclBtw + " €";
                opgeslagen = true;
            }

            Overal.teOpenenFactuur = null;
            Overal.Openen = null;
        }

        private double A4breedte = 21 / 2.54 * 96;
        private double A4hoogte = 29.7 / 2.54 * 96;
        private double Afstand;
        private FontFamily font = new FontFamily("Verdana");

        private FixedDocument StelAfdrukSamen()
        {
            Afstand = 0;
            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new System.Windows.Size(A4breedte, A4hoogte);
            PageContent inhoud = new PageContent();
            document.Pages.Add(inhoud);

            FixedPage page = new FixedPage();
            inhoud.Child = page;

            page.Width = A4breedte;
            page.Height = A4hoogte;
            page.Margin = new Thickness(100, 100, 100, 100);

            page.Children.Add(Titel());
            Afstand += 60;
            page.Children.Add(GegevensKunstenboetiek());
            Afstand += 140;
            page.Children.Add(FactuurGegevens());
            Afstand += 60;
            page.Children.Add(KlantGegevens());
            Afstand += 65;
            page.Children.Add(Artikels());
            Afstand = 850;
            TextBlock infoBetaling = new TextBlock();
            infoBetaling.Text = "Gelieve het bedrag inclusief BTW binnen 30 dagen te voldoen op het bovenstaande rekeningnummer, met mededeling: FACT" + textBlockFactuurNr.Text + ".";
            infoBetaling.Margin = new Thickness(0, Afstand, 0, 0);
            infoBetaling.TextWrapping = TextWrapping.Wrap;
            infoBetaling.MaxWidth = 500;
            infoBetaling.FontFamily = font;
            page.Children.Add(infoBetaling);


            return document;
        }

        private StackPanel Titel()
        {
            StackPanel titel = new StackPanel();
            titel.Orientation = Orientation.Horizontal;

            TextBlock titelKunstenboetiek = new TextBlock();
            titelKunstenboetiek.Text = "Kunstenboetiek";
            titelKunstenboetiek.FontSize = 24;
            titelKunstenboetiek.FontFamily = font;
            titelKunstenboetiek.HorizontalAlignment = HorizontalAlignment.Left;
            titelKunstenboetiek.VerticalAlignment = VerticalAlignment.Bottom;
            titelKunstenboetiek.Margin = new Thickness(0, 0, 10, 0);

            TextBlock titelFactuur = new TextBlock();
            titelFactuur.Text = "FACTUUR";
            titelFactuur.FontSize = 28;
            titelFactuur.FontFamily = font;
            titelFactuur.HorizontalAlignment = HorizontalAlignment.Left;
            titelFactuur.VerticalAlignment = VerticalAlignment.Bottom;

            titel.Children.Add(titelKunstenboetiek);
            titel.Children.Add(titelFactuur);

            return titel;
        }
        private StackPanel GegevensKunstenboetiek()
        {
            StackPanel gegevens = new StackPanel();
            gegevens.Orientation = Orientation.Vertical;
            gegevens.Margin = new Thickness(0, Afstand, 0, 0);

            TextBlock adres = new TextBlock();
            adres.Text = "Canadalaan 9";
            adres.FontFamily = font;
            gegevens.Children.Add(adres);

            TextBlock gemeente = new TextBlock();
            gemeente.Text = "9140 Temse";
            gemeente.FontFamily = font;
            gegevens.Children.Add(gemeente);

            TextBlock gsm = new TextBlock();
            gsm.Text = "0485 34 87 86";
            gsm.FontFamily = font;
            gsm.Margin = new Thickness(0, 0, 0, 18);
            gegevens.Children.Add(gsm);

            TextBlock btwNummer = new TextBlock();
            btwNummer.Text = "BTW: BE0874670774";
            btwNummer.FontFamily = font;
            btwNummer.Margin = new Thickness(0, 0, 0, 18);
            gegevens.Children.Add(btwNummer);

            TextBlock rekeningNr = new TextBlock();
            rekeningNr.Text = "Bankrekening: BE91 9731 1638 4876";
            rekeningNr.FontFamily = font;
            gegevens.Children.Add(rekeningNr);

            return gegevens;
        }

        private StackPanel FactuurGegevens()
        {
            StackPanel gegevens = new StackPanel();
            gegevens.Orientation = Orientation.Vertical;
            gegevens.Margin = new Thickness(0, Afstand, 0, 0);

            TextBlock factuurNr = new TextBlock();
            factuurNr.Text = "Factuurnummer: " + textBlockFactuurNr.Text;
            factuurNr.FontFamily = font;
            gegevens.Children.Add(factuurNr);

            TextBlock datum = new TextBlock();
            datum.Text = "Datum: " + tbDatum.Text;
            datum.FontFamily = font;
            gegevens.Children.Add(datum);

            return gegevens;
        }

        private StackPanel KlantGegevens()
        {
            Klant klant = tbKlant.SelectedItem as Klant;

            StackPanel gegevens = new StackPanel();
            gegevens.Orientation = Orientation.Vertical;
            gegevens.Margin = new Thickness(0, Afstand, 0, 0);

            TextBlock klantNr = new TextBlock();
            klantNr.Text = "Klantnummer: " + klant.KlantNr;
            klantNr.FontFamily = font;
            klantNr.Margin = new Thickness(0, 0, 0, 18);
            gegevens.Children.Add(klantNr);

            StackPanel persoon = new StackPanel();
            persoon.Margin = new Thickness(200, 0, 0, 0);
            gegevens.Children.Add(persoon);

            TextBlock naam = new TextBlock();
            naam.Text = klant.Naam;
            naam.FontFamily = font;
            persoon.Children.Add(naam);

            if (klant.Straat != null && klant.HuisNr != null)
            {
                TextBlock adres = new TextBlock();
                adres.Text = klant.Adres;
                adres.FontFamily = font;
                persoon.Children.Add(adres);
                Afstand += 15;
            }

            if (klant.Postcode != null && klant.Gemeente != null)
            {
                TextBlock woonplaats = new TextBlock();
                woonplaats.Text = klant.Woonplaats;
                woonplaats.FontFamily = font;
                persoon.Children.Add(woonplaats);
                Afstand += 15;
            }

            if (klant.Land != null)
            {
                TextBlock land = new TextBlock();
                land.Text = klant.Land;
                land.FontFamily = font;
                persoon.Children.Add(land);
                Afstand += 15;
            }

            if (klant.BtwNr != null)
            {
                TextBlock btwNr = new TextBlock();
                btwNr.Text = klant.BtwNr;
                btwNr.FontFamily = font;
                persoon.Children.Add(btwNr);
                Afstand += 15;
            }

            return gegevens;
        }

        private Grid Artikels()
        {
            Grid artikels = new Grid();
            artikels.Margin = new Thickness(0, Afstand, 0, 0); ;
            artikels.Width = 590;

            ColumnDefinition ArtNr = new ColumnDefinition();
            ArtNr.Width = new GridLength(1.3, GridUnitType.Star);
            artikels.ColumnDefinitions.Add(ArtNr);
            ColumnDefinition Art = new ColumnDefinition();
            Art.Width = new GridLength(5, GridUnitType.Star);
            artikels.ColumnDefinitions.Add(Art);
            ColumnDefinition Excl = new ColumnDefinition();
            Excl.Width = new GridLength(1.8, GridUnitType.Star);
            artikels.ColumnDefinitions.Add(Excl);
            ColumnDefinition Incl = new ColumnDefinition();
            Incl.Width = new GridLength(1.8, GridUnitType.Star);
            artikels.ColumnDefinitions.Add(Incl);

            RowDefinition header = new RowDefinition();
            header.Height = new GridLength(25, GridUnitType.Pixel);
            artikels.RowDefinitions.Add(header);

            Label labelArtikelNr = new Label();
            labelArtikelNr.Content = "Artikelnr.";
            labelArtikelNr.FontFamily = font;
            Grid.SetRow(labelArtikelNr, 0);
            Grid.SetColumn(labelArtikelNr, 0);
            artikels.Children.Add(labelArtikelNr);
            Border border = new Border();
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 0);
            artikels.Children.Add(border);

            Label labelNaam = new Label();
            labelNaam.Content = "Artikel";
            labelNaam.FontFamily = font;
            Grid.SetRow(labelNaam, 0);
            Grid.SetColumn(labelNaam, 1);
            artikels.Children.Add(labelNaam);
            border = new Border();
            border.BorderThickness = new Thickness(0, 1, 1, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 1);
            artikels.Children.Add(border);

            Label labelExclBtw = new Label();
            labelExclBtw.Content = "Excl. BTW";
            labelExclBtw.FontFamily = font;
            Grid.SetRow(labelExclBtw, 0);
            Grid.SetColumn(labelExclBtw, 2);
            artikels.Children.Add(labelExclBtw);
            border = new Border();
            border.BorderThickness = new Thickness(0, 1, 1, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 2);
            artikels.Children.Add(border);

            Label labelInclBtw = new Label();
            labelInclBtw.Content = "Incl. BTW";
            labelInclBtw.FontFamily = font;
            Grid.SetRow(labelInclBtw, 0);
            Grid.SetColumn(labelInclBtw, 3);
            artikels.Children.Add(labelInclBtw);
            border = new Border();
            border.BorderThickness = new Thickness(0, 1, 1, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 3);
            artikels.Children.Add(border);

            int aantal = 0;
            foreach (var regel in tbFactuurRegels.Items)
            {
                aantal += 1;
                RowDefinition artRow = new RowDefinition();
                artRow.Height = new GridLength(25, GridUnitType.Pixel);
                artikels.RowDefinitions.Add(artRow);
                Artikel art = regel as Artikel;

                TextBlock artikelNr = new TextBlock();
                artikelNr.Text = art.ArtikelNr.ToString();
                artikelNr.FontFamily = font;
                artikelNr.Padding = new Thickness(5);
                Grid.SetRow(artikelNr, aantal);
                Grid.SetColumn(artikelNr, 0);
                artikels.Children.Add(artikelNr);

                TextBlock artikel = new TextBlock();
                artikel.Text = art.Naam;
                artikel.FontFamily = font;
                artikel.Padding = new Thickness(5);
                Grid.SetRow(artikel, aantal);
                Grid.SetColumn(artikel, 1);
                artikels.Children.Add(artikel);

                TextBlock exclBtw = new TextBlock();
                exclBtw.Text = art.Prijs.ToString() + " €";
                exclBtw.FontFamily = font;
                exclBtw.Padding = new Thickness(5);
                Grid.SetRow(exclBtw, aantal);
                Grid.SetColumn(exclBtw, 2);
                artikels.Children.Add(exclBtw);

                TextBlock inclBtw = new TextBlock();
                inclBtw.Text = art.prijsInclBtw.ToString() + " €";
                inclBtw.FontFamily = font;
                inclBtw.Padding = new Thickness(5);
                Grid.SetRow(inclBtw, aantal);
                Grid.SetColumn(inclBtw, 3);
                artikels.Children.Add(inclBtw);
            }
            for (var teller = 0; teller <= 3; teller++)
            {
                border = new Border();
                border.BorderThickness = new Thickness(1, 0, 1, 1);
                border.BorderBrush = Brushes.Black;
                Grid.SetRow(border, 1);
                Grid.SetColumn(border, teller);
                Grid.SetRowSpan(border, aantal);
                artikels.Children.Add(border);
            }

            artikels.RowDefinitions.Add(new RowDefinition());
            Label labelTotaalExclBtw = new Label();
            labelTotaalExclBtw.Content = "Excl. BTW:";
            labelTotaalExclBtw.FontFamily = font;
            labelTotaalExclBtw.Padding = new Thickness(5);
            Grid.SetRow(labelTotaalExclBtw, aantal + 1);
            Grid.SetColumn(labelTotaalExclBtw, 2);
            artikels.Children.Add(labelTotaalExclBtw);
            border = new Border();
            border.BorderThickness = new Thickness(1, 0, 0, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, aantal + 1);
            Grid.SetColumn(border, 2);
            Grid.SetRowSpan(border, 2);
            artikels.Children.Add(border);

            TextBlock TotaalExclBtw = new TextBlock();
            TotaalExclBtw.Text = totaalExclBtw.ToString() + " €";
            TotaalExclBtw.FontFamily = font;
            TotaalExclBtw.Padding = new Thickness(5);
            Grid.SetRow(TotaalExclBtw, aantal + 1);
            Grid.SetColumn(TotaalExclBtw, 3);
            artikels.Children.Add(TotaalExclBtw);
            border = new Border();
            border.BorderThickness = new Thickness(1,0,1,1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, aantal + 1);
            Grid.SetColumn(border, 3);
            Grid.SetRowSpan(border, 2);
            artikels.Children.Add(border);


            artikels.RowDefinitions.Add(new RowDefinition());
            Label labelTotaalInclBtw = new Label();
            labelTotaalInclBtw.Content = "Incl. BTW:";
            labelTotaalInclBtw.FontFamily = font;
            labelTotaalInclBtw.Padding = new Thickness(5);
            Grid.SetRow(labelTotaalInclBtw, aantal + 2);
            Grid.SetColumn(labelTotaalInclBtw, 2);
            artikels.Children.Add(labelTotaalInclBtw);


            TextBlock TotaalInclBtw = new TextBlock();
            TotaalInclBtw.Text = totaalInclBtw.ToString() + " €";
            TotaalInclBtw.FontFamily = font;
            TotaalInclBtw.Padding = new Thickness(5);
            Grid.SetRow(TotaalInclBtw, aantal + 2);
            Grid.SetColumn(TotaalInclBtw, 3);
            artikels.Children.Add(TotaalInclBtw);


            return artikels;
        }
    }
}
