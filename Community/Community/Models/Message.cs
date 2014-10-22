
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
namespace Community.Models
{
    /// <summary>
    /// Instances of this class represent a message sent by a user.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Unique Message ID
        /// </summary>
        [Key]
        public int Id { get; set; } // Id for Entity Framework and database

        /// <summary>
        /// The actual message
        /// </summary>
        [Required]
        [MaxLength(5000)]
        public string TheMessage { get; set; }

        /// <summary>
        /// Message title
        /// </summary>
         [Required]
         [MaxLength(100)]
        public string Title { get; set; }

         /// <summary>
         /// Timestamp at the sending moment
         /// </summary>
        public System.DateTime sendTimeStamp { get; set; }

        /// <summary>
        /// UserID of the sender
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// ReadEntries of the message
        /// </summary>
        public virtual List<ReadEntry> ReadEntries { get; set; }

        public Message(string message, string title, string sender, string[] receivers)
        {
            this.TheMessage = message;
            this.Title = title;
            this.Sender = sender;
            this.sendTimeStamp = System.DateTime.Now;
            ReadEntries = new List<ReadEntry>();

            foreach (string receiver in receivers)
            {
                ReadEntries.Add(new ReadEntry(this, receiver));
            }
        }

        public Message()
        {
            ReadEntries = new List<ReadEntry>();
        } 
    }
}