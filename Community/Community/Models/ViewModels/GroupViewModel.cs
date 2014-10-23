using Community.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Community.ViewModels
{
    /// <summary>
    /// View Model for groups for users 
    /// </summary>
    public class GroupViewModel
    {
        /// <summary>
        /// Id for Entity Framework and database
        /// </summary>
        [Key]
        public int Id { get; set; } 

        /// <summary>
        /// name of group
        /// Only swedish letters, numbers, and blank spaces are allowed in group name
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "Group name must be at least 3 characters long")]
        [MaxLength(100, ErrorMessage="Group name cannot be longer than 100 characters")]
        [RegularExpression(@"^[a-zåäöA-ZÅÄÖ0-9''-'\s]{1,40}$", ErrorMessage =
            "Only swedish letters, numbers, and blank spaces are allowed in group name")]
        public string Name{ get; set; }

        /// <summary>
        /// group description
        /// </summary>
        [MaxLength(5000)]
        public string Description { get; set; }

        /// <summary>
        /// members in group
        /// </summary>
        public List<String> Members { get; set; }

        /// <summary>
        /// value for view
        /// </summary>
        public Boolean isMember { get; set; }

        /// <summary>
        /// value for view
        /// </summary>
        public Boolean isOwner { get; set; }

        /// <summary>
        /// empty constructor
        /// </summary>
        public GroupViewModel()
        {

        }

        /// <summary>
        /// create viewmodel group from group
        /// </summary>
        /// <param name="group">group to add values from</param>
        public GroupViewModel(Group group)
        {
            this.isOwner = false;
            this.isMember = false;
            this.Name = group.Name;
            this.Description = group.Description;
            this.Members = new List<string>();
            this.Id = group.Id;
            foreach (ApplicationUser user in group.Members)
            {
                Members.Add(user.Email);
            }
        }
    }
}