
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Community.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; } // Id for Entity Framework and database

        public string TheMessage { get; set; }

        public string Title { get; set; }

        // Navigation property
        public virtual ApplicationUser Sender { get; set; }

        // Navigation property
        public virtual ICollection<ReadEntry> ReadEntries { get; set; }

        public Message(string message, string title, ApplicationUser sender, ApplicationUser[] receivers)
        {
            this.TheMessage = message;
            this.Title = title;
            this.Sender = sender;
            ReadEntries = new List<ReadEntry>();

            foreach (ApplicationUser receiver in receivers)
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