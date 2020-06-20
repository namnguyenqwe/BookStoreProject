using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface ICouponService
    {
        Task<Coupon> GetCouponById(string couponId);
        Task<bool> CreateCoupon(Coupon couponCreate);
        Task<bool> UpdateCoupon(Coupon couponUpdate);
        Task<bool> DeleteCoupon(string couponId);
        IEnumerable<Coupon> GetCoupons(string keyword);
    }
    public class CouponService : ICouponService
    {
        private readonly BookStoreDbContext _dbContext;
        public CouponService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateCoupon(Coupon couponCreate)
        {
            try
            {
                _dbContext.Coupons.Add(couponCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteCoupon(string couponId)
        {
            try
            {
                var couponInDB = await _dbContext.Coupons.Include(x => x.Orders).FirstOrDefaultAsync(x => x.CouponID == couponId);
                if (couponInDB == null)
                    return false;
                _dbContext.Coupons.Remove(couponInDB);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Coupon> GetCouponById(string couponId)
        {
            return await _dbContext.Coupons.FirstOrDefaultAsync(x => x.CouponID.ToUpper() == couponId.ToUpper());
        }

        public IEnumerable<Coupon> GetCoupons(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _dbContext.Coupons
                        .Where(x =>
                        x.CouponID.ToUpper().Contains(keyword.ToUpper()) ||
                        x.Status.ToUpper().Contains(keyword.ToUpper())).AsEnumerable();
            }
            return _dbContext.Coupons.AsEnumerable();
        }

        public async Task<bool> UpdateCoupon(Coupon couponUpdate)
        {
            try
            {
                _dbContext.Coupons.Update(couponUpdate);
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
