using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookStoreProject.DataAccess;
using BookStoreProject.Models;
using BookStoreProject.Infrastructure;
using BookStoreProject.Repositorys;


namespace BookStoreProject.Services
{
    public interface ICommonService<T>where T:class
    {
       
        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes);
        

    }
    public class CommonService<T, R> : ICommonService<T>
         where T : class
         where R : IBaseRepository<T>
    {
        private R _repository;
        private IUnitOfWork _unitOfWork;
        public CommonService(IUnitOfWork unitOfWork, R repository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes)
        {
            return _repository.GetSingleByCondition(expression, includes);
        }



    }
}
