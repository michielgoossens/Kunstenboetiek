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
            setUp();
        }

        public void setUp()
        {
            setUpFacturen();
            setUpArtikels();
            setUpKlanten();
        }
        public void setUpFacturen()
        {
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (var f in dbEntities.Facturen)
                {
                    dbEntities.Klanten.Attach(f.Klant);
                    tbFacturen.Items.Add(f);
                }
            }
        }
        public void setUpArtikels()
        {
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
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (var k in dbEntities.Klanten)
                {
                    tbKlanten.Items.Add(k);
                }
            }
        }
    }
}
