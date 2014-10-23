using System.Collections.Generic;

namespace Community.ViewModels
{
    /// <summary>
    /// Viewmodel for inbox
    /// </summary>
    public class InboxViewModel
    {
        public int DeletedCount { get; set; }
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