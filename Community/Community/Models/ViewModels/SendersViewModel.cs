using System.Collections.Generic;

namespace Community.ViewModels
{
    /// <summary>
    /// Viewmodel inbox page with list of senders
    /// </summary>
    public class SendersViewModel
    {
        public List<string> senders { get; set; }

        public int ReceivedMessages { get; set; }
        public int DeletedMessages{ get; set; }

        public int ReadMessages { get; set; }


        public SendersViewModel()
        {
        }


    }
}