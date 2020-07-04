using System;
using System.ComponentModel.DataAnnotations;
namespace taskmanager_api.Attribute {
    public class StatusTaskAttribute : ValidationAttribute {
        public string[] status = { "accept", "reject", "start", "unstart" };
        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {
            if (value == null || string.IsNullOrEmpty (value.ToString ())) {
                return ValidationResult.Success;
            } else if (Array.IndexOf (status, value.ToString ()) > -1) {
                return ValidationResult.Success;
            } else {
                return new ValidationResult ("The Status field is not a valid");
            }
        }

    }

}