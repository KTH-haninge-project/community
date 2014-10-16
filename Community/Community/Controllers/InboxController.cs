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
            List<ReadEntry> readEntries = db.ReadEntries.Where(r => r.Receiver.Equals(currentuser)).ToList<ReadEntry>();

            List<MessageViewModel> messages = new List<MessageViewModel>();

            foreach (ReadEntry entry in readEntries)
            {
                messages.Add(new MessageViewModel(entry.Message));
            }
            return View(messages);
        }

        // GET: Inbox/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            MessageViewModel messageCopy = new MessageViewModel(message);
            //set read entry to viewed
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
            MessageViewModel messageViewModel = db.MesssagesViewModels.Find(id);
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
            MessageViewModel messageViewModel = db.MesssagesViewModels.Find(id);
            db.MesssagesViewModels.Remove(messageViewModel);
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
