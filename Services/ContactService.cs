using BookStoreProject.Helpers;
using BookStoreProject.Models;
using EnumsNET;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IContactService
    {
        Task<bool> CreateContact(Contact contactCreate);
        Task<bool> UpdateContact(Contact contactUpdate);
        Task<bool> DeleteContact(int contactId);
        Task<Contact> GetContactById(int contactId);
        IEnumerable<Contact> GetContacts(string keyword);
    }
    public class ContactService : IContactService
    {
        private readonly BookStoreDbContext _dbContext;
        public ContactService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateContact(Contact contactCreate)
        {
            try
            {
                _dbContext.Contacts.Add(contactCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteContact(int contactId)
        {
            try
            {
                var contactInDB = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.ContactID == contactId);
                if (contactInDB == null)
                    return false;
                _dbContext.Contacts.Remove(contactInDB);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Contact> GetContactById(int contactId)
        {
            return await _dbContext.Contacts.FirstOrDefaultAsync(x => x.ContactID == contactId);
        }

       public IEnumerable<Contact> GetContacts(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _dbContext.Contacts
                        .Where(delegate (Contact c)
                        {
                            if (MyConvert.ConvertToUnSign(c.Email.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                            MyConvert.ConvertToUnSign(c.Status.AsString(EnumFormat.Description).ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                            MyConvert.ConvertToUnSign(c.Name.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                            c.Email.ToUpper().Contains(keyword.ToUpper()) ||
                            c.Status.AsString(EnumFormat.Description).ToUpper().Contains(keyword.ToUpper()) ||
                            String.Format("{0:d/M/yyyy}", c.Date).ToUpper().Contains(keyword.ToUpper()) ||
                            c.Phone.ToUpper().Contains(keyword.ToUpper()))
                                return true;
                            else
                                return false;
                        }).AsEnumerable();
            }
            return _dbContext.Contacts.AsEnumerable();
        }

        public async Task<bool> UpdateContact(Contact contactUpdate)
        {
            try
            {
                _dbContext.Contacts.Update(contactUpdate);
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
