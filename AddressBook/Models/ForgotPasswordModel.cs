using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "We need your email to send you a reset link!")]
        [Display(Name = "Your account email")]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
    }
}