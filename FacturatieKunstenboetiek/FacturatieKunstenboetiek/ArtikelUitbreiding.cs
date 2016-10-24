using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FacturatieKunstenboetiek
{
    public partial class Artikel : IDataErrorInfo
    {
        public string zoekArtikel { get { return Naam + ArtikelNr.ToString(); } }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Naam")
                {
                    if (string.IsNullOrEmpty(Naam))
                        result = "Naam is verplicht!";
                    else
                    {
                        if (Naam.Length > 100)
                            result = "Max. 100 tekens.";
                    }
                }
                if (columnName == "Kleur")
                {
                    if (!string.IsNullOrEmpty(Kleur))
                    {
                        if (Kleur.Length > 25)
                            result = "Max. 25 tekens.";
                    }
                }
                if (columnName == "Soort")
                {
                    if (!string.IsNullOrEmpty(Soort))
                    {
                        if (Soort.Length > 15)
                            result = "Max. 15 tekens.";
                    }
                }
                if (columnName == "Prijs")
                {
                    if (!string.IsNullOrEmpty(Prijs.ToString()))
                    {
                        if (Prijs < 0)
                        {
                            result = "Prijs moet een positief getal zijn";
                        }
                    }
                }
                return result;
            }
        }
    }
}
