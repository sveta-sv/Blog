using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Models
{
    /* View: Account/Login, Account/Register */
    public class ApplicationUser: IdentityUser
    {
        public string Nickname { get; set; } // All properties of Identity and one new
    } 
}
