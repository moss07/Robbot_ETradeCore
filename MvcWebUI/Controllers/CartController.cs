using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult AddToCart(int? productId)
        {
            if (productId == null)
                return View("NotFound");
            var productResult = _productService.GetProduct(productId.Value);
            if (productResult.Status == ResultStatus.Exception)
                throw new Exception(productResult.Message);
            if (productResult.Status == ResultStatus.Error)
            {
                TempData["Message"] = productResult.Message;
                return RedirectToAction("Index", "Products");
            }
            List<CartModel> list = new List<CartModel>();
            CartModel item;
            string cartJson;

            string userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            if (HttpContext.Session.GetString("cart") != null)
            {
                cartJson = HttpContext.Session.GetString("cart");
                list = JsonConvert.DeserializeObject<List<CartModel>>(cartJson);
            }
            item = new CartModel()
            {
                ProductId = productId.Value,
                ProductName = productResult.Data.Name,
                UnitPrice = productResult.Data.UnitPrice
            };
            list.Add(item);
            cartJson = JsonConvert.SerializeObject(list);
            HttpContext.Session.SetString("cart", cartJson);

            TempData["Message"] = productResult.Data.Name + " added to cart.";
            return RedirectToAction("Index", "Products");
        }

        public IActionResult Index()
        {
            List<CartModel> list = new List<CartModel>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                list = JsonConvert.DeserializeObject<List<CartModel>>(HttpContext.Session.GetString("cart"));
            }

            var cartGroupBy = (from c in list
                               orderby c.ProductName
                               group c by new { c.ProductName, c.ProductId, c.UserId }
                                into cGroupBy
                               select new CartGroupByModel()
                               {
                                   UserId = cGroupBy.Key.UserId,
                                   ProductId = cGroupBy.Key.ProductId,
                                   ProductName = cGroupBy.Key.ProductName,
                                   TotalProductCount = cGroupBy.Count(),
                                   TotalUnitPriceText = cGroupBy.Sum(cg => cg.UnitPrice).ToString(new CultureInfo("en"))
                               }).ToList();

            return View(cartGroupBy);
        }

        public IActionResult RemoveFromCart(int? productId, int? userId)
        {
            if (productId == null || userId == null)
                return View("NotFound");
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<CartModel> list = JsonConvert.DeserializeObject<List<CartModel>>(HttpContext.Session.GetString("cart"));
                CartModel item = list.FirstOrDefault(l => l.ProductId == productId.Value && l.UserId == userId.Value);
                if (item != null)
                {
                    list.Remove(item);
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(list));
                    TempData["Message"] = item.ProductName + " removed from cart.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("cart");
            TempData["Message"] = "Cart cleared.";
            return RedirectToAction(nameof(Index));
        }
    }
}
