using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Community.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; } // Id for Entity Framework and database
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property
        public virtual List<ApplicationUser> Members { get; set; }

        public virtual ApplicationUser God { get; set; }

        public Group()
        {
            this.Members = new List<ApplicationUser>();
        }

        public void AddMember(ApplicationUser user){
            if(!Members.Contains(user)){
                Members.Add(user);
            }
        }

        public void RemoveMember(ApplicationUser user)
        {
            Members.Remove(user);
        }
    }
}