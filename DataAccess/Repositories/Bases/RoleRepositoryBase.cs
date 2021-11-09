using AppCore.DataAccess.Repositories.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Bases
{
    public abstract class RoleRepositoryBase : RepositoryBase<Role>
    {
        protected RoleRepositoryBase(DbContext db) : base(db)
        {

        }
    }
}
