using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class ContactController : BaseController
    {
        IContactService _ContactService;
        IMapper _mapper;

        public ContactController(IContactService ContactService, IMapper mapper)
        {
            _ContactService = ContactService;
            _mapper = mapper;
        }

        // GET: ContactController
        public ActionResult Index()
        {
            var listContacts = _ContactService.GetAll();
            var listaContactModel = _mapper.Map<List<ContactModel>>(listContacts);

            //foreach (var ContactModel in listaContactModel)
            //{
            //    ContactModel.Contact = _mapper.Map<List<ContactModel>>(_ContactService.GetAll())
            //}

            return View(listaContactModel.OrderBy(x => x.Id));
        }

        // GET: ContactController/Details/5
        public ActionResult Details(int id)
        {
            Contact Contact = _ContactService.Get(id);
            ContactModel ContactModel = _mapper.Map<ContactModel>(Contact);
            return View(ContactModel);
        }

        // GET: ContactController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactModel ContactModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Contact = _mapper.Map<Contact>(ContactModel);
                    _ContactService.Insert(Contact);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: ContactController/Edit/5
        public ActionResult Edit(int id)
        {
            Contact Contact = _ContactService.Get(id);
            ContactModel ContactModel = _mapper.Map<ContactModel>(Contact);
            return View(ContactModel);
        }

        // POST: ContactController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ContactModel ContactModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Contact = _mapper.Map<Contact>(ContactModel);
                    _ContactService.Edit(Contact);

                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactController/Delete/5
        public ActionResult Delete(int id)
        {
            Contact Contact = _ContactService.Get(id);
            ContactModel ContactModel = _mapper.Map<ContactModel>(Contact);
            return View(ContactModel);
        }

        // POST: ContactController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ContactModel ContactModel)
        {
            try
            {
                _ContactService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
