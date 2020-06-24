using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IRecipientService
    {
        Task<bool> CreateRecipient(Recipient recipientCreate);
        Task<IEnumerable<Recipient>> GetRecipientsByApplicationUserId(string applicationUserId);

        public class RecipientService : IRecipientService
        {
            private readonly BookStoreDbContext _dbContext;
            public RecipientService(BookStoreDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<bool> CreateRecipient(Recipient recipientCreate)
            {
                try
                {
                    _dbContext.Recipients.Add(recipientCreate);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public async Task<IEnumerable<Recipient>> GetRecipientsByApplicationUserId(string applicationUserId)
            {
                return await _dbContext.Recipients.Where(x => x.ApplicationUserID == applicationUserId).ToListAsync();
            }
        }
    }
