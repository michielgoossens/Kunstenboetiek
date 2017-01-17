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
    /// Interaction logic for AfbeeldingWindow.xaml
    /// </summary>
    public partial class AfbeeldingWindow : Window
    {
        public AfbeeldingWindow(string ImageLink)
        {
            InitializeComponent();
            afbeelding.Source = new BitmapImage(new Uri(ImageLink));
        }
    }
}
