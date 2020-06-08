using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookStoreProject.DataAccess
{
    public interface IBaseRepository<T> where T:class
    {
        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes);   
    }
}
