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



        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zåäöA-ZÅÄÖ0-9''-'\s]{1,40}$")]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<ApplicationUser> Members { get; set; }

        public virtual ApplicationUser Owner { get; set; }

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