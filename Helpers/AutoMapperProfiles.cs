using AutoMapper;
using BookStoreProject.Dtos.Admin;
using BookStoreProject.Dtos.ApplicationUser;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Dtos.CartItem;
using BookStoreProject.Dtos.Category;
using BookStoreProject.Dtos.City;
using BookStoreProject.Dtos.Coupon;
using BookStoreProject.Dtos.District;
using BookStoreProject.Dtos.Publisher;
using BookStoreProject.Dtos.Review;
using BookStoreProject.Dtos.Subcriber;
using BookStoreProject.Dtos.WishList;
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
            #region Book
            CreateMap<BookForCreateDto, Book>().ForMember(x => x.BookID, opt => opt.Ignore())
                                                .ForMember(x => x.QuantityOut, opt => opt.Ignore());
            CreateMap<Book, BookForDetailDto>();
            CreateMap<BookForUpdateDto, Book>().ForMember(x => x.QuantityOut, opt => opt.Ignore());
            CreateMap<Book, BookForListDto>();

            CreateMap<Book, BookForUserDetailDto>().ForMember(x => x.Category, y => { y.MapFrom(z => z.Category.Category); })
                                            .ForMember(x => x.publisher, y => { y.MapFrom(z => z.Publisher.publisher); })
                                            .ForMember(x => x.ReviewCount, y => { y.MapFrom(z => z.Reviews.Count); })
                                            .ForMember(x => x.Rating, y => {
                                                y.MapFrom(z => z.Reviews.Any() ? z.Reviews.Aggregate((a, b) => new Review { BookID = 0, Rating = a.Rating + b.Rating }).Rating / z.Reviews.Count : 0);
                                            })
                                            .ForMember(x => x.Reviews, y => { y.MapFrom(z => z.Reviews.Take(3)); })
                                            .ForMember(x => x.RemainQuantity, y => { y.MapFrom(z => z.QuantityIn - z.QuantityOut); });
            CreateMap<Book, BookForUserRelatedListDto>();
            CreateMap<Book, BookForUserSearchListDto>().ForMember(x => x.ReviewCount, y => { y.MapFrom(z => z.Reviews.Count); })
                                                        .ForMember(x => x.Rating, y => {
                                                            y.MapFrom(z => z.Reviews.Any() ? z.Reviews.Aggregate((a, b) => new Review { BookID = 0, Rating = a.Rating + b.Rating }).Rating / z.Reviews.Count : 0);
                                                        });
            #endregion

            #region Category
            CreateMap<CategoryDto, Categories>().ForMember(x => x.CategoryID, opt => opt.Ignore());
            CreateMap<Categories, CategoryForSelectDto>();
            CreateMap<Categories, CategoryForListDto>().ForMember(x => x.BookTitleCount, y => { y.MapFrom(z => z.Books.Count); });
            CreateMap<Categories, CategoryForUserListDto>().ForMember(x => x.ImageLink,
                                                        y =>
                                                        {
                                                            y.MapFrom(z => z.Books.OrderByDescending(x => x.Date.Year)
                                                                                    .ThenByDescending(x => x.Date.Month)
                                                                                    .ThenByDescending(x => x.Date.Day)
                                                                                    .FirstOrDefault().ImageLink);
                                                        });
            #endregion

            #region Review
            CreateMap<Review, ReviewForListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                           .ForMember(x => x.Email, y => { y.MapFrom(z => z.ApplicationUser.Email); });
            CreateMap<Review, ReviewForUserListDto>().ForMember(x => x.AvatarLink, y => { y.MapFrom(z => z.ApplicationUser.AvatarLink); })
                                                    .ForMember(x => x.Name, y => { y.MapFrom(z => z.ApplicationUser.Name); });
            CreateMap<ReviewForUserCreateDto, Review>();
            #endregion

            #region Publisher
            CreateMap<Publisher, PublisherForSelectDto>();
            CreateMap<Publisher, PublisherForListDto>();
            CreateMap<PublisherDto, Publisher>();
            #endregion

            #region Coupon
            CreateMap<Coupon, CouponForListDto>();
            CreateMap<CouponForModalDto, Coupon>();
            #endregion

            #region CartItem
            CreateMap<CartItems, CartItemForUserListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                                          .ForMember(x => x.Author, y => { y.MapFrom(z => z.Book.Author); })
                                                          .ForMember(x => x.Price, y => { y.MapFrom(z => z.Book.Price); })
                                                          .ForMember(x => x.ImageLink, y => { y.MapFrom(z => z.Book.ImageLink); })
                                                          .ForMember(x => x.OriginalPrice, y => { y.MapFrom(z => z.Book.OriginalPrice); });
            CreateMap<CartItems, CartItemForPaymentListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                                         .ForMember(x => x.Price, y => { y.MapFrom(z => z.Book.Price); })
                                                         .ForMember(x => x.ImageLink, y => { y.MapFrom(z => z.Book.ImageLink); });
            CreateMap<CartItemForUserUpdateDto, CartItems>().ForMember(x => x.CreatedDate, opt => opt.Ignore())
                                                            .ForMember(x => x.ApplicationUserId, opt => opt.Ignore())
                                                            .ForMember(x => x.BookID, opt => opt.Ignore());
            CreateMap<CartItems, CartItemForUserUpdateDto>();
            #endregion

            #region District
            CreateMap<District, DistrictForListDto>().ForMember(x => x.city, y => { y.MapFrom(z => z.City.city); });
            CreateMap<District, DistrictForDetailDto>().ForMember(x => x.city, y => { y.MapFrom(z => z.City.city); });
            CreateMap<DistrictForUpdateDto, District>().ForMember(x => x.DistrictID, opt => opt.Ignore())
                                                       .ForMember(x => x.CityID, opt => opt.Ignore())
                                                       .ForMember(x => x.type, opt => opt.Ignore())
                                                       .ForMember(x => x.district, opt => opt.Ignore());
            CreateMap<District, DistrictForUserListDto>();
            #endregion

            #region City
            CreateMap<City,CityForUserListDto>();
            #endregion

            #region ApplicationUser
            CreateMap<ApplicationUser, ApplicationUserForProfileDto>().ForMember(x => x.ApplicationUserId, y => { y.MapFrom(z => z.Id); })
                                                                       .ForMember(x => x.Name, y => { y.MapFrom(z => z.Name); }); 
            CreateMap<ApplicationUser, UserForListDto>().ForMember(x => x.Id, y => { y.MapFrom(z => z.Id); });
            #endregion

            #region Subcriber
            CreateMap<SubcriberForModalDto, Subcriber>().ForMember(x => x.CreatedDate, opt => opt.Ignore())
                                                        .ForMember(x => x.SubcriberId, opt => opt.Ignore());
            #endregion

            #region WishList
            CreateMap<WishList, WishListForUserListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                                        .ForMember(x => x.ImageLink, y => { y.MapFrom(z => z.Book.ImageLink); })
                                                        .ForMember(x => x.OriginalPrice, y => { y.MapFrom(z => z.Book.OriginalPrice); })
                                                        .ForMember(x => x.Price, y => { y.MapFrom(z => z.Book.Price); })
                                                        .ForMember(x => x.ReviewCount, y => { y.MapFrom(z => z.Book.Reviews.Count); })
                                                        .ForMember(x => x.Rating, y =>
                                                        {
                                                            y.MapFrom(z => z.Book.Reviews.Any() ? z.Book.Reviews.Aggregate((a, b) => new Review { BookID = 0, Rating = a.Rating + b.Rating }).Rating / z.Book.Reviews.Count : 0);
                                                        });
            #endregion

            #region Order
            CreateMap<Orders, OrderForListDto>().ForMember(x => x.Id, y => { y.MapFrom(z => z.OrderID); })
                                                .ForMember(x => x.NameOfUser, y => { y.MapFrom(z => z.ApplicationUser.Name); })
                                                .ForMember(x => x.NameOfRecipent, y => { y.MapFrom(z => z.Recipient.Name); })
                                                .ForMember(x => x.Phone, y => { y.MapFrom(z => z.Recipient.Phone); })
                                                .ForMember(x => x.Coupon, y => { y.MapFrom(z => z.CouponID); })
                                                .ForMember(x=>x.Address, y => { y.MapFrom(z => z.Recipient.Address + "," + z.Recipient.District.district + "," + z.Recipient.City.city); });
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
