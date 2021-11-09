using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.EntityFramework.Contexts;
using Business.Services.Bases;
using Business.Models;
using AppCore.Business.Models.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using MvcWebUI.Settings;

namespace MvcWebUI.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: Products
        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _productService.GetQuery().ToList();
            return View(model);
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin,User")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var product = _productService.GetQuery().SingleOrDefault(p => p.Id == id.Value);
            if (id == null)
            {
                return View("NotFound");
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryService.GetQuery().ToList(), "Id", "Name");
            var model = new ProductModel();
            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductModel product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                string fileExtension = null;
                string filePath = null;
                bool saveFile = false;
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName;
                    fileExtension = Path.GetExtension(fileName);
                    string[] appSettingsExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedFileExtension = false;
                    foreach (string appSettingsExtension in appSettingsExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsExtension.ToLower().Trim())
                        {
                            acceptedFileExtension = true;
                            break;
                        }
                    }
                    if (!acceptedFileExtension)
                    {
                        ModelState.AddModelError("", "The accepted image extensions are " + AppSettings.AcceptedImageMaximumLength);
                        return View(product);
                    }
                    var acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2);
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "The maximum size of an image must be " + AppSettings.AcceptedImageMaximumLength + " MB");
                        return View(product);
                    }
                    saveFile = true;
                }
                if (saveFile)
                {
                    fileName = Guid.NewGuid() + fileExtension;
                    filePath = Path.Combine("wwwroot", "files", "products", fileName);
                }
                product.ImagePath = fileName;
                var result = _productService.Add(product);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Success)
                {
                    if (saveFile)
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.Categories = new SelectList(_categoryService.GetQuery().ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }
        public IActionResult TestException()
        {
            throw new Exception("Test Exception!");
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Not Found");
            }

            var productResult = _productService.GetProduct(id.Value);
            if (productResult.Status == ResultStatus.Exception)
            {
                throw new Exception(productResult.Message);
            }
            if (productResult.Status == ResultStatus.Error)
                ModelState.AddModelError("", productResult.Message);
            ViewBag.Categories = new SelectList(_categoryService.GetQuery().ToList(), "Id", "Name", productResult.Data.CategoryId);
            return View(productResult.Data);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductModel product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                string fileExtension = null;
                string filePath = null;
                bool saveFile = false;
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName;
                    fileExtension = Path.GetExtension(fileName);
                    string[] appSettingsExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedFileExtension = false;
                    foreach (string appSettingsExtension in appSettingsExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsExtension.ToLower().Trim())
                        {
                            acceptedFileExtension = true;
                            break;
                        }
                    }
                    if (!acceptedFileExtension)
                    {
                        ModelState.AddModelError("", "The accepted image extensions are " + AppSettings.AcceptedImageMaximumLength);
                        return View(product);
                    }
                    var acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2);
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "The maximum size of an image must be " + AppSettings.AcceptedImageMaximumLength + " MB");
                        return View(product);
                    }
                    saveFile = true;
                }

                var existingProduct = _productService.GetQuery(p => p.Id == product.Id).SingleOrDefault();
                if (string.IsNullOrWhiteSpace(existingProduct.ImagePath) && saveFile)
                    fileName = Guid.NewGuid() + fileExtension;
                else
                    fileName = existingProduct.ImagePath;

                product.ImagePath = fileName;

                var productResult = _productService.Update(product);
                if (productResult.Status == ResultStatus.Exception)
                    throw new Exception(productResult.Message);
                if (productResult.Status == ResultStatus.Success)
                {
                    if (saveFile)
                    {
                        filePath = Path.Combine("wwwroot", "files", "products", fileName);
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", productResult.Message);
            }
            ViewBag.Categories = new SelectList(_categoryService.GetQuery().ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return View("Not Found");
                }
                var getResult = _productService.GetProduct(id.Value);
                if (getResult.Status == ResultStatus.Exception)
                    throw new Exception(getResult.Message);

                var product = getResult.Data;

                var deleteResult = _productService.Delete(id.Value);
                if (deleteResult.Status == ResultStatus.Exception)
                    throw new Exception(deleteResult.Message);

                if (!string.IsNullOrWhiteSpace(product.ImagePath))
                {
                    string filePath = Path.Combine("wwwroot", "files", "products", product.ImagePath);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult DeleteProductImage(int? id)
        {
            if (id == null)
                return View("NotFound");

            var productResult = _productService.GetProduct(id.Value);
            if (productResult.Status == ResultStatus.Exception)
                throw new Exception(productResult.Message);

            var product = productResult.Data;
            if (!string.IsNullOrWhiteSpace(product.ImagePath))
            {
                string filePath = Path.Combine("wwwroot", "files", "products", product.ImagePath);
                product.ImagePath = null;
                var updateResult = _productService.Update(product);
                if (updateResult.Status == ResultStatus.Exception)
                    throw new Exception(updateResult.Message);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            ViewBag.Categories = new SelectList(_categoryService.GetQuery().ToList(), "Id", "Name", product.CategoryId);
            return View("Details", product);
        }
    }
}
