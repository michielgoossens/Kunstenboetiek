using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace FacturatieKunstenboetiek
{
    class Print
    {
        private double A4breedte = 21 / 2.54 * 96;
        private double A4hoogte = 29.7 / 2.54 * 96;
        private double Afstand;
        private FontFamily font = new FontFamily("Verdana");

        public FixedDocument Preview(Factuur factuur)
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
            page.Children.Add(FactuurGegevens(factuur));
            Afstand += 60;
            page.Children.Add(KlantGegevens(factuur.Klant));
            Afstand += 65;
            page.Children.Add(Artikels(factuur.FactuurRegels.ToList()));
            Afstand = 850;
            TextBlock infoBetaling = new TextBlock();
            infoBetaling.Text = "Gelieve het bedrag inclusief BTW binnen 30 dagen te voldoen op het bovenstaande rekeningnummer, met mededeling: FACT" + factuur.FactuurNr + ".";
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
        private StackPanel FactuurGegevens(Factuur factuur)
        {
            StackPanel gegevens = new StackPanel();
            gegevens.Orientation = Orientation.Vertical;
            gegevens.Margin = new Thickness(0, Afstand, 0, 0);

            TextBlock factuurNr = new TextBlock();
            factuurNr.Text = "Factuurnummer: " + factuur.FactuurNr;
            factuurNr.FontFamily = font;
            gegevens.Children.Add(factuurNr);

            TextBlock datum = new TextBlock();
            datum.Text = "Datum: " + factuur.Datum;
            datum.FontFamily = font;
            gegevens.Children.Add(datum);

            return gegevens;
        }
        private StackPanel KlantGegevens(Klant klant)
        {
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
        private Grid Artikels(List<FactuurRegel> FactuurRegels)
        {
            Grid artikels = new Grid();
            artikels.Margin = new Thickness(0, Afstand, 0, 0); ;
            artikels.Width = 590;
            double totaalInclBtw = 0;
            double totaalExclBtw = 0;

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
            border.BorderThickness = new Thickness(0, 1, 0, 1);
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
            border.BorderThickness = new Thickness(1, 1, 1, 1);
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
            foreach (var regel in FactuurRegels)
            {
                aantal += 1;
                RowDefinition artRow = new RowDefinition();
                artRow.Height = new GridLength(25, GridUnitType.Pixel);
                artikels.RowDefinitions.Add(artRow);
                Artikel art = regel.Artikel as Artikel;

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
                totaalExclBtw += art.Prijs;

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
                
            }
            border = new Border();
            border.BorderThickness = new Thickness(1, 0, 1, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, 1);
            Grid.SetColumn(border, 0);
            Grid.SetRowSpan(border, aantal);
            artikels.Children.Add(border);
            border = new Border();
            border.BorderThickness = new Thickness(0, 0, 0, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, 1);
            Grid.SetColumn(border, 1);
            Grid.SetRowSpan(border, aantal);
            artikels.Children.Add(border);
            border = new Border();
            border.BorderThickness = new Thickness(1, 0, 1, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, 1);
            Grid.SetColumn(border, 2);
            Grid.SetRowSpan(border, aantal);
            artikels.Children.Add(border);
            border = new Border();
            border.BorderThickness = new Thickness(0, 0, 1, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, 1);
            Grid.SetColumn(border, 3);
            Grid.SetRowSpan(border, aantal);
            artikels.Children.Add(border);

            artikels.RowDefinitions.Add(new RowDefinition());
            Label labelTotaalExclBtw = new Label();
            labelTotaalExclBtw.Content = "Excl. BTW:";
            labelTotaalExclBtw.FontFamily = font;
            labelTotaalExclBtw.Padding = new Thickness(5);
            Grid.SetRow(labelTotaalExclBtw, aantal + 1);
            Grid.SetColumn(labelTotaalExclBtw, 2);
            artikels.Children.Add(labelTotaalExclBtw);
            border = new Border();
            border.BorderThickness = new Thickness(1, 0, 1, 1);
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
            border.BorderThickness = new Thickness(0, 0, 1, 1);
            border.BorderBrush = Brushes.Black;
            Grid.SetRow(border, aantal + 1);
            Grid.SetColumn(border, 3);
            Grid.SetRowSpan(border, 2);
            artikels.Children.Add(border);

            totaalInclBtw = totaalExclBtw * 1.06;
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
