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
            MessageViewModel messageViewModel = db.MesssagesViewModels.Find(id);
            if (messageViewModel == null)
            {
                return HttpNotFound();
            }
            return View(messageViewModel);
        }

        // GET: Inbox/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inbox/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Receiver,TheMessage,Sender")] MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                db.MesssagesViewModels.Add(messageViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(messageViewModel);
        }

        // GET: Inbox/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Inbox/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Receiver,TheMessage,Sender")] MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messageViewModel);
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
