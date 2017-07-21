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
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MessageBoxManager.Yes = "Ja";
            MessageBoxManager.No = "Nee";
            MessageBoxManager.OK = "Oké";
            MessageBoxManager.Cancel = "Annuleren";
            MessageBoxManager.Register();

            Overal.overzichtWindow.setUp();

            removeOldPictures();

            MainWindow main = new MainWindow();
            main.Show();
        }

        private void removeOldPictures()
        {
            /*
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (Artikel a in dbEntities.Artikels.Include("artikelAfbeeldingen"))
                {
                    if (a.Datum < DateTime.Now.AddYears(-2))
                    {
                        foreach (ArtikelAfbeelding aA in a.ArtikelAfbeeldingen)
                        {
                            //copy pictures to other location and remove from db
                        }
                        
                    }
                }
            }
            */
        }
    }
}
