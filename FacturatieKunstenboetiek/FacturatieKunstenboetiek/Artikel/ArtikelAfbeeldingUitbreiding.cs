using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturatieKunstenboetiek
{
    public partial class ArtikelAfbeelding
    {
        public string File
        {
            get
            {
                int lastSlash;
                if (AfbeeldingLink.Substring(0, 7) != "http://")
                {
                    lastSlash = AfbeeldingLink.LastIndexOf('\\');
                    return AfbeeldingLink.Substring(lastSlash + 1);
                }
                else
                {
                    lastSlash = AfbeeldingLink.LastIndexOf('/');
                    return AfbeeldingLink.Substring(lastSlash + 1);
                }
            }
            set { }
        }
    }
}
