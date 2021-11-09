using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using DataAccess.Repositories.Bases;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepositoryBase _userRepository;
        private readonly CountryRepositoryBase _countryRepository;
        private readonly CityRepositoryBase _cityRepository;

        public UserService(UserRepositoryBase userRepository, CountryRepositoryBase countryRepository, CityRepositoryBase cityRepository)
        {
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
        }

        public IQueryable<UserModel> GetQuery(Expression<Func<UserModel, bool>> predicate = null)
        {
            var userQuery = _userRepository.Query("UserDetail", "Role");
            var countryQuery = _countryRepository.Query();
            var cityQuery = _cityRepository.Query();

            var query = from user in userQuery
                        join country in countryQuery
                        on user.UserDetail.CountryId equals country.Id
                        join city in cityQuery
                        on user.UserDetail.CityId equals city.Id
                        select new UserModel()
                        {
                            Id = user.Id,
                            Password = user.Password,
                            Guid = user.Guid,
                            Active = user.Active,
                            ActiveText = user.Active ? "Yes" : "No",
                            Role = new RoleModel()
                            {
                                Id = user.Role.Id,
                                Name = user.Role.Name,
                                Guid = user.Role.Guid
                            },
                            RoleId = user.RoleId,
                            UserDetail = new UserDetailModel()
                            {
                                Id = user.UserDetail.Id,
                                EMail = user.UserDetail.EMail,
                                CountryId = user.UserDetail.CountryId,
                                CityId = user.UserDetail.CityId,
                                Guid = user.UserDetail.Guid,
                                Country = new CountryModel()
                                {
                                    Id = country.Id,
                                    Name = country.Name,
                                    Guid = country.Guid
                                },
                                City = new CityModel()
                                {
                                    Id = city.Id,
                                    Name = city.Name,
                                    CountryId = city.CountryId,
                                    Guid = city.Guid
                                }
                            },
                            UserDetailId = user.UserDetailId,
                            UserName = user.UserName
                        };
            return query;
        }

        public Result Add(UserModel model)
        {
            try
            {
                if (_userRepository.Query().Any(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim()))
                    return new ErrorResult("User with the same user name exists!");
                if (_userRepository.Query("UserDetail").Any(u => u.UserDetail.EMail.ToUpper() == model.UserDetail.EMail.ToUpper().Trim()))
                    return new ErrorResult("User with the same e-mail exists!");
                var entity = new User()
                {
                    Active = model.Active,
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    RoleId = model.RoleId,
                    UserDetail = new UserDetail()
                    {
                        Address = model.UserDetail.Address.Trim(),
                        CityId = model.UserDetail.CityId,
                        CountryId = model.UserDetail.CountryId,
                        EMail = model.UserDetail.EMail.Trim()
                    }
                };
                _userRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
        public Result Update(UserModel model)
        {
            try
            {
                if (_userRepository.Query().Any(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim() && u.Id!=model.Id))
                    return new ErrorResult("User with the same user name exists!");
                if (_userRepository.Query("UserDetail").Any(u => u.UserDetail.EMail.ToUpper() == model.UserDetail.EMail.ToUpper().Trim() && u.Id != model.Id))
                    return new ErrorResult("User with the same e-mail exists!");
                var entity = new User()
                {
                    Id=model.Id,
                    Active = model.Active,
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    RoleId = model.RoleId,
                    UserDetail = new UserDetail()
                    {
                        Id=model.UserDetail.Id,
                        Address = model.UserDetail.Address.Trim(),
                        CityId = model.UserDetail.CityId,
                        CountryId = model.UserDetail.CountryId,
                        EMail = model.UserDetail.EMail.Trim()
                    }
                };
                _userRepository.Update(entity);
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
                _userRepository.Delete(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _userRepository?.Dispose();
        }

        public Result<List<UserModel>> GetUsers()
        {
            try
            {
                var users = _userRepository.Query().ToList();
                if (users == null || users.Count == 0)
                    return new ErrorResult<List<UserModel>>("No users found!");
                return new SuccessResult<List<UserModel>>();
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<UserModel>>(exc);
            }
        }
        public Result<UserModel> GetUser(int id)
        {
            try
            {
                var user = _userRepository.Query().SingleOrDefault(u=>u.Id==id);
                if (user == null)
                    return new ErrorResult<UserModel>("No user found!");
                return new SuccessResult<UserModel>();
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserModel>(exc);
            }
        }
        public Result<UserModel> GetUser(Expression<Func<UserModel, bool>> predicate)
        {
            try
            {
                var user = GetQuery().SingleOrDefault(predicate);
                if (user == null)
                    return new ErrorResult<UserModel>("No user found!");
                return new SuccessResult<UserModel>();
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserModel>(exc);
            }
        }
    }
}
