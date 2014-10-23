using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Community.Models;
using Microsoft.AspNet.Identity;
using Community.ViewModels;
using System.Diagnostics;

namespace Community.Controllers
{
    /// <summary>
    /// Controller for Senders page
    /// </summary>
    public class SendersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Displays senders of active messages to the user
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string currentuser = User.Identity.GetUserId();
            List<ReadEntry> readEntries = db.ReadEntries.Where(r => r.Receiver.Equals(currentuser) && r.Active).ToList<ReadEntry>();
            int countDeleted = db.ReadEntries.Count(r => r.Receiver.Equals(currentuser) && !(r.Active));
            List<MessageViewModel> messages = new List<MessageViewModel>();
            List<string> senders = new List<string>();
            foreach (ReadEntry entry in readEntries)
            {
                string email = db.Users.Where(u => u.Id.Equals(entry.Message.Sender)).Single().Email;
                if (!senders.Contains(email))
                {
                    senders.Add(email);
                }
            }

            SendersViewModel vm = new SendersViewModel();
            vm.DeletedMessages = db.ReadEntries.Where(r => r.Receiver.Equals(currentuser) && r.Active == false).Count();
            vm.ReadMessages = db.ReadEntries.Where(r => r.Receiver.Equals(currentuser) && r.FirstReadTime!=null && r.Active).Count();
            vm.ReceivedMessages = db.ReadEntries.Where(r => r.Receiver.Equals(currentuser)).Count();
            vm.senders = senders;
            Debug.WriteLine("Number of senders in senderscontroller: " + vm.senders.Count);

            return View(vm);
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
