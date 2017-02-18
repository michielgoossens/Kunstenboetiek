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
    public class Overal
    {
        public static bool? Openen;
        public static Klant teOpenenKlant;
        public static Artikel teOpenenArtikel;
        public static Factuur teOpenenFactuur;
        public static int padLeft = 3;
        public static OverzichtWindow overzichtWindow = new OverzichtWindow();
        public static DataTemplate ZoekLayout(string Nummer)
        {
            DataTemplate layout = new DataTemplate();

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory Naam = new FrameworkElementFactory(typeof(TextBlock));
            Naam.SetBinding(TextBlock.TextProperty, new Binding("Naam"));
            TextTrimming trimming = TextTrimming.CharacterEllipsis;
            Naam.SetValue(TextBlock.TextTrimmingProperty, trimming);
            double sz = 200;
            Naam.SetValue(TextBlock.WidthProperty, sz);
            double fontSize = 12;
            Naam.SetValue(TextBlock.FontSizeProperty, fontSize);
            stackPanel.AppendChild(Naam);

            FrameworkElementFactory Nr = new FrameworkElementFactory(typeof(Label));
            if (Nummer == "klant")
            {
            Nr.SetBinding(Label.ContentProperty, new Binding("KlantNr"));
            }
            else if (Nummer == "artikel")
            {
                Nr.SetBinding(Label.ContentProperty, new Binding("ArtikelNr"));
            }
            else if (Nummer == "factuur")
            {
                Nr.SetBinding(Label.ContentProperty, new Binding("FactuurNr"));
            }
            Nr.SetValue(Label.FontStyleProperty, FontStyles.Italic);
            Nr.SetValue(Label.ForegroundProperty, Brushes.DarkGray);
            stackPanel.AppendChild(Nr);

            layout.VisualTree = stackPanel;

            return layout;
        }
    }
}
