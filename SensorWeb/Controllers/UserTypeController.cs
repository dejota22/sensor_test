using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SensorWeb.Controllers
{
    public class UserTypeController : BaseController
    {
        IUserTypeService _UserTypeService;
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="UserTypeService"></param>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public UserTypeController(IUserTypeService UserTypeService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer)
        {
            _UserTypeService = UserTypeService;
            _mapper = mapper;
            _localizer = localizer;
        }

        // GET: UserTypeController
        public ActionResult Index()
        {
            var listaUsuarios = _UserTypeService.GetAll();
            var listaUserTypeModel = _mapper.Map<List<UserTypeModel>>(listaUsuarios);

            //foreach (var UserTypeModel in listaUserTypeModel)
            //{
            //    UserTypeModel.Contact = _mapper.Map<List<ContactModel>>(_UserTypeService.GetAll())
            //}

            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
          //  ViewData["Title"] = _localizer["UserTypeTittle"];
            return View(listaUserTypeModel.OrderBy(x => x.Id));
        }

        // GET: UserTypeController/Details/5
        public ActionResult Details(int id)
        {
            UserType UserType = _UserTypeService.Get(id);
            UserTypeModel UserTypeModel = _mapper.Map<UserTypeModel>(UserType);
            return View(UserTypeModel);
        }

        // GET: UserTypeController/Create
        public ActionResult Create()
        {
            UserTypeModel user = new UserTypeModel()
            {
                Id = _UserTypeService.GetlastCode()
            };

            return View(user);
        }

        // POST: UserTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserTypeModel UserTypeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserTypeModel.Id = _UserTypeService.GetlastCode();
                    var UserType = _mapper.Map<UserType>(UserTypeModel);
                    _UserTypeService.Insert(UserType);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: UserTypeController/Edit/5
        public ActionResult Edit(int id)
        {
            UserType UserType = _UserTypeService.Get(id);
            UserTypeModel UserTypeModel = _mapper.Map<UserTypeModel>(UserType);
            return View(UserTypeModel);
        }

        // POST: UserTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserTypeModel UserTypeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UserType = _mapper.Map<UserType>(UserTypeModel);
                    _UserTypeService.Edit(UserType);

                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserTypeController/Delete/5
        public ActionResult Delete(int id)
        {
            UserType UserType = _UserTypeService.Get(id);
            UserTypeModel UserTypeModel = _mapper.Map<UserTypeModel>(UserType);
            return View(UserTypeModel);
        }

        // POST: UserTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, UserTypeModel UserTypeModel)
        {
            try
            {
                _UserTypeService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
