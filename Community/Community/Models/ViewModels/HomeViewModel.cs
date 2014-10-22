using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community.Models.ViewModels
{
    public class HomeViewModel
    {
    [Key]
        public string email { get; set; }
        public DateTime? lastLogin { get; set; }

        public int loginsLastMonth { get; set; }

        public int unreadMessages { get; set; }

        public HomeViewModel()
        {

        }

    }
}