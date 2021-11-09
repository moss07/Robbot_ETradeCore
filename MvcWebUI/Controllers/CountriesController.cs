using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.EntityFramework.Contexts;
using Entities.Entities;
using Business.Services.Bases;
using Business.Models;
using AppCore.Business.Models.Results;

namespace MvcWebUI.Controllers
{

    public class CountriesController : Controller
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        // GET: Countries
        public IActionResult Index()
        {
            var result = _countryService.GetQuery().ToList();
            return View(result);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            var model = new CountryModel();
            return View(model);
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var result = _countryService.Add(country);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(country);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var country = _countryService.GetQuery().SingleOrDefault(c => c.Id == id.Value);
            if (country == null)
            {
                return View("NotFound");
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var result = _countryService.Update(country);
                if (result.Status ==ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(country);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var deleteResult = _countryService.Delete(id);
            if (deleteResult.Status == ResultStatus.Exception)
                throw new Exception(deleteResult.Message);
            if (deleteResult.Status == ResultStatus.Error)
            {
                ModelState.AddModelError("", deleteResult.Message);
                var getResult = _countryService.GetQuery().SingleOrDefault(c => c.Id == id);
                return View("Edit",getResult);
            }
            return RedirectToAction(nameof(Index));
            
        }
    }
}
