using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace taskmanager_api.Models {
    public partial class BaseEntity {
            public DateTime CreatedAt { get; set; }

            public DateTime UpdatedAt { get; set; }
    }
}