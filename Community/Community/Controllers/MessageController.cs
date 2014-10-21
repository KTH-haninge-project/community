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
                MessageViewModel viewmodel = new MessageViewModel(message);
                if (viewmodel.TheMessage.Length > 15)
                {
                    viewmodel.TheMessage = viewmodel.TheMessage.Substring(0, 10) + "...";
                }
                if (viewmodel.Title.Length > 15)
                {
                    viewmodel.Title = viewmodel.Title.Substring(0, 10) + "...";
                }
                messagemodels.Add(viewmodel);
            }
            messagemodels.Reverse();
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

            MessageViewModel mvm=new MessageViewModel();
            List<ApplicationUser> users = db.Users.ToList();
            List<Group> gruops = db.Users.Find(User.Identity.GetUserId()).GroupMemberships.ToList();

            foreach (ApplicationUser user in users)
            {
                mvm.addressSpace.Add(user.Email);
            }
            foreach (Group group in gruops)
            {
                mvm.addressSpace.Add(group.Name);
            }

            return View(mvm);
        }

        // POST: MessageViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TheMessage,Title,recvList")] MessageViewModel messageViewModel)
        {
            string sender = User.Identity.GetUserId();
            
            //string[] receiveremails= messageViewModel.Receiver.Split(',');
            List<string> receiveremails = messageViewModel.recvList;

            List<string> receiverids = new List<string>();

            foreach (string receivername in receiveremails)
            {
                if (IsValidEmail(receivername)) // Receiver is a email address
                {
                    ApplicationUser receiver = db.Users.Where(u => u.Email.Equals(receivername)).Single();
                    if (!receiverids.Contains(receiver.Id))
                    {
                        receiverids.Add(receiver.Id);
                    }
                }
                else // Receiver is a group
                {
                    Group group = db.Groups.Where(g => g.Name.Equals(receivername)).Single();

                    foreach (ApplicationUser user in group.Members)
                    {
                        if (!receiverids.Contains(user.Id))
                        {
                            receiverids.Add(user.Id);
                        }
                    }
                }
            }
            
            if (ModelState.IsValid)
            {
                db.Messages.Add(new Message(messageViewModel.TheMessage, messageViewModel.Title, sender, receiverids.ToArray()));
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
            string currentuser = User.Identity.GetUserId();
            if (!message.Sender.Equals(currentuser))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
                string currentuser = User.Identity.GetUserId();
                if (!message.Sender.Equals(currentuser))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
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
            string currentuser = User.Identity.GetUserId();
            if (!message.Sender.Equals(currentuser))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
            string currentuser = User.Identity.GetUserId();
            if (!message.Sender.Equals(currentuser))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
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

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
  
}
