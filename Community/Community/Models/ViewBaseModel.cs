using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public abstract class ViewModelBase
    {
        public int unred { get; set; }
    }

    public class HomeViewModel : ViewModelBase
    {
    }
}