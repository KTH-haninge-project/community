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
        /// <summary>
        /// ID of Message
        /// </summary>
        public int Id { get; set; } // Id for Entity Framework and database

        /// <summary>
        /// True if message has been deleted
        /// </summary>
        public Boolean Deleted { get; set; }

        /// <summary>
        /// True if message has been read
        /// </summary>
        public String Read { get; set; }

        /// <summary>
        /// Date and time when message was sent
        /// </summary>
        public String Sent { get; set; }

        /// <summary>
+        /// availible addresses to send to
+        /// only for sender
+        /// </summary>
        public List<String> addressSpace { get; set; }
        
        /// <summary>
        /// List of receivers of message
        /// </summary>
        public List<String> recvList { get; set; }

        /// <summary>
        /// Message title
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        /// <summary>
        /// String representation of all receivers
        /// </summary>
         public string Receiver { get; set; }

         [Display(Name = "Message")]
         [Required]
         [MaxLength(5000, ErrorMessage = "Message cannot be longer than 5000 characters")]
        public string TheMessage { get; set; }

        /// <summary>
        /// ID of message sender
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public MessageViewModel() 
        {
            addressSpace = new List<String>();
            recvList = new List<string>();
        }
    }
}