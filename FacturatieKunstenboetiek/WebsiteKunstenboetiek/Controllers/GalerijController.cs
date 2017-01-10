using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKunstenboetiek.Models;

namespace WebsiteKunstenboetiek.Controllers
{
    public class GalerijController : Controller
    {
        // GET: Galerij
        public ActionResult Urnen()
        {
            var urnen = new List<Artikel>();
            using (var db = new KunstenboetiekDbEntities())
            {
                urnen = (from a in db.Artikels
                         where a.Soort == "Urne"
                         select a).ToList();
            }
            return View(urnen);
        }
        public ActionResult MiniUrnen()
        {
            var miniUrnen = new List<Artikel>();
            using (var db = new KunstenboetiekDbEntities())
            {
                miniUrnen = (from a in db.Artikels
                         where a.Soort == "Mini urne"
                         select a).ToList();
            }
            return View(miniUrnen);
        }
        public ActionResult DierenUrnen()
        {
            var dierenUrnen = new List<Artikel>();
            using (var db = new KunstenboetiekDbEntities())
            {
                dierenUrnen = (from a in db.Artikels
                         where a.Soort == "Dieren urne"
                         select a).ToList();
            }
            return View(dierenUrnen);
        }
        public ActionResult AndereWerken()
        {
            var andereWerken = new List<Artikel>();
            using (var db = new KunstenboetiekDbEntities())
            {
                andereWerken = (from a in db.Artikels
                               where a.Soort == "Andere werken"
                               select a).ToList();
            }
            return View(andereWerken);
        }
    }
}