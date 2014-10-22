using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.ViewModels
{
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