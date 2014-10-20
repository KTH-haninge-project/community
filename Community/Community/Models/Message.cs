
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
namespace Community.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; } // Id for Entity Framework and database

        public string TheMessage { get; set; }

        public string Title { get; set; }

        public System.DateTime sendTimeStamp { get; set; }

        // Navigation property
        public string Sender { get; set; }

        // Navigation property
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