using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

/// Instances of this class represents a message group with a list of members and an owner 
/// (who necessarily does not need to be a member)
/// 

namespace Community.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; } // Id for Entity Framework and database



        /// <summary>
        /// Name of group. Can be swedish letters(a-ö,A-Ö), numbers and white spaces
        /// </summary>
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zåäöA-ZÅÄÖ0-9''-'\s]{1,40}$")]
        public string Name { get; set; }


        /// <summary>
        /// Group description. Can be any kind of character.
        /// </summary>
        [MaxLength(5000)]
        public string Description { get; set; }


        /// <summary>
        /// List of members of the group. Members receives group messages
        /// </summary>
        public virtual ICollection<ApplicationUser> Members { get; set; }

        /// <summary>
        /// The owner of the group. Is by default a member of the group (but not necessarily)
        /// </summary>
        public virtual ApplicationUser Owner { get; set; }

        public Group()
        {
            this.Members = new List<ApplicationUser>();
        }

        /// <summary>
        /// Adds a new member to the group if it not already exists
        /// </summary>
        /// <param name="user">The new member</param>
        public void AddMember(ApplicationUser user){
            if(!Members.Contains(user)){
                Members.Add(user);
            }
        }
        /// <summary>
        /// Removes a member from the group if it exists in the group
        /// </summary>
        /// <param name="user">The member that should be removed from the group</param>
        public void RemoveMember(ApplicationUser user)
        {
            Members.Remove(user);
        }
    }

}