using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        ICompanyService _companyService;
        ICompanyTypeService _companyTypeService;
        IUserService _userService;
        ICompanyAlertContactService _companyAlertContactService;
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
                                  ICompanyAlertContactService companyAlertContactService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer)
        {
            _companyService = companyService;
            _companyTypeService = companyTypeService;
            _companyAlertContactService = companyAlertContactService;
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

            listaCompany = FilterCompaniesByUser(user, listaCompany);

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
            CompanyModel companyModel = new CompanyModel()
            {
                CompanyType = GetCompanyType(),
            };

            return View(companyModel);
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
                    var user = _userService.Get(Convert.ToInt32(LoggedUserId));

                    companyModel.Id = _companyService.GetlastCode();

                    var company = _mapper.Map<Company>(companyModel);
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

            if (CompanyModel != null)
                CompanyModel.CompanyType = GetCompanyType();

            var userId = LoggedUserId;
            var user = _userService.Get(Convert.ToInt32(userId));

            if (Company.CompanyTypeId == 2)
            {
                CompanyModel.CompanyType.Clear();
                CompanyModel.CompanyType.Add(new SelectListItemDTO() { Key = 2, Value = "Distribuidor" });
            }
            else if (Company.CompanyTypeId == 1)
            {
                CompanyModel.CompanyType.Clear();
                CompanyModel.CompanyType.Add(new SelectListItemDTO() { Key = 1, Value = "Administrador" });
            }

            ViewBag.Contacts = _companyAlertContactService.GetByCompany(id).ToList();

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
                    var user = _userService.Get(Convert.ToInt32(LoggedUserId));

                    var company = _mapper.Map<Company>(CompanyModel);
                    company.ParentCompanyId = user.Contact.CompanyId;

                    _companyService.Edit(company);
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
                ViewBag.ErrorMsg = _companyService.GetRelatedLocks(id);

                return View(CompanyModel);
            }
        }

        private List<SelectListItemDTO> GetCompanyType()
        {
            var user = _userService.Get(Convert.ToInt32(LoggedUserId));

            var list = new List<SelectListItemDTO>();

            if (user.UserType.Name == Constants.Roles.Administrator)
            {
                list.Add(new SelectListItemDTO() { Key = 2, Value = "Distribuidor" });
                list.Add(new SelectListItemDTO() { Key = 3, Value = "Consumidor Final" });
            }
            else if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                list.Add(new SelectListItemDTO() { Key = 3, Value = "Consumidor Final" });
            }

            return list;
        }

        [HttpPost]
        public JsonResult InsertCompanyContact(int cId, string name, string email)
        {
            var contact = new CompanyAlertContact()
            {
                CompanyId = cId, Name = name, Email = email
            };

            _companyAlertContactService.Insert(contact);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteCompanyContact(int id)
        {
            _companyAlertContactService.Remove(id);

            return Json(new { success = true });
        }
    }
}
