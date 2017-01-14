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
                urnen = (from a in db.Artikels.Include("ArtikelAfbeeldingen")
                         where a.Soort == "Urne" && a.ArtikelAfbeeldingen.Count > 0 && a.Verkocht == false
                         select a).ToList();
            }
            return View(urnen);
        }
        public ActionResult MiniUrnen()
        {
            var miniUrnen = new List<Artikel>();
            using (var db = new KunstenboetiekDbEntities())
            {
                miniUrnen = (from a in db.Artikels.Include("ArtikelAfbeeldingen")
                             where a.Soort == "Mini urne" && a.ArtikelAfbeeldingen.Count > 0 && a.Verkocht == false
                             select a).ToList();
            }
            return View(miniUrnen);
        }
        public ActionResult DierenUrnen()
        {
            var dierenUrnen = new List<Artikel>();
            using (var db = new KunstenboetiekDbEntities())
            {
                dierenUrnen = (from a in db.Artikels.Include("ArtikelAfbeeldingen")
                               where a.Soort == "Dieren urne" && a.ArtikelAfbeeldingen.Count > 0 && a.Verkocht == false
                               select a).ToList();
            }
            return View(dierenUrnen);
        }
        public ActionResult AndereWerken()
        {
            var andereWerken = new List<Artikel>();
            using (var db = new KunstenboetiekDbEntities())
            {
                andereWerken = (from a in db.Artikels.Include("ArtikelAfbeeldingen")
                                where a.Soort == "Andere werken" && a.ArtikelAfbeeldingen.Count > 0 && a.Verkocht == false
                                select a).ToList();
            }
            return View(andereWerken);
        }
    }
}