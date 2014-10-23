using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.ViewModels
{
    /// <summary>
    /// inbox view model, containing a list of messages from a sender
    /// </summary>
    public class InboxViewModel
    {
        /// <summary>
        /// [DEPRECATED]
        /// number of deleted messages
        /// </summary>
        public int DeletedCount { get; set; }

        /// <summary>
        /// list of messages
        /// </summary>
        public List<MessageViewModel> messages { get; set; }

        /// <summary>
        /// empty constructor 
        /// </summary>
        public InboxViewModel()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="messages">list of message to display</param>
        /// <param name="DeletedCount">number of deleted for a sender[DEPRECATED]</param>
        public InboxViewModel(List<MessageViewModel> messages, int DeletedCount)
        {
            this.messages = messages;
            this.DeletedCount = DeletedCount;
        }
    }
}