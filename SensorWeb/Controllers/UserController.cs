using AutoMapper;
using Core;
using Core.Service;
using Core.Utils;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        IUserService _userService;
        IUserTypeService _userTypeService;
        ICompanyService _companyService;
        IMapper _mapper;
        readonly SensorWeb.Resources.CommonLocalizationService _localizer;

        public UserController(IUserService userService, IMapper mapper, IUserTypeService userTypeService, ICompanyService companyService, SensorWeb.Resources.CommonLocalizationService localizer)
        {
            _userService = userService;
            _mapper = mapper;
            _userTypeService = userTypeService;
            _companyService = companyService;
            _localizer = localizer;
        }

        // GET: UserController
        public ActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            var listaUsuarios = _userService.GetAll();

            if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                listaUsuarios = listaUsuarios.Where(u => (u.UserTypeId == 2 ||  u.UserTypeId == 3) && (u.Contact.CompanyId == userCompany || companies.Any(y => y.Id == u.Contact.CompanyId)) || u.Id == Convert.ToInt32(userId));
            }
            else if (user.UserType.Name == Constants.Roles.Supervisor)
            {
                listaUsuarios = listaUsuarios.Where(u => (u.UserTypeId == 2 && u.Contact.CompanyId == userCompany) || u.Id == Convert.ToInt32(userId));
            }

            var listaUserModel = _mapper.Map<List<UserModel>>(listaUsuarios);

            return View(listaUserModel.OrderBy(x => x.Contact.FirstName));
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            User user = _userService.Get(id);
            UserModel userModel = _mapper.Map<UserModel>(user);
            return View(userModel);
        }

       // GET: UserController/Create
        public ActionResult Create()
        {
            //ViewBag.ListUserType = _userTypeService.GetQueryDropDownList();

            //Dictionary<string, string> states = _userTypeService.GetQueryDropDownList().ToDictionary(t => t.Key.ToString(), t => t.Value.ToString());
            //ViewBag.ListUserType = states;
            return View();
        }

       // POST: UserController
       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userModel.Password.Length < 6)
                    {
                        ViewData["Error"] = "A senha deve ter no mínimo 6 caracteres";
                        return View(userModel);
                    }
                    else if (!userModel.Password.Any(p => char.IsDigit(p)) || !userModel.Password.Any(p => char.IsLetter(p)))
                    {
                        ViewData["Error"] = "A senha deve ter números e letras";
                        return View(userModel);
                    }
                    else if (userModel.Password != userModel.PasswordConfirm)
                    {
                        ViewData["Error"] = _localizer.Get("Incorrect Password");
                        return View(userModel);
                    }

                    var existingUser = _userService.GetByEmail(userModel.Email);
                    if (existingUser != null)
                    {
                        ViewData["Error"] = "Já existe um usuário cadastrado com o mesmo e-mail";
                        return View(userModel);
                    }

                    userModel.Password = MD5Hash.CalculaHash(userModel.PasswordConfirm);

                    var user = _mapper.Map<User>(userModel);

                    _userService.Insert(user);

                    try
                    {
                        var mailMsg = string.Format("<p>Bem vindo {0}</p> <p>Segue abaixo as credenciais para acesso à plataforma:</p> <p>E-mail: <strong>{1}</strong><br>Senha: <strong>{2}</strong></p>", 
                            userModel.Contact.FirstName, user.Email, userModel.PasswordConfirm);
                        SendMail.Send(userModel.Email, "Novo Cadastro - IOT NEST/VIBRAÇÃO", mailMsg);
                    }
                    catch (Exception) { }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            User user = _userService.Get(id);
            UserModel userModel = _mapper.Map<UserModel>(user);
            return View(userModel);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _userService.Get(id);
                    UserModel userModelNew = _mapper.Map<UserModel>(user);

                    userModelNew.Contact.FirstName = userModel.Contact.FirstName;
                    userModelNew.Contact.Surname = userModel.Contact.Surname;
                    userModelNew.Contact.Email = userModel.Email;
                    userModelNew.Contact.Cpf = userModel.Contact.Cpf;
                    userModelNew.Contact.Rg = userModel.Contact.Rg;
                    userModelNew.Contact.CompanyId = userModel.Contact.CompanyId;
                    userModelNew.UserTypeId = userModel.UserTypeId;

                    userModelNew.Email = userModel.Email;

                    if (!String.IsNullOrEmpty(userModel.PasswordConfirm))
                    {
                        if (userModel.Password != userModel.PasswordConfirm)
                        {
                            ViewData["Error"] = _localizer.Get("Incorrect Password");
                            return View(userModelNew);
                        }

                        userModelNew.Password = MD5Hash.CalculaHash(userModel.PasswordConfirm);
                    }                                     

                    var userUpdate = _mapper.Map<User>(userModelNew);
                    _userService.Edit(userUpdate);

                }

                return RedirectToAction(nameof(Index));

            }
            catch(Exception e )
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            User user = _userService.Get(id);
            UserModel userModel = _mapper.Map<UserModel>(user);
            return View(userModel);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, UserModel userModel)
        {
            try
            {
                _userService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string Password)
        {
            var user = _userService.Get(int.Parse(LoggedUserId));

            if (user != null)
            {
                user.Password = MD5Hash.CalculaHash(Password);
                user.Contact = null;
                user.UserType = null;
                _userService.Edit(user);

                ViewData["Error"] = _localizer.Get("Password Changed");
            }

            return View();
        }
    }
}
