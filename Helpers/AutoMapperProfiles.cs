using AutoMapper;
using BookStoreProject.Dtos.Admin;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Dtos.Category;
using BookStoreProject.Dtos.Order;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BookForCreateDto, Book>().ForMember(x => x.BookID, opt => opt.Ignore())
                                                .ForMember(x => x.QuantityOut, opt => opt.Ignore());
            CreateMap<Book, BookForDetailDto>();
            CreateMap<BookForDetailDto, Book>().ForMember(x => x.BookID, opt => opt.Ignore());
            CreateMap<Book, BookForListDto>().ForMember(x => x.Category, y => { y.MapFrom(z => z.Category.Category); })
                                            .ForMember(x => x.publisher, y => { y.MapFrom(z => z.Publisher.publisher); });
            CreateMap<CategoryDto, Categories>().ForMember(x => x.CategoryID, opt => opt.Ignore());
            CreateMap<Categories, CategoryForListDto>();


            #region Applicationuser
            CreateMap<ApplicationUser, UserForListDto>().ForMember(x => x.Id, y => { y.MapFrom(z => z.Id); }); ;
            #endregion

            #region Order
            CreateMap<Orders, OrderForListDto>().ForMember(x => x.Id, y => { y.MapFrom(z => z.OrderID); })
                                                .ForMember(x => x.NameOfUser, y => { y.MapFrom(z => z.ApplicationUser.FullName); })
                                                .ForMember(x => x.NameOfRecipent, y => { y.MapFrom(z => z.Recipient.Name); })
                                                .ForMember(x => x.Phone, y => { y.MapFrom(z => z.Recipient.Phone); })
                                                .ForMember(x => x.Coupon, y => { y.MapFrom(z => z.CouponID); })
                                                .ForMember(x => x.Email, y => { y.MapFrom(z => z.Recipient.Email); })
                                                .ForMember(x => x.Address, y => { y.MapFrom(z => z.Recipient.Address + ", " + z.Recipient.District.district + ", " + z.Recipient.City.city); });


            CreateMap<Orders, OrderForDetailDto>().ForMember(x => x.Email, y => { y.MapFrom(z => z.Recipient.Email); })
                                                  .ForMember(x => x.NameOfRecipient, y => { y.MapFrom(z => z.Recipient.Name); })
                                                  .ForMember(x => x.Address, y => { y.MapFrom(z => z.Recipient.Address + ", " + z.Recipient.District.district + ", " + z.Recipient.City.city); })
                                                  .ForMember(x => x.ListBook, y => { y.MapFrom(z => z.OrderItems.Select(y => new { y.Book.NameBook, y.Quantity, y.Price })); })
                                                  .ForMember(x => x.Total1, y => { y.MapFrom(z =>z.OrderItems.Sum(y => y.Price)); })
                                                  .ForMember(x => x.Discount, y => { y.MapFrom(z => z.Coupon.Discount); })
                                                  .ForMember(x => x.Pay, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price) - (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100)); })
                                                  .ForMember(x => x.Total2, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price) - (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100) + z.ShippingFee); });


            CreateMap<OrderForCreateDto, Orders>().ForMember(x => x.OrderID, opt => opt.Ignore());
            CreateMap<OrderForUpdateDto, Orders>().ForMember(x => x.OrderID, opt => opt.Ignore())
                                                  .ForMember(x => x.ApplicationUserID, opt => opt.Ignore())
                                                  .ForMember(x => x.Date, opt => opt.Ignore())
                                                  .ForMember(x => x.RecipientID, opt => opt.Ignore())
                                                  .ForMember(x => x.CouponID, opt => opt.Ignore())
                                                  .ForMember(x => x.ShippingFee, opt => opt.Ignore())
                                                  .ForMember(x => x.Note, opt => opt.Ignore());

            CreateMap<Orders, OrderForUserListDto>().ForMember(x => x.Total, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price) - (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100) + z.ShippingFee); });

            CreateMap<Orders,OrderForUserDetailDto>().ForMember(x => x.Email, y => { y.MapFrom(z => z.Recipient.Email); })
                                                  .ForMember(x => x.NameOfRecipient, y => { y.MapFrom(z => z.Recipient.Name); })
                                                  .ForMember(x => x.NameOfUser, y => { y.MapFrom(z => z.ApplicationUser.FullName); })
                                                  .ForMember(x => x.Address, y => { y.MapFrom(z => z.Recipient.Address + ", " + z.Recipient.District.district + ", " + z.Recipient.City.city); })
                                                  .ForMember(x => x.ListBook, y => { y.MapFrom(z => z.OrderItems.Select(y => new { y.Book.NameBook, y.Quantity, y.Price })); })
                                                  .ForMember(x => x.TamTinh, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price)); })
                                                  .ForMember(x => x.Discount, y => { y.MapFrom(z => (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100)); })
                                                  .ForMember(x => x.Total, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price) - (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100) + z.ShippingFee); });
            #endregion
        }
    }
}
