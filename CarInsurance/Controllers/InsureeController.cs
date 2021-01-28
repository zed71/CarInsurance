using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        public InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Add Logic HERE
                // Start with base quote = $50
                insuree.Quote = 50;

                int insureeAge = DateTime.Now.Year - insuree.DateOfBirth.Year;

                // Adjust quote based on user's age
                if(insureeAge <= 18)
                {
                    insuree.Quote += 100;
                }
                else if((insureeAge >= 19) && (insureeAge <= 25))
                {
                    insuree.Quote += 50;
                }
                else
                {
                    insuree.Quote += 25;
                }

                // Adjust quote based on car's year of manufacture
                if((insuree.CarYear < 2000) || (insuree.CarYear > 2015))
                {
                    insuree.Quote += 25;
                }

                // Adjust quote if Car Make is a Porsche
                if(insuree.CarMake == "Porsche")
                {
                    insuree.Quote += 25;
                }

                // Adjust quote Car Make is a Porsche and CarModel is 911 Carrera
                if((insuree.CarMake == "Porsche") && (insuree.CarModel == "911 Carrera"))
                {
                    insuree.Quote += 25;
                }

                // Add $10 to the monthly total for each speedingTicket
                for(int i = 0; i < insuree.SpeedingTickets; i++)
                {
                    insuree.Quote += 10;
                }

                // If user has DUI, add 25% to total
                decimal DUIQuote = decimal.Multiply(insuree.Quote, .25m);
                if (insuree.DUI == true)
                {
                    insuree.Quote += DUIQuote;
                }

                // If has Full Coverage, add 50% to total
                decimal fullCoverage = decimal.Multiply(insuree.Quote, .5m);
                if(insuree.CoverageType == true)
                {
                    insuree.Quote += fullCoverage;
                }
                
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
