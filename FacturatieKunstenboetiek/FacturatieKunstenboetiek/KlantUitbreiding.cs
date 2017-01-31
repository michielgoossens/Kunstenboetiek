using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturatieKunstenboetiek
{
    public partial class Klant
    {
        public string zoekKlant { get { return Naam + " " + KlantNr.ToString().PadLeft(Overal.padLeft, '0');} }
        public string Naam
        {
            get
            {
                if (Voornaam != "")
                {
                    string naam = Voornaam;
                    if (Familienaam != "")
                    {
                        naam += " " + Familienaam;
                    }
                    return naam;
                }
                else
                {
                    return Familienaam;
                }
            }
        }

        public string Adres { get { return Straat + " " + HuisNr; } }
        public string Woonplaats { get { return Postcode + " " + Gemeente; } }
    }
}
