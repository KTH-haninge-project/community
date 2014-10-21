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