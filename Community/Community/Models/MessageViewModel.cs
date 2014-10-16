using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;

namespace Community.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; } // Id for Entity Framework and database



        public string Title { get; set; }

         public string Receiver { get; set; }

            [Display(Name = "Message")]
        public string TheMessage { get; set; }


        public string Sender { get; set; }

        public MessageViewModel() 
        { 
        //empty constructor 
        }

        public MessageViewModel(Message message)
        {
            this.Id = message.Id;
            this.Title = message.Title;
            this.Receiver = "";
            foreach (var entry in message.ReadEntries)
            {
                Receiver += entry.Receiver + ", ";
            }
           
           
            this.TheMessage = message.TheMessage;
            var db = new ApplicationDbContext();
            var lol = db.Users.Where(u => u.Id.Equals(message.Sender)).Single();
            this.Sender = lol.Email;
        }


    }
}