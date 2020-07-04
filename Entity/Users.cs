using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using taskmanager_api.Attribute;
namespace taskmanager_api.Models
{
    public partial class Users: BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string FirstName { get; set; }
        [MaxLength(15)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(15)]
        public string Password { get; set; }
        [Required]
        [MaxLength(5)]
        [RoleUser]
        public string Role { get; set; }
        public bool SoftDeleted { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Assignment> Assignment { get; set; }
    }
}
