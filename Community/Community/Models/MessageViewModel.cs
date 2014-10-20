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

namespace Community.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; } // Id for Entity Framework and database

        public Boolean Deleted { get; set; }

        public String Read { get; set; }

        public String Sent { get; set; }

        public List<String> addressSpace { get; set; }
        
        public List<String> recvList { get; set; }

        public string Title { get; set; }

         public string Receiver { get; set; }

            [Display(Name = "Message")]
        public string TheMessage { get; set; }


        public string Sender { get; set; }

        public MessageViewModel() 
        {
            addressSpace = new List<String>();
            recvList = new List<string>();
        //empty constructor 
        }

        public MessageViewModel(Message message)
        {
            var db = new ApplicationDbContext();
            this.Id = message.Id;
            this.Title = message.Title;
            this.Receiver = "";
            this.Sent = message.sendTimeStamp.ToString();
            foreach (var entry in message.ReadEntries)
            {
                ApplicationUser user = db.Users.Find(entry.Receiver);
                Receiver += user.Email + ", ";
            
            }
            if (!Receiver.Equals(""))
            {
                Receiver = Receiver.Remove(Receiver.Length - 2);
            }
            Debug.WriteLine("Receiver: "+Receiver);
            this.TheMessage = message.TheMessage;
            var lol = db.Users.Where(u => u.Id.Equals(message.Sender)).Single();
            this.Sender = lol.Email;
            addressSpace=new List<String>();

            recvList = new List<string>();

            
        }



    }
}