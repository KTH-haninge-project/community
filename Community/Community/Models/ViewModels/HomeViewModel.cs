using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community.Models.ViewModels
{
    /// <summary>
    /// view model class for home page
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// users email
        /// </summary>
        [Display(Name = "E-mail")]
        public string email { get; set; }

        /// <summary>
        /// date of last login
        /// </summary>
        [Display(Name = "Last login")]
        public DateTime? lastLogin { get; set; }

        /// <summary>
        /// number of logins last month
        /// </summary>
        [Display(Name = "Logins this month")]
        public int loginsLastMonth { get; set; }

        /// <summary>
        /// number of unread messages
        /// </summary>
        [Display(Name = "Unread messages")]
        public int unreadMessages { get; set; }


        /// <summary>
        /// empty constructor
        /// </summary>
        public HomeViewModel()
        {

        }

    }
}