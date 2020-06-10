using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreProject.Infrastructure
{
    public class DBFactory:IDBFactory
    {
        private BookStoreDbContext _bookStoreDbContext;
        public BookStoreDbContext Init()
        {
            //string _connectionString = "Server=A-PC;Database=BookStoreProject;Integrated Security=True";
            string _connectionString = "Data Source=tcp:dut-sql-101.database.windows.net,1433;Initial Catalog=BookStoreProject_db;User Id=appuser@dut-sql-101;Password=qwerty123!@#";
            var optionsBuilder = new DbContextOptionsBuilder<BookStoreDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            return _bookStoreDbContext ?? (_bookStoreDbContext = new BookStoreDbContext(optionsBuilder.Options));
        }
    }
}
