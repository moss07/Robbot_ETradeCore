using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Bases
{
    public interface IUserService : IService<UserModel>
    {
        Result<List<UserModel>> GetUsers();
        Result<UserModel> GetUser(int id);
        Result<UserModel> GetUser(Expression<Func<UserModel, bool>> predicate);
    }
}
