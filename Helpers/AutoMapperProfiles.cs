using AutoMapper;
using BookStoreProject.Dtos.Admin;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Dtos.Category;
using BookStoreProject.Dtos.Order;
using BookStoreProject.Models;
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
            CreateMap<CategoryDto,Categories>().ForMember(x => x.CategoryID, opt => opt.Ignore());
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
                                                .ForMember(x=>x.Address,y=> { y.MapFrom(z => z.Recipient.City.type); });
            CreateMap<Orders, OrderForDetailDto>();

            CreateMap<OrderForCreateDto, Orders>().ForMember(x => x.OrderID, opt => opt.Ignore());
            CreateMap<OrderForUpdateDto, Orders>().ForMember(x => x.OrderID, opt => opt.Ignore())
                                                  .ForMember(x => x.ApplicationUserID, opt => opt.Ignore())
                                                  .ForMember(x => x.Date, opt => opt.Ignore())
                                                  .ForMember(x => x.RecipientID, opt => opt.Ignore())
                                                  .ForMember(x => x.CouponID, opt => opt.Ignore())
                                                  .ForMember(x => x.ShippingFee, opt => opt.Ignore())
                                                  .ForMember(x => x.Note, opt => opt.Ignore());


            #endregion
        }
    }
}
