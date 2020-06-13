using BookStoreProject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Models;
using BookStoreProject.Infrastructure;

namespace BookStoreProject.Repositorys
{
    public interface IOrdersRepository:IBaseRepository<Orders>
    {

    }
    public class OrdersRepository:BaseRepository<Orders>, IOrdersRepository
    {
        public OrdersRepository(IDBFactory dFbactory):base(dFbactory)
        {

        }
    }
}
