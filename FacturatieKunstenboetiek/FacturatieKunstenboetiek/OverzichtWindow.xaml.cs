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
    /// Interaction logic for OverzichtWindow.xaml
    /// </summary>
    public partial class OverzichtWindow : Window
    {
        public OverzichtWindow()
        {
            InitializeComponent();
        }

        public void setUp()
        {
            setUpFacturen();
            setUpArtikels();
            setUpKlanten();
        }
        public void setUpFacturen()
        {
            tbFacturen.Items.Clear();
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (var f in dbEntities.Facturen.Include("klant"))
                {
                    tbFacturen.Items.Add(f);
                }
            }
            
        }
        public void setUpArtikels()
        {
            tbArtikels.Items.Clear();
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (var a in dbEntities.Artikels)
                {
                    tbArtikels.Items.Add(a);
                }
            }
        }
        public void setUpKlanten()
        {
            tbKlanten.Items.Clear();
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (var k in dbEntities.Klanten)
                {
                    tbKlanten.Items.Add(k);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 2; i == 0; i--)
            {
                tabControlOverzicht.SelectedIndex = i;
            }
        }
    }
}
