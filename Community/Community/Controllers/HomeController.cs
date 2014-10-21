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
using Microsoft.AspNet.Identity;

namespace Community.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Home
        public ActionResult Index()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            HomeViewModel home = new HomeViewModel();
            home.email = user.Email;
            home.lastLogin = user.lastLogin;
            home.loginsLastMonth = user.loginMonthCounter;
            home.unreadMessages = db.ReadEntries.Where(r => r.Receiver.Equals(user.Id)&&r.Active&&r.FirstReadTime==null).Count();
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
