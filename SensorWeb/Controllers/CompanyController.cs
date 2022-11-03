using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SensorWeb.Controllers
{
    public class CompanyController : BaseController
    {
        ICompanyService _CompanyService;
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="CompanyService"></param>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public CompanyController(ICompanyService CompanyService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer)
        {
            _CompanyService = CompanyService;
            _mapper = mapper;
            _localizer = localizer;
        }

        // GET: CompanyController
        public ActionResult Index()
        {
            var listaUsuarios = _CompanyService.GetAll();
            var listaCompanyModel = _mapper.Map<List<CompanyModel>>(listaUsuarios);

            //foreach (var CompanyModel in listaCompanyModel)
            //{
            //    CompanyModel.Contact = _mapper.Map<List<ContactModel>>(_CompanyService.GetAll())
            //}

            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
          //  ViewData["Title"] = _localizer["CompanyTittle"];
            return View(listaCompanyModel.OrderBy(x => x.Id));
        }

        // GET: CompanyController/Details/5
        public ActionResult Details(int id)
        {
            Company Company = _CompanyService.Get(id);
            CompanyModel CompanyModel = _mapper.Map<CompanyModel>(Company);
            return View(CompanyModel);
        }

        // GET: CompanyController/Create
        public ActionResult Create()
        {
            CompanyModel user = new CompanyModel()
            {
                Id = _CompanyService.GetlastCode()
            };

            return View(user);
        }



        public List<SelectListItem> Fruits { get; set; }
        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyModel CompanyModel, List<SelectListItem> lstUsers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //#region multiselect

                    //var fruits = (from fruit in CompanyModel.lstCompanySub
                    //    select new SelectListItem
                    //    {
                    //        Text = fruit.Text,
                    //        Value = fruit.Value.ToString()
                    //    }).ToList();
                    //this.Fruits = fruits;

                    //string[] fruitIds = Request.Form["lstFruits"].ToString().Split(",");
                    //foreach (string id in fruitIds)
                    //{
                    //    //if (!string.IsNullOrEmpty(id))
                    //    //{
                    //    //    string name = this.Fruits.Where(x => x.Value == id).FirstOrDefault().Text;
                    //    //    this.Message += "Id: " + id + "  Fruit Name: " + name + "\\n";
                    //    //}
                    //}

                    //#endregion

                    CompanyModel.Id = _CompanyService.GetlastCode();
                    var Company = _mapper.Map<Company>(CompanyModel);
                    _CompanyService.Insert(Company);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: CompanyController/Edit/5
        public ActionResult Edit(int id)
        {
            Company Company = _CompanyService.Get(id);
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
                    _CompanyService.Edit(Company);

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
            Company Company = _CompanyService.Get(id);
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
                _CompanyService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
