using AppCore.Business.Models.Results;
using Business.Services.Bases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWebUI.Controllers
{
    [Route("Cities")]
    public class CitiesJsonController : Controller
    {
        private readonly ICityService _cityService;

        public CitiesJsonController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [Route("CitiesGet/{countryId?}")]
        public IActionResult GetCitiesByCountryId(int? countryId)
        {
            if (countryId == null)
                return View("NotFound");
            var result = _cityService.GetCities(c => c.CountryId == countryId.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            return Json(result.Data);
        }

        [Route("CitiesPost/{countryId?}")]
        [HttpPost]
        public IActionResult GetCitiesByCountryIdWithPost(int? countryId)
        {
            if (countryId == null)
                return View("NotFound");
            var result = _cityService.GetCities(c => c.CountryId == countryId.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            return Json(result.Data);
        }
    }
}
