using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community.Models.ViewModels
{
    public class HomeViewModel
    {
        [Display(Name = "E-mail")]
        public string email { get; set; }

        [Display(Name = "Last login")]
        public DateTime? lastLogin { get; set; }

        [Display(Name = "Logins this month")]
        public int loginsLastMonth { get; set; }

        [Display(Name = "Unread messages")]
        public int unreadMessages { get; set; }

        public HomeViewModel()
        {

        }

    }
}