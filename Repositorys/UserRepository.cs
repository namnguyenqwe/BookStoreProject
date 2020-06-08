using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Models;
using BookStoreProject.DataAccess;
using BookStoreProject.Infrastructure;

namespace BookStoreProject.Repositorys
{

    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {

    }
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(IDBFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
