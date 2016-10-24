using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net.Mail;

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
                if (columnName == "Voornaam")
                {
                    if (string.IsNullOrEmpty(Voornaam))
                        result = "Voornaam is verplicht!";
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
                        if (Telefoon.Length > 50)
                            result = "Max. 50 tekens.";
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
                Int64 test;
                if (columnName == "BtwNr")
                {
                    if (!string.IsNullOrEmpty(BtwNr))
                    {
                        if (BtwNr.Length > 20)
                            result = "Max. 20 tekens.";
                        else
                        {
                            if (!string.IsNullOrEmpty(Land))
                            {
                                if (Land == "België")
                                {
                                    if (BtwNr.Length != 12)
                                    {
                                        result = "Btw-nummer moet uit 12 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string CheckNul = BtwNr.Substring(2, 1);
                                        string checkCijfers = BtwNr.Substring(3);
                                        if (LandCode.ToLower() != "be" || CheckNul != "0" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (BE0*********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Oostenrijk")
                                {
                                    if (BtwNr.Length != 11)
                                    {
                                        result = "Btw-nummer moet uit 11 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string CheckU = BtwNr.Substring(2, 1);
                                        string checkCijfers = BtwNr.Substring(3);
                                        if (LandCode.ToLower() != "at" || CheckU.ToLower() != "u" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (ATU********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Bulgarije")
                                {
                                    if (BtwNr.Length != 11 && BtwNr.Length != 12)
                                    {
                                        result = "Btw-nummer moet uit 11 of 12 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "bg" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (BG********* of BG**********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Cyprus")
                                {
                                    if (BtwNr.Length != 11)
                                    {
                                        result = "Btw-nummer moet uit 11 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2, 8);
                                        string CheckLetter = BtwNr.Substring(10);
                                        if (LandCode.ToLower() != "cy" || !Int64.TryParse(checkCijfers, out test) || Int64.TryParse(CheckLetter, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (CY********l) | * = cijfer, l = Letter";
                                        }
                                    }
                                }
                                if (Land == "Tsjechië")
                                {
                                    if (BtwNr.Length != 10 && BtwNr.Length != 11 && BtwNr.Length != 12)
                                    {
                                        result = "Btw-nummer moet uit 10, 11 of 12 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "cz" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (CZ********, CZ********* of CZ**********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Duitsland")
                                {
                                    if (BtwNr.Length != 11)
                                    {
                                        result = "Btw-nummer moet uit 11 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "de" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (DE*********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Denemarken")
                                {
                                    if (BtwNr.Length != 13)
                                    {
                                        result = "Btw-nummer moet uit 13 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijferGroep1 = BtwNr.Substring(2, 2);
                                        string checkSpatie1 = BtwNr.Substring(4, 1);
                                        string checkCijferGroep2 = BtwNr.Substring(5, 2);
                                        string checkSpatie2 = BtwNr.Substring(7, 1);
                                        string checkCijferGroep3 = BtwNr.Substring(8, 2);
                                        string checkSpatie3 = BtwNr.Substring(10, 1);
                                        string checkCijferGroep4 = BtwNr.Substring(11, 2);
                                        if (LandCode.ToLower() != "dk" || !Int64.TryParse(checkCijferGroep1, out test) || checkSpatie1 != " " || !Int64.TryParse(checkCijferGroep2, out test) || checkSpatie2 != " " || !Int64.TryParse(checkCijferGroep3, out test) || checkSpatie3 != " " || !Int64.TryParse(checkCijferGroep4, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (DK** ** ** **) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Estland")
                                {
                                    if (BtwNr.Length != 11)
                                    {
                                        result = "Btw-nummer moet uit 11 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "ee" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (EE*********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Griekenland")
                                {
                                    if (BtwNr.Length != 11)
                                    {
                                        result = "Btw-nummer moet uit 11 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "el" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (EL*********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Spanje")
                                {
                                    if (BtwNr.Length != 11)
                                    {
                                        result = "Btw-nummer moet uit 11 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(3, 7);
                                        string checkAlfanumeriek1 = BtwNr.Substring(2, 1);
                                        string checkAlfanumeriek2 = BtwNr.Substring(10, 1);

                                        if (LandCode.ToLower() != "es" || !Int64.TryParse(checkCijfers, out test) || ((checkAlfanumeriek1.Any(x => !char.IsLetter(x)) && checkAlfanumeriek2.Any(x => !char.IsLetter(x))) || (!Int64.TryParse(checkAlfanumeriek1, out test) && checkAlfanumeriek2.Any(x => !char.IsLetter(x))) && (checkAlfanumeriek1.Any(x => !char.IsLetter(x)) && !Int64.TryParse(checkAlfanumeriek2, out test))))
                                        {
                                            result = "Btw-nummer is niet correct. (ESx*********x) | * = cijfer, x = alfanumeriek, niet beiden numeriek";
                                        }
                                    }
                                }
                                if (Land == "Finland")
                                {
                                    if (BtwNr.Length != 10)
                                    {
                                        result = "Btw-nummer moet uit 10 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "fi" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (FI********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Frankrijk")
                                {
                                    if (BtwNr.Length != 14)
                                    {
                                        result = "Btw-nummer moet uit 14 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkAlfanumeriek = BtwNr.Substring(2, 2);
                                        string checkSpatie = BtwNr.Substring(4, 1);
                                        string checkCijfers = BtwNr.Substring(5);
                                        if (LandCode.ToLower() != "fr" || (Int64.TryParse(checkAlfanumeriek.Substring(0, 1), out test) && Int64.TryParse(checkAlfanumeriek.Substring(1, 1), out test)) || checkSpatie != " " || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (FRxx *********) | * = cijfer, x = alfanumeriek, niet beiden numeriek";
                                        }
                                    }
                                }
                                if (Land == "Verenigd Koninkrijk")
                                {
                                    if (BtwNr.Length != 7 && BtwNr.Length != 13 && BtwNr.Length != 17)
                                    {
                                        result = "Btw-nummer moet uit 7, 13 of 17 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        if (LandCode.ToLower() == "gb")
                                        {
                                            if (BtwNr.Length == 7)
                                            {
                                                string InstellingCode = BtwNr.Substring(2, 2);
                                                string checkCijfers = BtwNr.Substring(4);

                                                if ((InstellingCode.ToLower() != "gd"  && InstellingCode.ToLower() != "ha") || !Int64.TryParse(checkCijfers, out test))
                                                {
                                                    result = "Btw-nummer is niet correct. (GBGD***, GBHA***, GB*** **** ** of GB*** **** ** ***) | * = cijfer";
                                                }
                                            }
                                            if (BtwNr.Length == 13)
                                            {
                                                string checkCijferGroep1 = BtwNr.Substring(2, 3);
                                                string checkSpatie1 = BtwNr.Substring(5, 1);
                                                string checkCijferGroep2 = BtwNr.Substring(6, 4);
                                                string checkSpatie2 = BtwNr.Substring(10, 1);
                                                string checkCijferGroep3 = BtwNr.Substring(11, 2);
                                                if (!Int64.TryParse(checkCijferGroep1, out test) || checkSpatie1 != " " || !Int64.TryParse(checkCijferGroep2, out test) || checkSpatie2 != " " || !Int64.TryParse(checkCijferGroep3, out test))
                                                {
                                                    result = "Btw-nummer is niet correct. (GBGD***, GBHA***, GB*** **** ** of GB*** **** ** ***) | * = cijfer";
                                                }
                                            }
                                            if (BtwNr.Length == 17)
                                            {
                                                string checkCijferGroep1 = BtwNr.Substring(2, 3);
                                                string checkSpatie1 = BtwNr.Substring(5, 1);
                                                string checkCijferGroep2 = BtwNr.Substring(6, 4);
                                                string checkSpatie2 = BtwNr.Substring(10, 1);
                                                string checkCijferGroep3 = BtwNr.Substring(11, 2);
                                                string checkSpatie3 = BtwNr.Substring(13, 1);
                                                string checkCijferGroep4 = BtwNr.Substring(14, 3);
                                                if (!Int64.TryParse(checkCijferGroep1, out test) || checkSpatie1 != " " || !Int64.TryParse(checkCijferGroep2, out test) || checkSpatie2 != " " || !Int64.TryParse(checkCijferGroep3, out test) || checkSpatie3 != " " || !Int64.TryParse(checkCijferGroep4, out test))
                                                {
                                                    result = "Btw-nummer is niet correct. (GBGD***, GBHA***, GB*** **** ** of GB*** **** ** ***) | * = cijfer";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result = "Btw-nummer is niet correct. (GBGD***, GBHA***, GB*** **** ** of GB*** **** ** ***) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Kroatië")
                                {
                                    if (BtwNr.Length != 13)
                                    {
                                        result = "Btw-nummer moet uit 13 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "hr" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (HR***********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Hongarije")
                                {
                                    if (BtwNr.Length != 10)
                                    {
                                        result = "Btw-nummer moet uit 10 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "hu" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (HU********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Ierland")
                                {
                                    if (BtwNr.Length != 10)
                                    {
                                        result = "Btw-nummer moet uit 10 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfer = BtwNr.Substring(2, 1);
                                        string checkSpecial = BtwNr.Substring(3, 1);
                                        string checkCijfers = BtwNr.Substring(4,5);
                                        string checkLetter = BtwNr.Substring(9);
                                        if (LandCode.ToLower() != "ie" || !Int64.TryParse(checkCijfer, out test) || (!Int64.TryParse(checkSpecial, out test) && checkSpecial.Any(x => !char.IsLetter(x)) && checkSpecial != "+" && checkSpecial!= "*")  || !Int64.TryParse(checkCijfers, out test) || Int64.TryParse(checkLetter, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (IE*s*****l) | * = cijfer, s = cijfer, letter, + of *, l = letter";
                                        }
                                    }
                                }
                                if (Land == "Italië")
                                {
                                    if (BtwNr.Length != 13)
                                    {
                                        result = "Btw-nummer moet uit 13 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "it" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (IT***********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Litouwen")
                                {
                                    if (BtwNr.Length != 11 && BtwNr.Length != 14)
                                    {
                                        result = "Btw-nummer moet uit 11 of 14 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "lt" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (LT********* of LT************) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Luxemburg")
                                {
                                    if (BtwNr.Length != 10)
                                    {
                                        result = "Btw-nummer moet uit 10 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "lu" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (LU********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Letland")
                                {
                                    if (BtwNr.Length != 13)
                                    {
                                        result = "Btw-nummer moet uit 13 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "lv" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (LV***********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Malta")
                                {
                                    if (BtwNr.Length != 10)
                                    {
                                        result = "Btw-nummer moet uit 10 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "mt" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (MT********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Nederland")
                                {
                                    if (BtwNr.Length != 13)
                                    {
                                        result = "Btw-nummer moet uit 13 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfergroep1 = BtwNr.Substring(2, 9);
                                        string checkLetterB = BtwNr.Substring(11, 1);
                                        string checkCijfergroep2 = BtwNr.Substring(12, 2);
                                        if (LandCode.ToLower() != "nl" || !Int64.TryParse(checkCijfergroep1, out test) || checkLetterB.ToLower() != "b")
                                        {
                                            result = "Btw-nummer is niet correct. (NL*********B**) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Polen")
                                {
                                    if (BtwNr.Length != 12)
                                    {
                                        result = "Btw-nummer moet uit 12 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "pl" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (PL**********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Portugal")
                                {
                                    if (BtwNr.Length != 11)
                                    {
                                        result = "Btw-nummer moet uit 11 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "pt" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (PT*********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Roemenië")
                                {
                                    if (BtwNr.Length < 4 || BtwNr.Length > 12)
                                    {
                                        result = "Btw-nummer moet tussen 4 en 12 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "ro" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (min. RO** of max. RO**********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Zweden")
                                {
                                    if (BtwNr.Length != 14)
                                    {
                                        result = "Btw-nummer moet uit 14 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "se" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (SE************) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Slovenië")
                                {
                                    if (BtwNr.Length != 10)
                                    {
                                        result = "Btw-nummer moet uit 10 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "sl" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (SL********) | * = cijfer";
                                        }
                                    }
                                }
                                if (Land == "Slowakije")
                                {
                                    if (BtwNr.Length != 12)
                                    {
                                        result = "Btw-nummer moet uit 12 tekens bestaan.";
                                    }
                                    else
                                    {
                                        string LandCode = BtwNr.Substring(0, 2);
                                        string checkCijfers = BtwNr.Substring(2);
                                        if (LandCode.ToLower() != "sk" || !Int64.TryParse(checkCijfers, out test))
                                        {
                                            result = "Btw-nummer is niet correct. (SK**********) | * = cijfer";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                result = "Gelieve eerst een land te selecteren.";
                            }
                        }
                    }
                }
                return result;
            }
        }
    }
}
