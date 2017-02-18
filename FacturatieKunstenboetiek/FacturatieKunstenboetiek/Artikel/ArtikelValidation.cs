using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace FacturatieKunstenboetiek
{
    /// <summary>
    /// Validation class voor artikel
    /// </summary>
    public partial class Artikel : IDataErrorInfo
    {
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                //Naam is verplicht
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
                //kleur kan max 25 tekens zijn
                if (columnName == "Kleur")
                {
                    if (!string.IsNullOrEmpty(Kleur))
                    {
                        if (Kleur.Length > 50)
                            result = "Max. 50 tekens.";
                    }
                }
                //Prijs moet een positief getal zijn en is verplicht
                if (columnName == "Prijs")
                {
                    if (string.IsNullOrEmpty(Prijs.ToString()))
                        result = "Prijs is verplicht!";
                    else
                    {
                        if (Prijs < 0)
                        {
                            result = "Prijs mag niet kleiner dan nul zijn.";
                        }
                    }
                }
                //Info kan max 300 tekens zijn
                if (columnName == "Info")
                {
                    if (!string.IsNullOrEmpty(Info))
                    {
                        if (Info.Length > 500)
                            result = "Max. 500 tekens.";
                    }
                }
                return result;
            }
        }
    }
}
