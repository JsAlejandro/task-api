using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using taskmanager_api.Attribute;
namespace taskmanager_api.Models {
    public partial class AssignmentShowDTO {
        public int Id { get; set; }

        [Required]
        [MaxLength (20)]
        public string Title { get; set; }

        [Required]
        [StatusTask]
        public string Status { get; set; }

        [Required]
        [MaxLength (500)]
        public string Description { get; set; }
        public bool SoftDeleted { get; set; }

        [Required]
        public int UserId { get; set; }

        
        [Required]
        public int AssigneeId {get; set;}
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}