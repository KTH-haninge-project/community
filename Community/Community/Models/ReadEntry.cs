using System.ComponentModel.DataAnnotations;

namespace Community.Models
{
    /// <summary>
    /// Instances of this class represents a message in an inbox belonging to a user.
    /// Object keeps track on whether the receiver has read the message
    /// </summary>
    public class ReadEntry
    {
        public ReadEntry()
        {

        }

        /// <summary>
        /// Unique ID of the ReadEntry
        /// </summary>
        [Key]
        public int Id { get; set; } // Id for Entity Framework and database

        /// <summary>
        /// Date and time on the first time the receiver opened the message
        /// </summary>
        public System.DateTime? FirstReadTime { get; set; }

        /// <summary>
        /// Reference to the message
        /// </summary>
        public virtual Message Message { get; set; }

        /// <summary>
        /// UserID of the receiver
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// Keeps track on whether the message has been deleted from receivers inbox
        /// If Active==true => Message is in the inbox
        /// If Active==false => Message has been removed from the inbox
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Returns whether the receiver has read the message or not
        /// </summary>
        /// <returns>If the receiver has read the message or not</returns>
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