using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.EntityFramework.Contexts;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Business.Services.Bases;
using Business.Models;
using AppCore.Business.Models.Results;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: Roles
        public IActionResult Index()
        {
            var model = _roleService.GetQuery().ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var query = _roleService.GetQuery();

            var role = query.SingleOrDefault(r => r.Id == id.Value);

            if (role == null)
            {
                return View("NotFound");
            }
            return View(role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            var model = new RoleModel();
            return View(model);
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleModel role)
        {
            if (ModelState.IsValid)
            {
                var result= _roleService.Add(role);
                if(result.Status==ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(role);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var role = _roleService.GetQuery().SingleOrDefault(r=>r.Id==id.Value);
            if (role == null)
            {
                return View("NotFound");
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleModel role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Update(role);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(role);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // POST: Roles/Delete
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var deleteResult = _roleService.Delete(id);
            if (deleteResult.Status == ResultStatus.Exception)
                deleteResult.Message = "An exception occured while deleting the role!";
            return Json(deleteResult.Message);
        }
    }
}
