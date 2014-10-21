using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.ViewModels
{
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