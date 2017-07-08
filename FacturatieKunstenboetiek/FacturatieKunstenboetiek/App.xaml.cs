using System;
using System.IO;
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
            removeOldPictures();

            MessageBoxManager.Yes = "Ja";
            MessageBoxManager.No = "Nee";
            MessageBoxManager.OK = "Oké";
            MessageBoxManager.Cancel = "Annuleren";
            MessageBoxManager.Register();

            Overal.overzichtWindow.setUp();

            MainWindow main = new MainWindow();
            main.Show();
        }

        private void removeOldPictures()
        {
            string local = "C:/Kunstenboetiek/Old/";
            if (!Directory.Exists(local))
            {
                Directory.CreateDirectory(local);
            }
            Ftp ftpClient = new Ftp(@"ftp://kunstenboetiek.be/", "ftpafbeeldingen", "KunstenBoetiek..123");
            using (var dbEntities = new KunstenboetiekDbEntities())
            {
                foreach (Artikel a in dbEntities.Artikels.Include("ArtikelAfbeeldingen"))
                {
                    if (a.Verkocht == true && a.Datum > DateTime.Now.AddMonths(-6))
                    {
                        foreach (var afbeelding in a.ArtikelAfbeeldingen)
                        {
                            string remote = afbeelding.AfbeeldingLink;
                            remote = remote.Substring(remote.LastIndexOf('/'));
                            
                            //verplaats afbeeldingen naar locaal
                        }
                    }
                }
            }
        }
    }
}