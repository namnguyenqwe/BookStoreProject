using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookStoreProject.Models;
using BookStoreProject.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.DataAccess
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        protected DbSet<T> _dbSet;
        private BookStoreDbContext _bookStoreDbContext;
        protected IDBFactory dBFactory
        {
            get;
            private set;
        }

        public BookStoreDbContext BookStoreDbContext
        {
            get { return _bookStoreDbContext ?? (_bookStoreDbContext = dBFactory.Init()); }
        }

        public BaseRepository(IDBFactory dbFactory)
        {
            dBFactory = dbFactory;
            _dbSet = BookStoreDbContext.Set<T>();
        }
        public virtual T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = BookStoreDbContext.Set<T>().Include(includes.First());
                foreach (string i in includes.Skip(1))
                {
                    query = query.Include(i);
                }
                return query.FirstOrDefault(expression);
            }
            return BookStoreDbContext.Set<T>().FirstOrDefault(expression);
        }


    }
}
