using System;
using System.Linq;
using System.Linq.Expressions;
using AppCore.DataAccess.Repositories.Bases;
using AppCore.Records;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Repositories.EntityFramework
{
    
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Record, new()
    {
        private readonly DbContext _db;

        protected RepositoryBase(DbContext db)
        {
            _db = db;
        }

        public virtual IQueryable<TEntity> Query(params string[] entitiesToInclude)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            foreach (var entityToInclude in entitiesToInclude)
            {
                query = query.Include(entityToInclude);
            }
            return query;
        }

        public virtual void Add(TEntity entity, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString();
            _db.Set<TEntity>().Add(entity);
            if (save)
                Save();
        }

        public virtual void Update(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
                Save();
        }

        public virtual void Delete(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Remove(entity);
            if (save)
                Save();
        }

        public virtual void Delete(int id, bool save=true)
        {
            var entity = Query().SingleOrDefault(e => e.Id == id);
            Delete(entity, save);
        }

        public virtual IQueryable<TEntity> EntityQuery(Expression<Func<TEntity,bool>> predicate, params string[] entitiesToInclude)
        {
            var query = Query(entitiesToInclude);
            return query.Where(predicate);
        }

        public virtual int Save()
        {
            return _db.SaveChanges();
        }

        #region Dispose
        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            _db?.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
