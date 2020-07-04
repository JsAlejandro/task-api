using System;
using System.ComponentModel.DataAnnotations;
namespace taskmanager_api.Attribute {
    public class RoleUserAttribute : ValidationAttribute {
        private string[] roles = { "owner", "requester" };
        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {
            if (value == null || string.IsNullOrEmpty (value.ToString ())) {
                return ValidationResult.Success;

            } else if (Array.IndexOf (roles, value.ToString ()) > -1) {
                return ValidationResult.Success;
            } else {
                return new ValidationResult ("The Role field is not a valid");

            }
        }

    }

}