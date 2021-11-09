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
    public class CityService : ICityService
    {
        private readonly CityRepositoryBase _cityRepository;

        public CityService(CityRepositoryBase cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public IQueryable<CityModel> GetQuery(Expression<Func<CityModel, bool>> predicate = null)
        {
            var query= _cityRepository.Query("Country").OrderBy(c => c.Name).Select(c => new CityModel()
            {
                CountryId = c.CountryId,
                Guid = c.Guid,
                Id = c.Id,
                Name = c.Name,
                Country=new CountryModel()
                {
                    Id=c.Country.Id,
                    Name=c.Country.Name,
                    Guid=c.Country.Guid
                }
            });
            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }
        public Result<List<CityModel>> GetCities(Expression<Func<CityModel, bool>> predicate = null)
        {
            try
            {
                var query = GetQuery(predicate);
                return new SuccessResult<List<CityModel>>(query.ToList());
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<CityModel>>(exc);
            }
        }

        public Result Add(CityModel model)
        {
            try
            {
                if (_cityRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("City with the same name exists!");
                var entity = new City()
                {
                    CountryId = model.CountryId,
                    Name = model.Name.Trim()
                };
                _cityRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
        public Result Update(CityModel model)
        {
            try
            {
                if (_cityRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim() && c.Id!=model.Id))
                    return new ErrorResult("City with the same name exists!");
                var entity = new City()
                {
                    Id=model.Id,
                    CountryId = model.CountryId,
                    Name = model.Name.Trim()
                };
                _cityRepository.Update(entity);
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
                var city = _cityRepository.Query("UserDetails").SingleOrDefault(c => c.Id == id);
                if (city == null)
                    return new ErrorResult("City not found!");
                if (city.UserDetails != null && city.UserDetails.Count > 0)
                    return new ErrorResult("While city has users can't be deleted!");
                
                _cityRepository.Delete(city);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _cityRepository?.Dispose();
        }

        public Result<CityModel> GetCity(int id)
        {
            try
            {
                var city = GetQuery().SingleOrDefault(c=>c.Id==id);
                if (city == null)
                    return new ErrorResult<CityModel>("City not found!");
                return new SuccessResult<CityModel>(city);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<CityModel>(exc);
            }
        }
    }
}
