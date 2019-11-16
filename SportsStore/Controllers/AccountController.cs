using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager; // to retrieve user's information
        private SignInManager<IdentityUser> signInManager; // to validate the password of the user and allow the user found by the userManager to log in

        public AccountController(UserManager<IdentityUser> userMgr,
             SignInManager<IdentityUser> signInMgr) // MVC will create the userMgr for us (Dependency injection) because we use "app.UseAuthentication();" inside the Startup
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }

        [AllowAnonymous] // exception for the Authorize flag
        // Login is a get method, the key-value pair will be appended in the URL of a get request
        // for example, localhost:53672/Account/Login?ReturnUrl=%2FAdmin%2FIndex
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken] // extra layer for security, extra validation
        // in order to manage async action method, we need to use Task<>
        // before, our parameter are string name and string password
        // by using ViewModel, if we have more parameters, it's more neat
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginModel.Name); // userManager doesn't provide validation of password

                if (user != null)
                {
                    if ((await signInManager.PasswordSignInAsync(user,    // signInManager provide password validation
                        loginModel.Password, false, false)).Succeeded)    // check weather the password matches the user
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/Admin/Index");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid name or password");
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
