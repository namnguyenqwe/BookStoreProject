using BookStoreProject.Helpers;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface ISubcriberService
    {
        Task<bool> CreateSubcriber(Subcriber subcriberCreate);
        Task<bool> DeleteSubcriber(int subcriberId);
        Task<Subcriber> GetSubcriberById(int subcriberId);
        IEnumerable<Subcriber> GetSubcribers(string keyword);
        Task<bool> UpdateSubcriber(Subcriber subcriberUpdate);
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
                var subcriber = await _dbContext.Subcribers.FirstOrDefaultAsync(x => x.Email == subcriberCreate.Email);
                if (subcriber != null)
                    return false;
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
                        .Where(delegate (Subcriber s)
                        {
                            if (MyConvert.ConvertToUnSign(s.Email.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||

                            s.Email.ToUpper().Contains(keyword.ToUpper()))

                                return true;
                            else
                                return false;
                        }).AsEnumerable();
            }
            return _dbContext.Subcribers.AsEnumerable();
        }

        public async Task<bool> UpdateSubcriber(Subcriber subcriberUpdate)
        {
           try
            {
                var subcriber = await _dbContext.Subcribers.FirstOrDefaultAsync(x => x.Email == subcriberUpdate.Email && x.SubcriberId != subcriberUpdate.SubcriberId);
                if (subcriber != null)
                    return false;
                var subcriberInDB = await _dbContext.Subcribers.FirstOrDefaultAsync(x => x.SubcriberId == subcriberUpdate.SubcriberId);
                if (subcriberInDB == null)
                    return false;
                _dbContext.Subcribers.Update(subcriberUpdate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
