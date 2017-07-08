using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKunstenboetiek.Models;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.UI;

namespace WebsiteKunstenboetiek.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Artikel> newArticles;
            using (var db = new KunstenboetiekDbEntities())
            {
                newArticles = (db.Artikels.Include("ArtikelAfbeeldingen").Where(a => a.ArtikelAfbeeldingen.Count > 0 && a.Verkocht == false).OrderByDescending(a => a.Datum)).ToList();
                newArticles = newArticles.Take(5);
            }
                return View(newArticles);
        }
        public ActionResult Nieuws()
        {
            return View();
        }
        [HttpGet]
        public  ActionResult Contact()
        {
            EmailForm emailForm = new EmailForm();
            if (TempData["shortMessage"] != null)
            ViewBag.Message = TempData["shortMessage"].ToString();
            return View(emailForm);
        }
        [HttpPost]
        public ActionResult Contact(EmailForm emailForm)
        {
            if (this.ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("contactkunstenboetiek@gmail.com", "Contact - Kunstenboetiek");
                mail.To.Add("kunstenboetiek@hotmail.com");
                mail.Subject = emailForm.Onderwerp;

                string body = emailForm.Voornaam + " " + emailForm.Familienaam + "\n" + emailForm.Email + "\n" + emailForm.TelNr + "\n\n" + "Bericht:\n" + emailForm.Bericht;
                mail.Body = body;

                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("contactkunstenboetiek@gmail.com", "KunstenBoetiek..123");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);

                TempData["shortMessage"] = "Het formulier is goed verzonden. U krijgt zo spoedig mogelijk antwoord.";

                return RedirectToAction("Contact");
            }
            else
            {
                return View();
            }
        }
    }
}