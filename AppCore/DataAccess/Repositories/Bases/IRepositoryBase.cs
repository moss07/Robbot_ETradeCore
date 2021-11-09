using System;
using System.Linq;
using AppCore.Records;

namespace AppCore.DataAccess.Repositories.Bases
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : Record, new()
    {
        IQueryable<TEntity> Query(params string[] entitiesToInclude);
        void Add(TEntity entity, bool save = true);
        void Update(TEntity entity, bool save = true);
        void Delete(TEntity entity, bool save = true);
        int Save();
    }
}
