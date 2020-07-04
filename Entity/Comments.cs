using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace taskmanager_api.Models {
    public partial class Comments: BaseEntity{
        public int Id { get; set; }

        [Required]
        [MaxLength (500)]
        public string Messages { get; set; }

        public bool SoftDeleted { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual Users User { get; set; }
    }
}