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

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            BookStoreDbContext.Entry(entity).State = EntityState.Modified;
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T GetSingleById(int id)
        {
            return _dbSet.Find(id);
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

        public virtual IEnumerable<T> GetAll(string[] includes)
        {
            if(includes != null && includes.Count()>0)
            {
                var query = BookStoreDbContext.Set<T>().Include(includes.First());
                foreach(string i in includes.Skip(1))
                {
                    query = query.Include(i);
                }
                return query.AsQueryable();
            }
            return BookStoreDbContext.Set<T>().AsQueryable();
        }

        public virtual IEnumerable<T> GetMultiByCondition(Expression<Func<T, bool>> expression, string[] includes)
        {
            if(includes != null && includes.Count()>0)
            {
                var query = BookStoreDbContext.Set<T>().Include(includes.First());
                foreach(string i in includes.Skip(1))
                {
                    query = query.Include(i);
                }
                return query.Where<T>(expression).AsQueryable();
            }
            return BookStoreDbContext.Set<T>().Where<T>(expression).AsQueryable();
        }

        public virtual IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> expression, int index = 0, int size = 10, string[] includes = null)
        {
            var skipCount = index * size;
            IQueryable<T> _resetSet = null;
            if(includes != null && includes.Count() > 0)
            {
                var query = BookStoreDbContext.Set<T>().Include(includes.First());
                foreach(string i in includes.Skip(1))
                {
                    query = query.Include(i);
                }
                _resetSet = expression != null ? query.Where<T>(expression).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = expression != null ? BookStoreDbContext.Set<T>().Where<T>(expression).AsQueryable() : BookStoreDbContext.Set<T>().AsQueryable();
            }
            _resetSet = index == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            return _resetSet.AsQueryable();
        }
        public int GetCount(Expression<Func<T, bool>> expression)
        {
            return expression == null ? BookStoreDbContext.Set<T>().Count() : BookStoreDbContext.Set<T>().Where(expression).Count();
        }

    }
}
