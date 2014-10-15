
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Community.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; } // Id for Entity Framework and database

        public string TheMessage { get; set; }

        // Navigation property
        public virtual ApplicationUser Sender { get; set; }

        // Navigation property
        public virtual ICollection<ReadEntry> ReadEntries { get; set; }

        public Message(string message, ApplicationUser sender, ApplicationUser[] receivers)
        {
            this.TheMessage = message;
            this.Sender = sender;
            ReadEntries = new List<ReadEntry>();

            foreach (ApplicationUser receiver in receivers)
            {
                ReadEntries.Add(new ReadEntry(this, receiver));
            }
        }
    }
}