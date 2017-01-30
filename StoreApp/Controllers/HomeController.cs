using StoreApp.Core;
using StoreApp.Data;
using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    public class HomeController : Controller
    {
        private StoreDbContext _db;
        public HomeController(StoreDbContext db)
        {
            _db = db;
        }

        public HomeController()
        {
            _db = new StoreDbContext();
        }

        public ActionResult Index()
        {
            // have a single store for now. will possilby have more later
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Us";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us";

            return View();
        }
    }
}