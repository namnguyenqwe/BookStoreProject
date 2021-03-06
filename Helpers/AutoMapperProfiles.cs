﻿using AutoMapper;
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

using BookStoreProject.Dtos.Recipient;

using System.Runtime.CompilerServices;
using BookStoreProject.Dtos.Payment;
using BookStoreProject.Dtos.Contact;
using EnumsNET;
using BookStoreProject.Dtos.OrderItem;

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
                                                y.MapFrom(z => z.Reviews.Any() ? (int) Math.Round((double)(z.Reviews.Aggregate((a, b) => new Review { BookID = 0, Rating = a.Rating + b.Rating }).Rating / (double) z.Reviews.Count)) : 0);
                                            })
                                            .ForMember(x => x.Reviews, y => { y.MapFrom(z => z.Reviews.Take(3)); })
                                            .ForMember(x => x.RemainQuantity, y => { y.MapFrom(z => z.QuantityIn - z.QuantityOut); });
            CreateMap<Book, BookForUserRelatedListDto>();
            CreateMap<Book, BookForUserSearchListDto>().ForMember(x => x.ReviewCount, y => { y.MapFrom(z => z.Reviews.Count); })
                                                        .ForMember(x => x.Rating, y => {
                                                            y.MapFrom(z => z.Reviews.Any() ? (int)Math.Round((double)(z.Reviews.Aggregate((a, b) => new Review { BookID = 0, Rating = a.Rating + b.Rating }).Rating / (double)z.Reviews.Count)) : 0);
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
            CreateMap<Coupon, CouponForPaymentDto>();
            #endregion

            #region CartItem
            CreateMap<CartItems, CartItemForUserListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                                          .ForMember(x => x.Author, y => { y.MapFrom(z => z.Book.Author); })
                                                          .ForMember(x => x.Price, y => { y.MapFrom(z => z.Book.Price); })
                                                          .ForMember(x => x.ImageLink, y => { y.MapFrom(z => z.Book.ImageLink); })
                                                          .ForMember(x => x.OriginalPrice, y => { y.MapFrom(z => z.Book.OriginalPrice); });
            CreateMap<CartItems, CartItemForPaymentListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                                         .ForMember(x => x.Price, y => { y.MapFrom(z => z.Book.Price); })
                                                         .ForMember(x => x.ImageLink, y => { y.MapFrom(z => z.Book.ImageLink); })
                                                         .ForMember(x => x.Weight, y => { y.MapFrom(z => z.Book.Weight); });
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
                                                        .ForMember(x => x.Author, y => { y.MapFrom(z => z.Book.Author); })
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
                                                .ForMember(x => x.Email, y => { y.MapFrom(z => z.Recipient.Email); })
                                                .ForMember(x=> x.Address, y => { y.MapFrom(z => z.Recipient.Address + ", " + z.Recipient.District.district + ", " + z.Recipient.City.city); });




            CreateMap<Orders, OrderForDetailDto>().ForMember(x => x.Email, y => { y.MapFrom(z => z.Recipient.Email); })
                                                  .ForMember(x => x.Phone, y => { y.MapFrom(z => z.Recipient.Phone); })
                                                  .ForMember(x => x.NameOfRecipient, y => { y.MapFrom(z => z.Recipient.Name); })
                                                  .ForMember(x => x.Address, y => { y.MapFrom(z => z.Recipient.Address + ", " + z.Recipient.District.district + ", " + z.Recipient.City.city); })
                                                  //.ForMember(x => x.ListBook, y => { y.MapFrom(z => z.OrderItems.Select(y => new { y.Book.NameBook, y.Quantity, y.Price })); })
                                                  .ForMember(x => x.Total1, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price)); })
                                                  .ForMember(x => x.Discount, y => { y.MapFrom(z => z.Coupon.Discount); });
                                                  //.ForMember(x => x.Pay, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price) - (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100)); })
                                                  //.ForMember(x => x.Total2, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price) - (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100) + z.ShippingFee); }); ;


            CreateMap<OrderForCreateDto, Orders>().ForMember(x => x.OrderID, opt => opt.Ignore());
            CreateMap<OrderForUpdateDto, Orders>().ForMember(x => x.OrderID, opt => opt.Ignore())
                                                  .ForMember(x => x.ApplicationUserID, opt => opt.Ignore())
                                                  .ForMember(x => x.Date, opt => opt.Ignore())
                                                  .ForMember(x => x.RecipientID, opt => opt.Ignore())
                                                  .ForMember(x => x.CouponID, opt => opt.Ignore())
                                                  .ForMember(x => x.ShippingFee, opt => opt.Ignore())
                                                  .ForMember(x => x.Note, opt => opt.Ignore());

            CreateMap<Orders, OrderForUserListDto>().ForMember(x => x.Total, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price) - (decimal)((z.OrderItems.Sum(y => y.Price) * (z.Coupon == null ? 0 : z.Coupon.Discount) ) / 100) + z.ShippingFee); });

            CreateMap<Orders, OrderForUserDetailDto>().ForMember(x => x.Email, y => { y.MapFrom(z => z.Recipient.Email); })
                                                  .ForMember(x => x.NameOfRecipient, y => { y.MapFrom(z => z.Recipient.Name); })
                                                  .ForMember(x => x.Phone, y => { y.MapFrom(z => z.Recipient.Phone); })
                                                  .ForMember(x => x.NameOfUser, y => { y.MapFrom(z => z.ApplicationUser.Name); })
                                                  .ForMember(x => x.Address, y => { y.MapFrom(z => z.Recipient.Address + ", " + z.Recipient.District.district + ", " + z.Recipient.City.city); })
                                                  //.ForMember(x => x.ListBook, y => { y.MapFrom(z => z.OrderItems.Select(y => new { y.Book.NameBook, y.Quantity, y.Price })); })
                                                  .ForMember(x => x.TamTinh, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price)); })
                                                  .ForMember(x => x.Discount, y => { y.MapFrom(z => (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100)); });
                                                  //.ForMember(x => x.Total, y => { y.MapFrom(z => z.OrderItems.Sum(y => y.Price) - (decimal)((z.OrderItems.Sum(y => y.Price) * z.Coupon.Discount) / 100) + z.ShippingFee); });
            #endregion

            #region OrderItem
            CreateMap<OrderItems, OrderItemForListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                                       .ForMember(x => x.Price, y => { y.MapFrom(z => z.Book.Price); })
                                                       .ForMember(x => x.SubTotal, y => { y.MapFrom(z => z.Price); });
            CreateMap<OrderItems, OrderItemForUserListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); });
                                                       
            #endregion

            #region Recipient
            CreateMap<RecipientForCreateDto, Recipient>().ForMember(x => x.RecipientID, opt => opt.Ignore())
                                                        .ForMember(x => x.Email, opt => opt.Ignore());
            CreateMap<Recipient, RecipientForUserListDto>().ForMember(x => x.city, y => { y.MapFrom(z => z.City.city); })
                                                           .ForMember(x => x.district, y => { y.MapFrom(z => z.District.district); });
            CreateMap<Recipient,RecipientForDefaultDto>().ForMember(x => x.Fee, y => { y.MapFrom(z => z.District.Fee); })
                                                         .ForMember(x => x.city, y => { y.MapFrom(z => z.City.city); })
                                                         .ForMember(x => x.district, y => { y.MapFrom(z => z.District.district); });
            CreateMap<Recipient, RecipientForUserDetailDto>().ForMember(x => x.Fee, y => { y.MapFrom(z => z.District.Fee); })
                                                         .ForMember(x => x.city, y => { y.MapFrom(z => z.City.city); })
                                                         .ForMember(x => x.district, y => { y.MapFrom(z => z.District.district); });
            CreateMap<RecipientForUpdateDto, Recipient>().ForMember(x => x.RecipientID, opt => opt.Ignore())
                                                        .ForMember(x => x.Email, opt => opt.Ignore())
                                                        .ForMember(x => x.CityID, opt => opt.Ignore())
                                                        .ForMember(x => x.DistrictID, opt => opt.Ignore())
                                                        .ForMember(x => x.Name, opt => opt.Ignore())
                                                        .ForMember(x => x.Address, opt => opt.Ignore())
                                                        .ForMember(x => x.Phone, opt => opt.Ignore())
                                                        .ForMember(x => x.Status, opt => opt.Ignore());
            #endregion

            #region Contact
            CreateMap<Contact, ContactForListDto>().ForMember(x => x.Status, y => { y.MapFrom(z => z.Status.AsString(EnumFormat.Description)); });
            CreateMap<ContactForCreateDto, Contact>().ForMember(x => x.ContactID, opt => opt.Ignore());
            CreateMap<ContactForUpdateDto, Contact>().ForMember(x => x.ContactID, opt => opt.Ignore())
                                                     .ForMember(x => x.Name, opt => opt.Ignore())
                                                     .ForMember(x => x.Email, opt => opt.Ignore())
                                                     .ForMember(x => x.Date, opt => opt.Ignore())
                                                     .ForMember(x => x.Message, opt => opt.Ignore())
                                                     .ForMember(x => x.Phone, opt => opt.Ignore());
            #endregion
        }
    }
}
