using System.Collections.Generic;

namespace Community.ViewModels
{
    /// <summary>
    /// Viewmodel for inbox
    /// </summary>
    public class InboxViewModel
    {
        /// <summary>
        /// Number of deleted messages
        /// </summary>
        public int DeletedCount { get; set; }
        /// <summary>
        /// Messages
        /// </summary>
        public List<MessageViewModel> messages { get; set; }

        public InboxViewModel()
        {

        }
        public InboxViewModel(List<MessageViewModel> messages, int DeletedCount)
        {
            this.messages = messages;
            this.DeletedCount = DeletedCount;
        }
    }
}