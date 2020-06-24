using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetCities();
    }
    public class CityService : ICityService
    {
        private readonly BookStoreDbContext _dbContext;
        public CityService (BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<City>> GetCities()
        {
            return await _dbContext.Cities.ToListAsync();
        }
    }
}
