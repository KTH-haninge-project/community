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
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Message
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

        // GET: Message/Details/5
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

        // GET: Message/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TheMessage,Title")] MessageViewModel messageViewModel)
        {
            string sender = User.Identity.GetUserId();
            string[] receivers={"hej"};
            if (ModelState.IsValid)
            {
                db.Messages.Add(new Message(messageViewModel.TheMessage,messageViewModel.Title,sender,receivers));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(messageViewModel);
        }

        // GET: Message/Edit/5
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

        // POST: Message/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TheMessage,Title,Sender")] MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messageViewModel);
        }

        // GET: Message/Delete/5
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

        // POST: Message/Delete/5
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
