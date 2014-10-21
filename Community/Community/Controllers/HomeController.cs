using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Community.Models;
using System.Diagnostics;
using Microsoft.AspNet.Identity;

namespace Community.Controllers
{
    public class HomeController : DefaultController
    {
        public ActionResult Index()
        { //ViewBag.unread="";
            ViewBag.unread = "not logedins";
            //try{
            if (User.Identity.IsAuthenticated)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                String id = User.Identity.GetUserId();
                int unread = 0;
                List<ReadEntry> unreadlist = db.ReadEntries.Where(r => r.Receiver.Equals(id)).ToList();
                foreach(ReadEntry read in unreadlist){
                    if(!read.hasRead()){
                        unread++;
                    }
                }
                ViewBag.unread = "[" + unread + "]";
                String outputer = ViewBag.unread + "";
                Debug.WriteLine("tot messages: " + outputer);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}