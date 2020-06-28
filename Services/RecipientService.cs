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
        Task<IEnumerable<Recipient>> GetRecipientsByEmail(string email);
        Task<bool> UpdateRecipient(Recipient recipientUpdate);
        Task<Recipient> GetRecipientById(int recipientId, string email);
        Task<Recipient> GetDefaultRecipient(string email);
    }
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
                    if (recipientCreate.Default == true)
                    {
                        var RecipientInDB = await _dbContext.Recipients
                        .FirstOrDefaultAsync(x => x.Default == true && x.Email == recipientCreate.Email);
                        if (RecipientInDB != null)
                        {
                            RecipientInDB.Default = false;
                            var result = await UpdateRecipient(RecipientInDB);
                            if (!result)
                                return false;
                        }    
                    }    
                    _dbContext.Recipients.Add(recipientCreate);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        public async Task<Recipient> GetDefaultRecipient(string email)
        {
            var defaultRecipient = await _dbContext.Recipients.Include(x => x.District).Include(x => x.City)
                    .FirstOrDefaultAsync(x => x.Email == email && x.Default == true && x.Status == true);
            if (defaultRecipient != null)
                return defaultRecipient;
            Random rnd = new Random();
            var recipients = await GetRecipientsByEmail(email);
            var randomDefault = recipients.Skip(rnd.Next(0, recipients.Count() - 1)).FirstOrDefault();
            randomDefault.Default = true;
            await UpdateRecipient(randomDefault);
            return await GetRecipientById(randomDefault.RecipientID, email);
        }

        public async Task<Recipient> GetRecipientById(int recipientId, string email)
        {
            return await _dbContext.Recipients.Include(x => x.City).Include(x => x.District)
                    .FirstOrDefaultAsync(x => x.RecipientID == recipientId && x.Email == email && x.Status == true);
        }

        public async Task<IEnumerable<Recipient>> GetRecipientsByEmail(string email)
            {
                return await _dbContext.Recipients.Include(x => x.City)
                               .Include(x => x.District).Where(x => x.Email == email && x.Status == true).ToListAsync();
            }

            public async Task<bool> UpdateRecipient(Recipient recipientUpdate)
            {
                try
                {
                    _dbContext.Recipients.Update(recipientUpdate);
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
