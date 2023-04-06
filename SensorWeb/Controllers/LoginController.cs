using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Core;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class LoginController : BaseController
    {
        IUserService _userService;
        readonly Resources.CommonLocalizationService _localizer;

        public LoginController(IUserService userService, Resources.CommonLocalizationService localizer)
        {
            _userService = userService;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string username, string pass, string returnUrl)
        {
            if (username is null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (pass is null)
            {
                throw new ArgumentNullException(nameof(pass));
            }

            var passCrypto = MD5Hash.CalculaHash(pass);

            var userLogin = _userService.Login(username, passCrypto);

            if (userLogin is null)
            {
                ViewData["Error"] = _localizer.Get("UserLoginInvalid");
                return View();
            }

            if (userLogin != null && userLogin.IsActive == 0)
            {
                ViewData["Error"] = _localizer.Get("This user is inactive");
                return View();
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()),
                    new Claim(ClaimTypes.Email, userLogin.Email),
                    // new Claim(ClaimTypes.Role, userLogin.UserType.ToString())
                };

            var claimsIdentity = new ClaimsIdentity(claims, "Login");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect(returnUrl ?? "/Home/Index");
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Login");
        }
    }
}
