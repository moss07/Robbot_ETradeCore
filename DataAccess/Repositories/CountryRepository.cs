using DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class CountryRepository : CountryRepositoryBase
    {
        public CountryRepository(DbContext db) : base(db)
        {

        }
    }
}
