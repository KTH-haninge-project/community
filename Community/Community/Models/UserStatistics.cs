using System;
using System.ComponentModel.DataAnnotations;

namespace Community.Models
{
    /// <summary>
    /// Instances of this class represents login statistics to a user
    /// </summary>
    public class UserStatistics
    {
        /// <summary>
        /// UserID of the user
        /// </summary>
        [Key]
        public string userid { get; set; }

        /// <summary>
        /// Date and time on last time user logged in (the login moment before the current log in)
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// Date and time of the latest login moment
        /// </summary>
        public DateTime? CurrentLogin { get; set; }

        /// <summary>
        /// Number of logins this month(Restored to 1 when user signs in on a new month)
        /// </summary>
        public int numberOfLoginsThisMonth { get; set; }

        public UserStatistics(String userid)
        {
            this.userid = userid;
            this.numberOfLoginsThisMonth = 0;
        }

        public UserStatistics()
        {
        }
    }
}