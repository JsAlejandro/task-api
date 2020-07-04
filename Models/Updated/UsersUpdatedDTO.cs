using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using taskmanager_api.Attribute;
namespace taskmanager_api.Models {
    public partial class UsersUpdatedDTO {
    
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
    }
}