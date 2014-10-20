using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Community.Models
{
    public class GroupViewModel
    {
        [Key]
        public int Id { get; set; } // Id for Entity Framework and database

        public string Name{ get; set; }
        public string Description { get; set; }

        public List<String> Members { get; set; }
        public Boolean isMember { get; set; }

        public GroupViewModel()
        {

        }

        public GroupViewModel(Group group)
        {
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