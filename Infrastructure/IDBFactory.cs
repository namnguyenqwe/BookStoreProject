using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Models;

namespace BookStoreProject.Infrastructure
{
    public interface IDBFactory
    {
        BookStoreDbContext Init();
    }
}
