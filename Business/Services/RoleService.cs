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
    public class RoleService : IRoleService
    {
        private readonly RoleRepositoryBase _roleRepository;

        public RoleService(RoleRepositoryBase roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public IQueryable<RoleModel> GetQuery(Expression<Func<RoleModel, bool>> predicate = null)
        {
            return _roleRepository.Query("Users").OrderBy(r => r.Name).Select(r => new RoleModel()
            {
                Id = r.Id,
                Name = r.Name,
                Guid = r.Guid,
                Users = r.Users.Select(u=>new UserModel() 
                {
                    Id=u.Id,
                    UserName=u.UserName,
                    Active=u.Active
                }).ToList()
            });
        }

        public Result Add(RoleModel model)
        {
            try
            {
                if (_roleRepository.Query().Any(r => r.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("Role with the same name exists!");
                var entity = new Role()
                {
                    Name = model.Name.Trim()
                };
                _roleRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
        public Result Update(RoleModel model)
        {
            try
            {
                if (_roleRepository.Query().Any(r => r.Name.ToUpper() == model.Name.ToUpper().Trim() && r.Id!=model.Id))
                    return new ErrorResult("Role with the same name exists!");
                var entity = new Role()
                {
                    Id=model.Id,
                    Name = model.Name.Trim()
                };
                _roleRepository.Update(entity);
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
                var role = _roleRepository.EntityQuery(r => r.Id == id, "Users").SingleOrDefault();
                if (role == null)
                    return new ErrorResult("Role not found!");
                if (role.Users != null && role.Users.Count > 0)
                    return new ErrorResult("Role cannot be deleted because it has users!");
                _roleRepository.Delete(role);
                return new SuccessResult("Role successfully deleted.");
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _roleRepository?.Dispose();
        }
    }
}
