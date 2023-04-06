using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        ICompanyService _companyService;
        ICompanyTypeService _companyTypeService;
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
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer)
        {
            _companyService = companyService;
            _companyTypeService = companyTypeService;
            _mapper = mapper;
            _localizer = localizer;
        }

        // GET: CompanyController
        public ActionResult Index()
        {
            var listaCompany = _companyService.GetAll();
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
            CompanyModel user = new CompanyModel()
            {
                Id = _companyService.GetlastCode()
            };

            return View(user);
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyModel CompanyModel, List<SelectListItem> lstUsers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyModel.Id = _companyService.GetlastCode();
                    var Company = _mapper.Map<Company>(CompanyModel);
                    _companyService.Insert(Company);
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
