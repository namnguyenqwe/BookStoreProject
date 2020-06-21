using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface ISubcriberService
    {
        Task<bool> CreateSubcriber(Subcriber subcriberCreate);
        Task<bool> DeleteSubcriber(int subcriberId);
        Task<Subcriber> GetSubcriberById(int subcriberId);
        IEnumerable<Subcriber> GetSubcribers(string keyword);
    }
    public class SubcriberService : ISubcriberService
    {
        private readonly BookStoreDbContext _dbContext;
        public SubcriberService (BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }    
        public async Task<bool> CreateSubcriber(Subcriber subcriberCreate)
        {
            try
            {
                _dbContext.Subcribers.Add(subcriberCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteSubcriber(int subcriberId)
        {
            try
            {
                var subcriberInDB = await _dbContext.Subcribers.FirstOrDefaultAsync(x => x.SubcriberId == subcriberId);
                if (subcriberInDB == null)
                    return false;
                _dbContext.Subcribers.Remove(subcriberInDB);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Subcriber> GetSubcriberById(int subcriberId)
        {
            return await _dbContext.Subcribers.FirstOrDefaultAsync(x => x.SubcriberId == subcriberId);
        }

        public IEnumerable<Subcriber> GetSubcribers(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _dbContext.Subcribers
                        .Where(x =>
                        x.Email.ToUpper().Contains(keyword.ToUpper())).AsEnumerable();
            }
            return _dbContext.Subcribers.AsEnumerable();
        }
    }
}
