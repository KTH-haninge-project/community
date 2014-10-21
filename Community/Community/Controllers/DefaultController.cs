using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Community.Models;
using Microsoft.AspNet.Identity;
using System.Diagnostics;




namespace Community.Controllers
{
    public class DefaultController:Controller
    {
        public DefaultController()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try{
                if( User.Identity.IsAuthenticated){
                    String id = User.Identity.GetUserId();
                    List<ReadEntry> unreadlist = db.ReadEntries.Where(r => r.Receiver.Equals(id)).ToList();

                    ViewBag.unread = unreadlist.Count();
                    String outputer = ViewBag.unread + "";
                    Debug.WriteLine("tot messages: " + outputer);
                }
            }catch (NullReferenceException e){
            
            }
        }
    }
}
