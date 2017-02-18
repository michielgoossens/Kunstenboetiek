using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturatieKunstenboetiek
{
    public partial class Factuur
    {
        public string zoekFactuur { get { return Naam + " " + FactuurNr.ToString().PadLeft(Overal.padLeft, '0'); } }

        public string Naam
        {
            get
            {
                if (Klant.Voornaam != "")
                {
                    string naam = Klant.Voornaam;
                    if (Klant.Familienaam != "")
                    {
                        naam+= " " + Klant.Familienaam;
                    }
                    return naam;
                }
                else
                {
                    return Klant.Familienaam;
                }
            }
        }

        public double PrijsExclBtw
        {
            get
            {
                double prijs = 0;
                using (var dbEntities = new KunstenboetiekDbEntities())
                {
                    List<FactuurRegel> regels = (from r in dbEntities.FactuurRegels
                                                 where r.FactuurNr == FactuurNr
                                                 select r).ToList();

                    foreach (var regel in regels)
                    {
                        prijs += regel.Artikel.Prijs;
                    }
                }
                return prijs;
            }
        }
        public double PrijsInclBtw { get { return PrijsExclBtw * 1.06; } }

       
    }
}
