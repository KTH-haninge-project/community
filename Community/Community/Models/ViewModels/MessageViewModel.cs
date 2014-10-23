using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Community.Models;

namespace Community.ViewModels
{
    /// <summary>
    /// MessageViewModel, copy of Message for view, filled by message  and readentry
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// Id for Entity Framework and database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// deleted or not
        /// only for receivers
        /// </summary>
        public Boolean Deleted { get; set; }

        /// <summary>
        /// Read or not 
        /// only for receivers
        /// </summary>
        public String Read { get; set; }

        /// <summary>
        /// date and time when sent
        /// </summary>
        public String Sent { get; set; }

        /// <summary>
        /// availible addresses to send to
        /// only for sender
        /// </summary>
        public List<String> addressSpace { get; set; }
        
        /// <summary>
        /// selected addresses to send to 
        /// only for sender
        /// </summary>
        public List<String> recvList { get; set; }

        /// <summary>
        /// title of message
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        /// <summary>
        /// deprecated
        /// </summary>
         public string Receiver { get; set; }

        /// <summary>
        /// the actual message, max 5000 chars
        /// </summary>
         [Display(Name = "Message")]
         [Required]
         [MaxLength(5000, ErrorMessage = "Message cannot be longer than 5000 characters")]
        public string TheMessage { get; set; }

        /// <summary>
        /// the sender of the message
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// empty constructor
        /// </summary>
        public MessageViewModel() 
        {
            addressSpace = new List<String>();
            recvList = new List<string>();
        }

        /// <summary>
        /// fills half the message
        /// </summary>
        /// <param name="message">message to fill with</param>
        public MessageViewModel(Message message)
        {
            var db = new ApplicationDbContext();
            this.Id = message.Id;
            this.Title = message.Title;
            this.Receiver = "";
            this.Sent = message.sendTimeStamp.ToString();
            //depricated? add recivers from textinput field
            foreach (var entry in message.ReadEntries)
            {
                ApplicationUser user = db.Users.Find(entry.Receiver);
                Receiver += user.Email + ", ";
            
            }
            if (!Receiver.Equals(""))//remove last " ,"
            {
                Receiver = Receiver.Remove(Receiver.Length - 2);
            }
            //Debug.WriteLine("Receiver: "+Receiver);
            this.TheMessage = message.TheMessage;
            var tempuser = db.Users.Where(u => u.Id.Equals(message.Sender)).Single();
            this.Sender = tempuser.Email;

            addressSpace=new List<String>();

            recvList = new List<string>();

            
        }



    }
}