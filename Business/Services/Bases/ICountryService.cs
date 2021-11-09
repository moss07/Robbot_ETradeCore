using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Services.Bases
{
    public interface ICountryService : IService<CountryModel>
    {
        Result<List<CountryModel>> GetCountries();
    }
}
