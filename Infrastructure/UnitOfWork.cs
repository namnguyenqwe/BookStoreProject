using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Models;

namespace BookStoreProject.Infrastructure
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly IDBFactory dbFactory;
        private BookStoreDbContext dbContext;

        public UnitOfWork(IDBFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public BookStoreDbContext DbContext
        {
            get { return dbContext ??(dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
