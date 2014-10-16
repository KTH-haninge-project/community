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
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MessageViewModels
        public ActionResult Index()
        {
            string currentuser = User.Identity.GetUserId();

            var messages = db.Messages.Where(m => m.Sender.Equals(currentuser)).ToList();
            List<MessageViewModel> messagemodels = new List<MessageViewModel>();

            foreach (var message in messages)
            {
                messagemodels.Add(new MessageViewModel(message));
            }
            return View(messagemodels);
        }

        // GET: MessageViewModels/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }

            return View(new MessageViewModel(message));
        }

        // GET: MessageViewModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TheMessage,Title,Receiver")] MessageViewModel messageViewModel)
        {
            string sender = User.Identity.GetUserId();
            string[] receivers = { messageViewModel.Receiver };
            if (ModelState.IsValid)
            {
                db.Messages.Add(new Message(messageViewModel.TheMessage, messageViewModel.Title, sender, receivers));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(messageViewModel);
        }

        // GET: MessageViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message==null)
            {
                return HttpNotFound();
            }
            return View(new MessageViewModel(message));
        }

        // POST: MessageViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TheMessage,Title,Receiver")] MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                Message message = db.Messages.Find(messageViewModel.Id);
                message.Id = messageViewModel.Id;
                message.TheMessage = messageViewModel.TheMessage;
                message.Title = messageViewModel.Title;
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messageViewModel);
        }

        // GET: MessageViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(new MessageViewModel(message));
        }

        // POST: MessageViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        
        {
           Debug.WriteLine("DELETE MESSAGE WITH ID " + id);

            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            List<ReadEntry> readentries = db.ReadEntries.Where(w => w.Message.Id == message.Id).ToList();
            foreach (ReadEntry readentry in readentries)
            {
                db.ReadEntries.Remove(readentry);
            }
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
