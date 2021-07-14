using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    /* View: Account/Register */
    public class RegisterAccount
    {
        [Required (ErrorMessage = "Please enter your nickname")]
        public string Nickname { get; set; }

        [Required (ErrorMessage = "Please enter your email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required (ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
    }
}
