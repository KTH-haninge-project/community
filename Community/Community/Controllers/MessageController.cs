using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Community.Models;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using Community.ViewModels;

namespace Community.Controllers
{
    /// <summary>
    /// Controllers for sending and viewing sent messages
    /// </summary>
    [Authorize]
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       

        /// <summary>
        /// Shows all sent messages by current user
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string currentuser = User.Identity.GetUserId();

            var messages = db.Messages.Where(m => m.Sender.Equals(currentuser)).ToList();
            List<MessageViewModel> messagemodels = new List<MessageViewModel>();

            foreach (var message in messages)
            {
                MessageViewModel viewmodel = MessageToViewModel(message);
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

        /// <summary>
        /// Shows detailed page about message sent from user
        /// </summary>
        /// <param name="id">Message id</param>
        /// <returns></returns>
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

            return View(MessageToViewModel(message));
        }

       /// <summary>
       /// Shows Message creation page
       /// </summary>
       /// <returns></returns>
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

       /// <summary>
       /// Creates message
       /// </summary>
       /// <param name="messageViewModel">Message data</param>
       /// <returns></returns>
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
      /// <summary>
      /// Shows message edit page
      /// </summary>
      /// <param name="id">Message id</param>
      /// <returns></returns>
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

            return View(MessageToViewModel(message));
        }

        /// <summary>
        /// Edits message sent by current user
        /// </summary>
        /// <param name="messageViewModel">Message data</param>
        /// <returns></returns>
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

       /// <summary>
       /// Shows message deletion page
       /// </summary>
       /// <param name="id">Message id</param>
       /// <returns></returns>
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
            return View(MessageToViewModel(message));
        }

        /// <summary>
        /// Deletes message sent by current user
        /// </summary>
        /// <param name="id">Message id</param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks whether a string is an valid email or not
        /// </summary>
        /// <param name="email">Email candidate string</param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
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

        
        public static MessageViewModel MessageToViewModel(Message message)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MessageViewModel viewmodel = new MessageViewModel();
            viewmodel.Id = message.Id;
            viewmodel.Title = message.Title;
            viewmodel.Receiver = "";
            viewmodel.Sent = message.sendTimeStamp.ToString();
            foreach (var entry in message.ReadEntries)
            {
                ApplicationUser user = db.Users.Find(entry.Receiver);
                viewmodel.Receiver += user.Email + ", ";
            
            }
            if (!viewmodel.Receiver.Equals(""))
            {
                viewmodel.Receiver = viewmodel.Receiver.Remove(viewmodel.Receiver.Length - 2);
            }
            viewmodel.TheMessage = message.TheMessage;
            var lol = db.Users.Where(u => u.Id.Equals(message.Sender)).Single();
            viewmodel.Sender = lol.Email;
            viewmodel.addressSpace = new List<string>();
            viewmodel.recvList = new List<string>();

            return viewmodel;
        }

    }
  
}
