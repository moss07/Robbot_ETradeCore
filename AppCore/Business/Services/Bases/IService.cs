using System;
using System.Linq;
using System.Linq.Expressions;
using AppCore.Business.Models.Results;
using AppCore.Records;

namespace AppCore.Business.Services.Bases
{
    public interface IService<TModel> : IDisposable where TModel : Record, new()
    {
        IQueryable<TModel> GetQuery(Expression<Func<TModel,bool>> predicate=null);
        Result Add(TModel model);
        Result Update(TModel model);
        Result Delete(int id);
    }
}
