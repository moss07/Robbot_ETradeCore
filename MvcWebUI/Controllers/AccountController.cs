using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebUI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcWebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public AccountController(IAccountService accountService, ICountryService countryService, ICityService cityService)
        {
            _accountService = accountService;
            _countryService = countryService;
            _cityService = cityService;
        }

        public IActionResult Register()
        {
            ViewBag.Countries=new SelectList(_countryService.GetQuery().ToList(),"Id","Name");
            var model = new UserRegisterModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                model.Active = AppSettings.NewUserActive;
                var accountResult = _accountService.Register(model);
                if (accountResult.Status ==ResultStatus.Exception)
                    throw new Exception(accountResult.Message);
                if (accountResult.Status == ResultStatus.Success)
                    return RedirectToAction("Login");
                ModelState.AddModelError("", accountResult.Message);
            }
            ViewBag.Countries = new SelectList(_countryService.GetQuery().ToList(), "Id", "Name",model.UserDetail.CountryId);
            var citiesResult = _cityService.GetCities(c => c.CountryId == model.UserDetail.CountryId);
            if(citiesResult.Status==ResultStatus.Exception)
                throw new Exception(citiesResult.Message);
            ViewBag.Cities = new SelectList(citiesResult.Data, "Id", "Name", model.UserDetail.CityId);
            return View(model);
        }

        public IActionResult Login()
        {
            var model = new UserLoginModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Login(model);
                if(result.Status==ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Success)
                {
                    var user = result.Data;
                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role,user.RoleName),
                    new Claim(ClaimTypes.Sid,user.Id.ToString())
                };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
