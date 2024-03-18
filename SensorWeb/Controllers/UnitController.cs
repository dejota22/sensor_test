using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using SensorService;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Xml.Linq;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace SensorWeb.Controllers
{
    [Authorize(Roles = Constants.Roles.Administrator + "," + Constants.Roles.Supervisor + "," + Constants.Roles.Sysadmin + "," + Constants.Roles.User)]
    public class UnitController : BaseController
    {
        ICompanyService _companyService;
        ICompanyUnitService _companyUnitService;
        IDeviceService _deviceService;
        IUserService _userService;
        ICompanyAlertContactService _companyAlertContactService;
        IMotorService _motorService;
        IReceiveService _receiveService;
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
                                  IDeviceService deviceService,
                                  IUserService userService,
                                  ICompanyAlertContactService companyAlertContactService,
                                  IMotorService motorService,
                                  IReceiveService receiveService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer)
        {
            _companyService = companyService;
            _companyUnitService = companyUnitService;
            _companyAlertContactService = companyAlertContactService;
            _deviceService = deviceService;
            _userService = userService;
            _motorService = motorService;
            _receiveService = receiveService;
            _mapper = mapper;
            _localizer = localizer;
        }

        // GET: UnitController
        public ActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));
            var companies = _companyService.GetAll();
            var supContacts = _userService.GetAll().Where(u => u.UserTypeId == 3 || u.UserTypeId == 4)
                    .Select(u => u.Contact);
            var listaCompanyUnit = _companyUnitService.GetAll();

            listaCompanyUnit = FilterUnitsByUser(user, listaCompanyUnit, companies, supContacts);

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
                    companyUnit.Company = _companyService.Get(CompanyUnitModel.CompanyId);

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
                .Where(s => s.CompanyUnitId == unitId && s.ParentSectorId == parentSector).Any();

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

        [HttpPost]
        public JsonResult GetSectors(int uId)
        {
            var unit = _companyUnitService.Get(uId);
            var mainsectors = new List<CompanyUnitSector>();

            if (unit?.CompanyUnitSector != null)
                mainsectors = unit.CompanyUnitSector.Where(s => s.ParentSectorId == null)
                    .Select(s => new CompanyUnitSector() { Id = s.Id, Name = s.Name }).ToList();

            return Json(new { secs = mainsectors });
        }

        [HttpPost]
        public JsonResult GetSubSectors(int sId)
        {
            var sector = _companyUnitService.GetSector(sId);
            var subsectors = new List<CompanyUnitSector>();

            if (sector.SubSectors != null)
                subsectors = sector.SubSectors
                    .Select(s => new CompanyUnitSector() { Id = s.Id, Name = s.Name }).ToList();

            return Json(new { subs = subsectors });
        }
    }
}
