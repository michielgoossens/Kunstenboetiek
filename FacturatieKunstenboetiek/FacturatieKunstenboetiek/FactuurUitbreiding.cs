﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Globalization;

namespace FacturatieKunstenboetiek
{
    public partial class Factuur : IDataErrorInfo
    {
        public string zoekFactuur { get { return Naam + " " + FactuurNr.ToString().PadLeft(Overal.padLeft, '0'); } }

        public string Naam { get
            {
                if (Klant.Familienaam != null)
                {
                    return Klant.Voornaam + " " + Klant.Familienaam;
                }
                else
                {
                    return Klant.Voornaam;
                }
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Klant")
                {
                    if (Klant == null)
                    {
                        result = "Klant is verplicht!";
                    }
                }
                if (columnName == "Datum")
                {
                    if (string.IsNullOrEmpty(Datum))
                        result = "Datum is verplicht! (dd-mm-yyyy)";
                    else
                    {
                        DateTime dDatum;
                        if (!DateTime.TryParseExact(Datum, "dd-mm-yyyy", null, DateTimeStyles.None, out dDatum))
                        {
                            result = "Datum moet dd-mm-yyyy zijn.";
                        }
                    }
                }
                return result;
            }
        }
    }
}