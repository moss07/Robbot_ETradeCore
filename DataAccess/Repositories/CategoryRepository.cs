using DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class CategoryRepository : CategoryRepositoryBase
    {
        public CategoryRepository(DbContext db) : base(db)
        {

        }
    }
}
