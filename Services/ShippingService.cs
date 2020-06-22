using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IShippingService
    {
        IEnumerable<District> GetShippingLocations(string keyword);
        Task<District> GetShippingLocationById(string DistrictId);
        Task<bool> UpdateShippingLocation(District districtUpdate);
        Task<bool> DeleteShippingLocation(string DistrictId);
    }
    public class ShippingService : IShippingService
    {
        private readonly BookStoreDbContext _dbContext;
        public ShippingService (BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<bool> DeleteShippingLocation(string DistrictId)
        {
            throw new NotImplementedException();
        }

        public async Task<District> GetShippingLocationById(string DistrictId)
        {
            return await _dbContext.Districts.Include(x => x.City).FirstOrDefaultAsync(x => x.DistrictID == DistrictId);
        }

        public IEnumerable<District> GetShippingLocations(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _dbContext.Districts.Include(x => x.City).Where(x =>
                        x.district.ToUpper().Contains(keyword.ToUpper())
                        || x.City.city.ToUpper().Contains(keyword.ToUpper()))
                    .AsEnumerable();
            }
            return _dbContext.Districts.Include(x => x.City).AsEnumerable();
        }

        public async Task<bool> UpdateShippingLocation(District districtUpdate)
        {
            try
            {
                _dbContext.Districts.Update(districtUpdate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
