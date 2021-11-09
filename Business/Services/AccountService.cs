using AppCore.Business.Models.Results;
using Business.Enums;
using Business.Models;
using Business.Services.Bases;
using DataAccess.Repositories.Bases;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public class AccountService :IAccountService
    {
        private readonly UserRepositoryBase _userRepository;

        public AccountService(UserRepositoryBase userRepository)
        {
            _userRepository = userRepository;
        }

        public Result Register(UserRegisterModel model)
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
                    UserName=model.UserName.Trim(),
                    Password=model.Password.Trim(),
                    RoleId=(int)Roles.User,
                    UserDetail=new UserDetail()
                    {
                        Address=model.UserDetail.Address.Trim(),
                        CityId=model.UserDetail.CityId,
                        CountryId=model.UserDetail.CountryId,
                        EMail=model.UserDetail.EMail.Trim()
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
        public Result<UserLoginModel> Login(UserLoginModel model)
        {
            try
            {
                var user = _userRepository.Query("Role").SingleOrDefault(u => u.UserName == model.UserName && u.Password == model.Password && u.Active);
                if (user == null)
                    return new ErrorResult<UserLoginModel>("No user found!");
                var userModel = new UserLoginModel()
                {
                    Id = user.Id,
                    Guid = user.Guid,
                    UserName = user.UserName,
                    RoleName=user.Role.Name
                };
                return new SuccessResult<UserLoginModel>(userModel);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserLoginModel>(exc);
            }
        }
    }
}
