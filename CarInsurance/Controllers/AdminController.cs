using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class AdminController : Controller
    {

        public InsuranceEntities db = new InsuranceEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }
    }
}