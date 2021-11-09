using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using DataAccess.Repositories.Bases;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Business.Services
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepositoryBase _countryRepository;

        public CountryService(CountryRepositoryBase countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public IQueryable<CountryModel> GetQuery(Expression<Func<CountryModel, bool>> predicate = null)
        {
            var query = _countryRepository.Query().OrderBy(c=>c.Name).Select(c => new CountryModel()
            {
                Id = c.Id,
                Guid = c.Guid,
                Name = c.Name
            });
            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }

        public Result Add(CountryModel model)
        {
            try
            {
                if (_countryRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("Country with the same name exists!");
                var entity = new Country()
                {
                    Name = model.Name.Trim()
                };
                _countryRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
        public Result Update(CountryModel model)
        {
            try
            {
                if (_countryRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim() && c.Id!=model.Id))
                    return new ErrorResult("Country with the same name exists!");
                var entity = new Country()
                {
                    Id=model.Id,
                    Name = model.Name.Trim()
                };
                _countryRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result Delete(int id)
        {
            try
            {
                var country = _countryRepository.Query("Cities", "UserDetails").SingleOrDefault(c => c.Id == id);
                if (country == null)
                    return new ErrorResult("Country not found!");
                if (country.Cities != null && country.Cities.Count > 0)
                    return new ErrorResult("While country has cities can't be deleted!");
                if (country.UserDetails != null && country.UserDetails.Count > 0)
                    return new ErrorResult("While country has users can't be deleted!");
                _countryRepository.Delete(country);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _countryRepository?.Dispose();
        }

        public Result<List<CountryModel>> GetCountries()
        {
            try
            {
                var countries = GetQuery().ToList();
                return new SuccessResult<List<CountryModel>>(countries);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<CountryModel>>(exc);
            }
        }
    }
}
