using System;
using System.ComponentModel.DataAnnotations;

namespace Community.Models.ViewModels
{
    /// <summary>
    /// Viewmodel for index page
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Current user e mail address
        /// </summary>
        [Display(Name = "E-mail")]
        public string email { get; set; }

        /// <summary>
        /// Current users last login before current log in
        /// </summary>
        [Display(Name = "Last login")]
        public DateTime? lastLogin { get; set; }

        /// <summary>
        /// Number of logins this month
        /// </summary>
        [Display(Name = "Logins this month")]
        public int loginsLastMonth { get; set; }

        /// <summary>
        /// Number of unread messages
        /// </summary>
        [Display(Name = "Unread messages")]
        public int unreadMessages { get; set; }

        public HomeViewModel()
        {

        }

    }
}