using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net.Mail;
using System.Windows;

namespace FacturatieKunstenboetiek
{
    public partial class Klant : IDataErrorInfo
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
                if (columnName == "Voornaam" || columnName == "Familienaam")
                {
                    if (string.IsNullOrEmpty(Voornaam) && string.IsNullOrEmpty(Familienaam))
                        result = "Voornaam of familienaam is verplicht!";
                    else
                    {
                        if (Voornaam.Length > 50)
                            result = "Max. 50 tekens.";
                    }
                }
                if (columnName == "Familienaam")
                {
                    if (!string.IsNullOrEmpty(Familienaam))
                    {
                        if (Familienaam.Length > 50)
                            result = "Max. 50 tekens.";
                    }
                }
                if (columnName == "Straat")
                {
                    if (!string.IsNullOrEmpty(Straat))
                    {
                        if (Straat.Length > 50)
                            result = "Max. 50 tekens.";
                    }
                }
                if (columnName == "HuisNr")
                {
                    if (!string.IsNullOrEmpty(HuisNr))
                    {
                        if (HuisNr.Length > 10)
                            result = "Max. 10 tekens.";
                    }
                }
                if (columnName == "Postcode")
                {
                    if (!string.IsNullOrEmpty(Postcode))
                    {
                        if (Postcode.Length > 15)
                            result = "Max. 15 tekens";
                    }
                }
                if (columnName == "Gemeente")
                {
                    if (!string.IsNullOrEmpty(Gemeente))
                    {
                        if (Gemeente.Length > 50)
                            result = "Max. 50 tekens.";
                    }
                }
                if (columnName == "Telefoon")
                {
                    if (!string.IsNullOrEmpty(Telefoon))
                    {
                        if (Telefoon.Length > 30)
                            result = "Max. 30 tekens.";
                    }
                }
                if (columnName == "Email")
                {
                    if (!string.IsNullOrEmpty(Email))
                    {
                        if (Email.Length > 254)
                            result = "Max. 254 tekens.";
                        else
                        {
                            try
                            {
                                MailAddress m = new MailAddress(Email);
                            }

                            catch (FormatException)
                            {
                                result = "Email adress is niet geldig.";
                            }
                        }
                    }
                }
                if (columnName == "BtwNr")
                {
                    if (!string.IsNullOrEmpty(BtwNr))
                    {
                        if (BtwNr.Length > 17)
                            result = "Max. 17 tekens.";
                    }
                }
                return result;
            }
        }
    }
}
