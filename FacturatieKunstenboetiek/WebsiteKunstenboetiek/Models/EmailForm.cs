using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebsiteKunstenboetiek.Models
{
    public class EmailForm
    {
        [Display(Name = "Voornaam:")]
        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string Voornaam { get; set; }
        [Display(Name = "Familienaam:")]
        public string Familienaam { get; set; }
        [Display(Name = "E-mail adres:")]
        [Required(ErrorMessage = "E-mail adres is verplicht")]
        public string Email { get; set; }
        [Display(Name = "Telefoon/GSM:")]
        public string TelNr { get; set; }
        [Display(Name = "Onderwerp:")]
        [Required(ErrorMessage = "Onderwerp is verplicht")]
        public string Onderwerp { get; set; }
        [Display(Name = "Bericht:")]
        [Required(ErrorMessage = "Bericht is verplicht")]
        public string Bericht { get; set; }
    }
}