using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IndexViewModel ivm = new IndexViewModel();
            FamiliesDb db = new FamiliesDb(Properties.Settings.Default.ConStr);
            ivm.FamiliesWithNumberOfKids = db.GetFamiliesWithNumberOfKids();
            Family family = (Family)TempData["Family"];
            if (family != null)
            {
                ivm.Activity = db.AddedDeleted("Families", family);
                ivm.Family = family;
            }
            
            return View(ivm);
        }
        public ActionResult NewFamily()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFamily(Family family)
        {
           FamiliesDb db = new FamiliesDb(Properties.Settings.Default.ConStr);
           family.Id = db.AddFamily(new Family { LastName = family.LastName });
           TempData["Family"] = family;
           return Redirect("/");
        }
        [HttpPost]
        public ActionResult DeleteFamily(Family family)
        {
            FamiliesDb db = new FamiliesDb(Properties.Settings.Default.ConStr);
            db.DeleteFamily(family);
            TempData["Family"] = family;
            return Redirect("/");
        }
        public ActionResult SeeKids(int Id)
        {
            SeeKidsViewModel skv = new SeeKidsViewModel();
            FamiliesDb db = new FamiliesDb(Properties.Settings.Default.ConStr);
            if (TempData["Kid"] != null)
            {
                skv.KidAdded = (Kid)TempData["Kid"];
            }
            skv.Family = new Family();
            skv.Family.Id = Id;
            skv.Kids = db.GetKids(Id);
            return View(skv);
        }
        public ActionResult NewKid(int Id, string LastName)
        {
            NewKidView nkv = new NewKidView();
            nkv.Family = new Family();
            nkv.Family.Id = Id;
            return View(nkv);
        }
        [HttpPost]
        public ActionResult AddKid(Kid kid)
        {
            FamiliesDb db = new FamiliesDb(Properties.Settings.Default.ConStr);
            db.AddKid(kid);
            TempData["Kid"] = kid;
            return Redirect($"/home/SeeKids?Id={kid.FamilyId}");
        }
    }
}