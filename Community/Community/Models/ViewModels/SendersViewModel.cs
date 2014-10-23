using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.ViewModels
{
    /// <summary>
    /// View model for senders view, containing list of senders and simple stats
    /// </summary>
    public class SendersViewModel
    {
        /// <summary>
        /// list of senders 
        /// </summary>
        public List<string> senders { get; set; }

        /// <summary>
        /// number of received messages
        /// </summary>
        public int ReceivedMessages { get; set; }
        /// <summary>
        /// number of deleted messages
        /// </summary>
        public int DeletedMessages{ get; set; }
        /// <summary>
        /// number of read messages
        /// </summary>
        public int ReadMessages { get; set; }


        public SendersViewModel()
        {
        }


    }
}