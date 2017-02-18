using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturatieKunstenboetiek
{
    public partial class Artikel
    {
        public double prijsInclBtw {
            get { return (Prijs * 1.06); }
            set { }
        }
        public string zoekArtikel { get { return Naam + " " + ArtikelNr.ToString().PadLeft(Overal.padLeft, '0'); } } 
    }
}

