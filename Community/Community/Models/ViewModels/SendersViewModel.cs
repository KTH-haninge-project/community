using System.Collections.Generic;

namespace Community.ViewModels
{
    /// <summary>
    /// Viewmodel inbox page with list of senders
    /// </summary>
    public class SendersViewModel
    {
        /// <summary>
        /// Senders of messages
        /// </summary>
        public List<string> senders { get; set; }

        /// <summary>
        /// Number of received messages
        /// </summary>
        public int ReceivedMessages { get; set; }

        /// <summary>
        /// Number of deleted messages
        /// </summary>
        public int DeletedMessages{ get; set; }

        /// <summary>
        /// Number of read messages
        /// </summary>

        public int ReadMessages { get; set; }


        public SendersViewModel()
        {
        }


    }
}