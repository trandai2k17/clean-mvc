using clean_mvc.Application.Common.Interfaces;
using clean_mvc.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clean_mvc.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext applicationDbContext) 
        {
            _db = applicationDbContext;

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
