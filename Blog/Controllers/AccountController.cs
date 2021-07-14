using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        /* Page login to account*/
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            // Go to the view Account/Login
            return View();
        }

        /* Login to account */
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginAccount user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "PersonalArea");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login");
            }

            return View("Login", user);
        }

        /*Page register account*/
        public IActionResult Register()
        {
            // Go to the view Account/Register
            return View();
        }

        /* Register new account */
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterAccount model)
        {
            if (_userManager.Users.Any(x => x.Nickname == model.Nickname)) ModelState.AddModelError("Nickname", "Nickname already exists");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Nickname = model.Nickname,
                    UserName = model.Email, // UserName can be only email
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                // Show list of errors in ValidationSummary
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("Register", model);
        }

        /* Logout from account */
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
