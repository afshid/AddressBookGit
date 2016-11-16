using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.ViewModels
{
    public class Login
    {
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        [Required(ErrorMessage = "Please Provide Username", AllowEmptyStrings = false)]
        //[DataType(DataType.EmailAddress)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Provide Password", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}