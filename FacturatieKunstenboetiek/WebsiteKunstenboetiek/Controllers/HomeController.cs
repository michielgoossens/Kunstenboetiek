﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKunstenboetiek.Models;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace WebsiteKunstenboetiek.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Info()
        {
            return View();
        }
        [HttpGet]
        public  ActionResult Contact()
        {
            EmailForm emailForm = new EmailForm();
            return View(emailForm);
        }
        [HttpPost]
        public ActionResult Contact(EmailForm emailForm)
        {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("michielgoossens1409@gmail.com");
                mail.To.Add("michielgoossens1409@gmail.com");
                mail.Subject = emailForm.Onderwerp;

                string body = emailForm.Voornaam + " " + emailForm.Familienaam + "\n" + emailForm.Email + "\n" + emailForm.TelNr + "\n\n" + "Bericht:\n" + emailForm.Bericht;
                mail.Body = body;

                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("michielgoossens1409@gmail.com", "wim123hlG000");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
                return View();
        }
    }
}