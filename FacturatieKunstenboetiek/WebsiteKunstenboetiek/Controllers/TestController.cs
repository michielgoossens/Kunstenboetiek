using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using WebsiteKunstenboetiek.Models;

namespace WebsiteKunstenboetiek.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Contact()
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
                SmtpClient smtpServer = new SmtpClient("smtp.mijnhostingpartner.nl");

                mail.From = new MailAddress("contact@kunstenboetiek.be");
                mail.To.Add("kunstenboetiek@hotmail.com");
                mail.Subject = emailForm.Onderwerp;

                string body = emailForm.Voornaam + " " + emailForm.Familienaam + "\n" + emailForm.Email + "\n" + emailForm.TelNr + "\n\n" + "Bericht:\n" + emailForm.Bericht;
                mail.Body = body;

                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("contact@kunstenboetiek.be", "KunstenBoetiek...123");
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