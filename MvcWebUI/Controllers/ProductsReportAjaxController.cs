using AppCore.Business.Models.Paging;
using AppCore.Business.Models.Results;
using AppCore.Business.Ordering;
using Business.Models.Filters;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebUI.Models;
using MvcWebUI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsReportAjaxController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsReportAjaxController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index(int? categoryId)
        {
            var filter = new ProductFilterModel();
            var page = new PageModel()
            {
                RecordsPerPageCount = AppSettings.RecordsPerPageCount
            };

            var order = new OrderModel();

            var result = _productService.GetReport(filter,page, order);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);

            double recordsCount = page.RecordsCount;
            double recordsPerPageCount = AppSettings.RecordsPerPageCount;
            double totalPageCount = Math.Ceiling(recordsCount / recordsPerPageCount);
            List<int> pageNumbers = new List<int>();
            if (totalPageCount == 0)
            {
                pageNumbers.Add(1);
            }
            else
            {
                for (int pageNumber = 1; pageNumber <= totalPageCount; pageNumber++)
                {
                    pageNumbers.Add(pageNumber);
                }
            }

            List<SelectListItem> pageNumberSelectListNumbers = pageNumbers.Select(pn => new SelectListItem()
            {
                Value = pn.ToString(),
                Text = pn.ToString()
            }).ToList();

            var viewModel = new ProductReportViewModel()
            {
                Products = result.Data,
                Filter = filter,
                PageNumbers = new SelectList(pageNumberSelectListNumbers, "Value", "Text",page.PageNumber),
                Categories = new SelectList(_categoryService.GetQuery().ToList(), "Id", "Name") //* 1
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(ProductReportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var page = new PageModel()
                {
                    PageNumber = viewModel.PageNumber,
                    RecordsPerPageCount = AppSettings.RecordsPerPageCount
                };

                var order = new OrderModel()
                {
                    Expression = viewModel.OrderByExpression,
                    DirectionAscending = viewModel.OrderByDirectionAscending
                };

                var result = _productService.GetReport(viewModel.Filter,page, order);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);

                viewModel.Products = result.Data;

                double recordsCount = page.RecordsCount;
                double recordsPerPageCount = AppSettings.RecordsPerPageCount;
                double totalPageCount = Math.Ceiling(recordsCount / recordsPerPageCount);
                List<int> pageNumbers = new List<int>();
                if (totalPageCount == 0)
                {
                    pageNumbers.Add(1);
                }
                else
                {
                    for (int pageNumber = 1; pageNumber <= totalPageCount; pageNumber++)
                    {
                        pageNumbers.Add(pageNumber);
                    }
                }

                List<SelectListItem> pageNumberSelectListNumbers = pageNumbers.Select(pn => new SelectListItem()
                {
                    Value = pn.ToString(),
                    Text = pn.ToString()
                }).ToList();
                viewModel.PageNumbers = new SelectList(pageNumberSelectListNumbers, "Value", "Text", viewModel.PageNumber);

            }
            return PartialView("_Products", viewModel);
        }
    }
}
