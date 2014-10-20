using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Community.Models;
using Microsoft.AspNet.Identity;
using System.Diagnostics;


namespace Community.Controllers
{
    [Authorize]
    public class InboxController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inbox
        public ActionResult Index()
        {
            string currentuser = User.Identity.GetUserId();
            List<ReadEntry> readEntries = db.ReadEntries.Where(r => r.Receiver.Equals(currentuser)&&r.Active).ToList<ReadEntry>();
            int countDeleted = db.ReadEntries.Count(r => r.Receiver.Equals(currentuser) && !(r.Active));
            List<MessageViewModel> messages = new List<MessageViewModel>();

            foreach (ReadEntry entry in readEntries)
            {
                MessageViewModel viewmodel = new MessageViewModel(entry.Message);
                if (entry.hasRead())
                {
                    viewmodel.Read = entry.FirstReadTime.ToString();
                }else{
                    viewmodel.Read = "[NEW]";
                }
                messages.Add(viewmodel);

            }

            messages.Reverse();
            InboxViewModel inboxview= new InboxViewModel(messages, countDeleted);
            return View(inboxview);
        }

        [HttpPost, ActionName("MarkAsRead")]
        public ActionResult Index(FormCollection collection)
        {
            string currentuser = User.Identity.GetUserId();
            //Do stuff with formCollection
            Debug.WriteLine("Index-contrller with formcollection called");
            

             if (ModelState.IsValid){
                 foreach (string _formData in collection)
                 {
                     string id = collection[_formData];
                     int idnumber = Convert.ToInt32(id);
                     ReadEntry entry = db.ReadEntries.Where(r => r.Message.Id == idnumber && r.Receiver.Equals(currentuser)).Single();
                     if (!entry.hasRead())
                     {
                         entry.FirstReadTime = System.DateTime.Now;
                     }
                 }

                 db.SaveChanges();
             }

            return RedirectToAction("Index", "Inbox");
        }

        // GET: Inbox/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string currentuser = User.Identity.GetUserId();
            Message message = db.Messages.Find(id);
            MessageViewModel messageCopy = new MessageViewModel(message);
            //set read entry to viewed
            ReadEntry entry=db.ReadEntries.Where(r => r.Message.Id ==id && r.Receiver.Equals(currentuser)).Single();
            if (!entry.hasRead()){
                entry.FirstReadTime = System.DateTime.Now;
                db.SaveChanges();
                Debug.WriteLine("time read at "+entry.FirstReadTime);
            }


            if (messageCopy == null)
            {
                return HttpNotFound();
            }
            return View(messageCopy);
        }

        // GET: Inbox/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageViewModel messageViewModel = new MessageViewModel(db.Messages.Find(id));
            if (messageViewModel == null)
            {
                return HttpNotFound();
            }
            return View(messageViewModel);
        }

        // POST: Inbox/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string receiver = User.Identity.GetUserId();
            Message message = db.Messages.Find(id);
            ReadEntry readentry = db.ReadEntries.Where(r => r.Message.Id == message.Id&&r.Receiver.Equals(receiver)).Single();
            readentry.Active = false;
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
