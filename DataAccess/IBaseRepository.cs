using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookStoreProject.DataAccess
{
    public interface IBaseRepository<T> where T:class
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);

        T GetSingleById(int id);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes);

        IEnumerable<T> GetAll(string[] includes);
        IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> expression, int index = 0, int size = 10, string[] includes = null);
        int GetCount(Expression<Func<T, bool>> expression);
        
    }
}
