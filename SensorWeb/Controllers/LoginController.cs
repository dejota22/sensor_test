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
using Core.Utils;
using MySqlX.XDevAPI;
using SensorWeb.Models;
using System.Text;
using System.Web;

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
        public ActionResult Login(string? qr)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (qr != null)
                {
                    //User.Claims.
                    return RedirectToAction("ActionQR", "Device", new { dCode = qr, obfs = true });
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.qr = qr;
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

            if (returnUrl != null && returnUrl.Contains("obfs"))
            {
                Uri myUri = new Uri($"http://localhost{returnUrl}");
                string dCode = HttpUtility.ParseQueryString(myUri.Query).Get("dCode");
                ViewBag.qr = dCode;
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
                    new Claim(ClaimTypes.Role, userLogin.UserType.Name)
                };

            var claimsIdentity = new ClaimsIdentity(claims, "Login");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect(returnUrl ?? "/Home/Index");
        }

        public async Task<ActionResult> Logout(string qr = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (qr == null)
                return RedirectToAction("Login", "Login");
            else
                return RedirectToAction("Login", "Login", new { qr = Convert.ToBase64String(Encoding.UTF8.GetBytes(qr)) });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult EsqueciSenha(string email)
        {
            User user = _userService.GetByEmail(email);

            if (user != null)
            {
                string senha = MD5Hash.GerarSenhaAleatoria();
                user.Password = MD5Hash.CalculaHash(senha);
                user.Contact = null;
                user.UserType = null;
                _userService.Edit(user);

                try
                {
                    var mailMsg = string.Format("<p>Olá {0}</p> <p>Segue abaixo as credenciais para acesso à plataforma:</p> <p>E-mail: <strong>{1}</strong><br>Senha: <strong>{2}</strong></p>",
                        user.Contact.FirstName, user.Email, senha);
                    SendMail.Send(user.Email, "Esqueci minha senha - IOT NEST/VIBRAÇÃO", mailMsg);

                    ViewData["Error"] = "E-mail enviado";
                }
                catch (Exception) { ViewData["Error"] = "Erro ao enviar e-mail"; }
            }
            else
            {
                ViewData["Error"] = _localizer.Get("UserLoginInvalid");
            }

            return View("Login");
        }
    }
}
