using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public class UserStatistics
    {
        [Key]
        public string userid { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime? CurrentLogin { get; set; }
        public int numberOfLoginsThisMonth { get; set; }

        public UserStatistics(String userid)
        {
            this.userid = userid;
            this.numberOfLoginsThisMonth = 0;
        }

        public UserStatistics()
        {
        }
        public void resetLogins()
        {
            numberOfLoginsThisMonth = 0;
        }
    }
}