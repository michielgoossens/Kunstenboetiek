using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool? Openen;
        public bool? Opgeslagen;
        public Klant teOpenenKlant;
        public Artikel teOpenenArtikel;
        public int padLeft = 3;
        double fontSize = 10;

        public DataTemplate klantLayout()
        {
            double sz1 = 70;
            double sz2 = 70;
            DataTemplate layout = new DataTemplate();
            layout.DataType = typeof(Klant);

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "klantStackPanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory Voornaam = new FrameworkElementFactory(typeof(Label));
            Voornaam.SetBinding(Label.ContentProperty, new Binding("Voornaam"));
            Voornaam.SetValue(Label.WidthProperty, sz1);
            Voornaam.SetValue(Label.FontSizeProperty, fontSize);
            stackPanel.AppendChild(Voornaam);

            FrameworkElementFactory Familienaam = new FrameworkElementFactory(typeof(Label));
            Familienaam.SetBinding(Label.ContentProperty, new Binding("Familienaam"));
            Familienaam.SetValue(Label.WidthProperty, sz2);
            Familienaam.SetValue(Label.FontSizeProperty, fontSize);
            stackPanel.AppendChild(Familienaam);

            FrameworkElementFactory KlantNr = new FrameworkElementFactory(typeof(Label));
            KlantNr.SetBinding(Label.ContentProperty, new Binding("KlantNr"));
            KlantNr.SetValue(Label.FontStyleProperty, FontStyles.Italic);
            KlantNr.SetValue(Label.ForegroundProperty, Brushes.DarkGray);
            stackPanel.AppendChild(KlantNr);

            layout.VisualTree = stackPanel;

            return layout;
        }

        public DataTemplate ArtikelLayout()
        {
            double sz = 140;
            DataTemplate layout = new DataTemplate();
            layout.DataType = typeof(Klant);

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "klantStackPanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory Naam = new FrameworkElementFactory(typeof(Label));
            Naam.SetBinding(Label.ContentProperty, new Binding("Naam"));
            Naam.SetValue(Label.WidthProperty, sz);
            Naam.SetValue(Label.FontSizeProperty, fontSize);
            stackPanel.AppendChild(Naam);

            FrameworkElementFactory ArtikelNr = new FrameworkElementFactory(typeof(Label));
            ArtikelNr.SetBinding(Label.ContentProperty, new Binding("ArtikelNr"));
            ArtikelNr.SetValue(Label.FontStyleProperty, FontStyles.Italic);
            ArtikelNr.SetValue(Label.ForegroundProperty, Brushes.DarkGray);
            stackPanel.AppendChild(ArtikelNr);

            layout.VisualTree = stackPanel;

            return layout;
        }
    }
}
