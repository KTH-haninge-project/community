using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Community.Models;
using Community.Models.ViewModels;

namespace Community.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Home
        public ActionResult Index()
        {
            HomeViewModel home = new HomeViewModel();
            home.email = "din@hårdkodade.mejl";
            home.lastLogin = DateTime.Today;
            home.loginsLastMonth = 1339;
            home.unreadMessages = 1337;
            return View(home);
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
