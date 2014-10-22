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
    /// <summary>
    /// Home pages controller
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Shows index page with user statistics
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            HomeViewModel home = new HomeViewModel();
            UserStatistics stats = db.UserStatistics.Where(s => s.userid.Equals(user.Id)).Single();
            home.email = user.Email;
            home.lastLogin = stats.LastLogin;
            home.loginsLastMonth = stats.numberOfLoginsThisMonth;
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
