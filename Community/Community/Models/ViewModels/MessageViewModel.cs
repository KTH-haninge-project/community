using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Community.Models;

namespace Community.ViewModels
{
    /// <summary>
    /// Viewmodel for messages
    /// </summary>
    public class MessageViewModel
    {
        public int Id { get; set; } // Id for Entity Framework and database

        public Boolean Deleted { get; set; }

        public String Read { get; set; }

        public String Sent { get; set; }

        public List<String> addressSpace { get; set; }
        
        public List<String> recvList { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

         public string Receiver { get; set; }

         [Display(Name = "Message")]
         [Required]
         [MaxLength(5000, ErrorMessage = "Message cannot be longer than 5000 characters")]
        public string TheMessage { get; set; }


        public string Sender { get; set; }

        public MessageViewModel() 
        {
            addressSpace = new List<String>();
            recvList = new List<string>();
        //empty constructor 
        }
    }
}