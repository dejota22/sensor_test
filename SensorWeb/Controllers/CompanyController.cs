using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SensorWeb.Controllers
{
    [Authorize(Roles = Constants.Roles.Administrator + "," + Constants.Roles.Supervisor)]
    public class CompanyController : BaseController
    {
        ICompanyService _companyService;
        ICompanyTypeService _companyTypeService;
        IUserService _userService;
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="CompanyService"></param>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public CompanyController(ICompanyService companyService,
                                  ICompanyTypeService companyTypeService,
                                  IUserService userService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer)
        {
            _companyService = companyService;
            _companyTypeService = companyTypeService;
            _userService = userService;
            _mapper = mapper;
            _localizer = localizer;
        }

        // GET: CompanyController
        public ActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            var listaCompany = _companyService.GetAll();

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                listaCompany = listaCompany.Where(x => x.ParentCompanyId == user.Contact.CompanyId);
            }

            var listaCompanyModel = _mapper.Map<List<CompanyModel>>(listaCompany);

            return View(listaCompanyModel.OrderBy(x => x.Id));
        }

        // GET: CompanyController/Details/5
        public ActionResult Details(int id)
        {
            Company Company = _companyService.Get(id);
            CompanyModel CompanyModel = _mapper.Map<CompanyModel>(Company);
            return View(CompanyModel);
        }

        // GET: CompanyController/Create
        public ActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            var list = new List<SelectListItemDTO>();

            if (user.UserType.Name == Constants.Roles.Administrator)
            {
                list.Add(new SelectListItemDTO() { Key = 2, Value = "Distribuidor" });
                list.Add(new SelectListItemDTO() { Key = 3, Value = "Consumidor Final" });
            }
            else if (user.UserType.Name == Constants.Roles.Supervisor)
            {
                list.Add(new SelectListItemDTO() { Key = 3, Value = "Consumidor Final" });
            }

            CompanyModel company = new CompanyModel()
            {
                Id = _companyService.GetlastCode(),
                CompanyType = list,          
            };

            return View(company);
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyModel companyModel, List<SelectListItem> lstUsers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    companyModel.Id = _companyService.GetlastCode();
                    var company = _mapper.Map<Company>(companyModel);

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = _userService.Get(Convert.ToInt32(userId));

                    company.ParentCompanyId = user.Contact.CompanyId;
                    _companyService.Insert(company);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: CompanyController/Edit/5
        public ActionResult Edit(int id)
        {
            Company Company = _companyService.Get(id);
            CompanyModel CompanyModel = _mapper.Map<CompanyModel>(Company);
            return View(CompanyModel);
        }

        // POST: CompanyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CompanyModel CompanyModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Company = _mapper.Map<Company>(CompanyModel);
                    _companyService.Edit(Company);

                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompanyController/Delete/5
        public ActionResult Delete(int id)
        {
            Company Company = _companyService.Get(id);
            CompanyModel CompanyModel = _mapper.Map<CompanyModel>(Company);
            return View(CompanyModel);
        }

        // POST: CompanyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CompanyModel CompanyModel)
        {
            try
            {
                _companyService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
