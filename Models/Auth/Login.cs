using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace taskmanager_api.Models{
    public partial class Login {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}