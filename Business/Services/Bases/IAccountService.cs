using AppCore.Business.Models.Results;
using Business.Models;

namespace Business.Services.Bases
{
    public interface IAccountService
    {
        Result Register(UserRegisterModel model);
        Result<UserLoginModel> Login(UserLoginModel model);
    }
}
