using System;
using System.Collections.Generic;
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
    /// Controller for inbox pages
    /// </summary>
    [Authorize]
    public class InboxController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       /// <summary>
       /// Shows list of messages from a specific sender
       /// </summary>
       /// <param name="sendermail">E-mail address of sender</param>
       /// <returns>Messages from sender to user</returns>
        public ActionResult Index(String sendermail)
        {
            if (sendermail == null)
            {
                RedirectToAction("Index", "Senders");
            }
            Debug.WriteLine("REceived sendermail "+sendermail);
            Debug.WriteLine("Sendermail length: "+sendermail.Length);
            string senderid = db.Users.Where(u => u.Email.Equals(sendermail)).Single().Id;
            string currentuser = User.Identity.GetUserId();
            List<ReadEntry> readEntries = db.ReadEntries.Where(r => r.Receiver.Equals(currentuser) && r.Active && r.Message.Sender.Equals(senderid)).ToList<ReadEntry>();
            int countDeleted = db.ReadEntries.Count(r => r.Receiver.Equals(currentuser) && !(r.Active));
            List<MessageViewModel> messages = new List<MessageViewModel>();

            foreach (ReadEntry entry in readEntries)
            {
                MessageViewModel viewmodel = MessageController.MessageToViewModel(entry.Message);
                if (entry.hasRead())
                {
                    viewmodel.Read = entry.FirstReadTime.ToString();
                }
                else
                {
                    viewmodel.Read = "[NEW]";
                }
                if (viewmodel.TheMessage.Length > 15)
                {
                    viewmodel.TheMessage = viewmodel.TheMessage.Substring(0, 10) + "...";
                }
                if (viewmodel.Title.Length > 15)
                {
                    viewmodel.Title = viewmodel.Title.Substring(0, 10) + "...";
                }
                messages.Add(viewmodel);
            }

            messages.Reverse();
            InboxViewModel inboxview = new InboxViewModel(messages, countDeleted);
            return View(inboxview);
        }


        /// <summary>
        /// Marks messages as read
        /// </summary>
        /// <param name="collection">Collection of messages</param>
        /// <returns></returns>
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
                     Debug.WriteLine("Inbox controller MarkAsRead vill göra en int av detta: "+id);
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

        /// <summary>
        /// Shows detailed page of a specific message
        /// </summary>
        /// <param name="id">Message id</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string currentuser = User.Identity.GetUserId();
            Message message = db.Messages.Find(id);
            MessageViewModel messageCopy = MessageController.MessageToViewModel(message);
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

        /// <summary>
        /// Shows message deletion page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageViewModel messageViewModel = MessageController.MessageToViewModel(db.Messages.Find(id));
            if (messageViewModel == null)
            {
                return HttpNotFound();
            }
            return View(messageViewModel);
        }

        /// <summary>
        /// Removes a message from receivers inbox by marking the ReadEntry as inactive
        /// </summary>
        /// <param name="id">ID of the message</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string receiver = User.Identity.GetUserId();
            Message message = db.Messages.Find(id);
            List<ReadEntry> entries = db.ReadEntries.Where(r => r.Message.Id == message.Id&&r.Receiver.Equals(receiver)).ToList();
            foreach(var readentry in entries){
                readentry.Active=false;
            }
            db.SaveChanges();

            string sendermail = db.Users.Where(u => u.Id.Equals(message.Sender)).Single().Email;
            return RedirectToAction("Index", "Senders");
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
