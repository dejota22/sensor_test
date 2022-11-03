using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Core;
using System;

namespace SensorWeb.Controllers
{
    public class LoginController : BaseController
    {

        IUserService _userService;
        
        readonly SensorWeb.Resources.CommonLocalizationService _localizer;

        public LoginController(IUserService userService, SensorWeb.Resources.CommonLocalizationService localizer)
        {
            _userService = userService;
            _localizer = localizer;
        }

        [HttpGet]
        public ActionResult Login()
        {            
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string username, string pass)
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

            return RedirectToAction("Index", "Home");
        }

    }
}
