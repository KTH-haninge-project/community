using System.ComponentModel.DataAnnotations;

namespace Community.Models
{
    public class ReadEntry
    {
        public ReadEntry()
        {

        }

        [Key]
        public int Id { get; set; } // Id for Entity Framework and database

        public System.DateTime? FirstReadTime { get; set; }

        // Navigation property
        public virtual Message Message { get; set; }

        public string Receiver { get; set; }

        public bool Active { get; set; }

        public bool hasRead()
        {
            return (FirstReadTime != null);
        }

        public ReadEntry(Message message, string receiver)
        {
            FirstReadTime = null;
            this.Message = message;
            this.Receiver = receiver;
            this.Active = true;
        }
    }

}