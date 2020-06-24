using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IDistrictService
    {
        Task<IEnumerable<District>> GetDistrictsByCityId(string cityId);
    }
    public class DistrictService : IDistrictService
    {
        private readonly BookStoreDbContext _dbContext;
        public DistrictService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<District>> GetDistrictsByCityId(string cityId)
        {
            return await _dbContext.Districts.Where(x => x.CityID == cityId).ToListAsync();
        }
    }
}
