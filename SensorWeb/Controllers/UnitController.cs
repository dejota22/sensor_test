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
using System.Xml.Linq;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace SensorWeb.Controllers
{
    [Authorize(Roles = Constants.Roles.Administrator + "," + Constants.Roles.Supervisor)]
    public class UnitController : BaseController
    {
        ICompanyService _companyService;
        ICompanyUnitService _companyUnitService;
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
        public UnitController(ICompanyService companyService,
                                  ICompanyUnitService companyUnitService,
                                  IUserService userService,
                                  ICompanyAlertContactService companyAlertContactService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer)
        {
            _companyService = companyService;
            _companyUnitService = companyUnitService;
            _companyAlertContactService = companyAlertContactService;
            _userService = userService;
            _mapper = mapper;
            _localizer = localizer;
        }

        // GET: UnitController
        public ActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            var listaCompanyUnit = _companyUnitService.GetAll();

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                listaCompanyUnit = listaCompanyUnit.Where(x => x.Company.ParentCompanyId == user.Contact.CompanyId);
            }

            var listaCompanyModel = _mapper.Map<List<CompanyUnitModel>>(listaCompanyUnit);

            return View(listaCompanyModel.OrderBy(x => x.Id));
        }

        // GET: UnitController/Details/5
        public ActionResult Details(int id)
        {
            CompanyUnit CompanyUnit = _companyUnitService.Get(id);
            CompanyUnitModel CompanyUnitModel = _mapper.Map<CompanyUnitModel>(CompanyUnit);
            return View(CompanyUnitModel);
        }

        // GET: UnitController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyUnitModel companyUnitModel)
        {
            try
            {
                if (companyUnitModel.CompanyId != 0 && !string.IsNullOrEmpty(companyUnitModel.Name))
                {
                    var unit = _mapper.Map<CompanyUnit>(companyUnitModel);
                    _companyUnitService.Insert(unit);
                }
                else
                    return View(companyUnitModel);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: UnitController/Edit/5
        public ActionResult Edit(int id)
        {
            CompanyUnit companyUnit = _companyUnitService.Get(id);
            CompanyUnitModel companyUnitModel = _mapper.Map<CompanyUnitModel>(companyUnit);

            //ViewBag.Contacts = _companyAlertContactService.GetByCompany(id).ToList();

            return View(companyUnitModel);
        }

        // POST: UnitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CompanyUnitModel CompanyUnitModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var companyUnit = _companyUnitService.Get(id);
                    companyUnit.CompanyId = CompanyUnitModel.CompanyId;
                    companyUnit.Name = CompanyUnitModel.Name;
                    companyUnit.Company = _companyService.Get(companyUnit.Id);

                    _companyUnitService.Edit(companyUnit);
                }

                CompanyUnit companyUnitView = _companyUnitService.Get(id);
                CompanyUnitModel companyUnitModel = _mapper.Map<CompanyUnitModel>(companyUnitView);

                return View(companyUnitModel);
            }
            catch
            {
                return View();
            }
        }

        // GET: UnitController/Delete/5
        public ActionResult Delete(int id)
        {
            CompanyUnit companyUnit = _companyUnitService.Get(id);
            CompanyUnitModel companyUnitModel = _mapper.Map<CompanyUnitModel>(companyUnit);
            return View(companyUnitModel);
        }

        // POST: UnitController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CompanyUnitModel CompanyUnitModel)
        {
            try
            {
                var hasSectors = _companyUnitService.Get(id).CompanyUnitSector.Any();

                if (hasSectors)
                {
                    ViewBag.ErrorMsg = "Não foi possível excluir. Esta Unidade possui Setores vinculados.";
                    return View(CompanyUnitModel);
                }

                _companyUnitService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult InsertSector(int unitId, string name, int? parentSector)
        {

            var hasNamed = _companyUnitService.GetSectorByName(name)
                .Where(s => s.ParentSectorId == parentSector).Any();

            if (hasNamed)
                return Json(new { success = false, msg = "Não é possível cadastrar. Já existe um setor cadastrado com o mesmo nome." });

            var sector = new CompanyUnitSector()
            {
                CompanyUnitId = unitId, Name = name, ParentSectorId = parentSector
            };

            _companyUnitService.InsertSector(sector);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteSector(int id)
        {
            _companyUnitService.RemoveSector(id);

            return Json(new { success = true });
        }
    }
}
