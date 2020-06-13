using AutoMapper;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Dtos.Category;
using BookStoreProject.Dtos.Publisher;
using BookStoreProject.Dtos.Review;
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
            CreateMap<BookForUpdateDto, Book>().ForMember(x => x.QuantityOut, opt => opt.Ignore());
            CreateMap<Book, BookForListDto>().ForMember(x => x.Category, y => { y.MapFrom(z => z.Category.Category); })
                                            .ForMember(x => x.publisher, y => { y.MapFrom(z => z.Publisher.publisher); });

            CreateMap<Book, BookForUserDetailDto>().ForMember(x => x.Category, y => { y.MapFrom(z => z.Category.Category); })
                                            .ForMember(x => x.publisher, y => { y.MapFrom(z => z.Publisher.publisher); })
                                            .ForMember(x => x.ReviewCount, y => { y.MapFrom(z => z.Reviews.Count); })
                                            .ForMember(x => x.Rating, y => {
                                                       y.MapFrom(z => z.Reviews.Any() ? z.Reviews.Aggregate((a, b) => new Review { BookID = 0, Rating = a.Rating + b.Rating }).Rating / z.Reviews.Count : 0);
                                            })
                                            .ForMember(x => x.Reviews, y => { y.MapFrom(z => z.Reviews.Take(3)); }); 
            CreateMap<Book, BookForUserRelatedListDto>();
            CreateMap<Book, BookForUserSearchListDto>().ForMember(x => x.ReviewCount, y => { y.MapFrom(z => z.Reviews.Count); })
                                                        .ForMember(x => x.Rating, y => { 
                                                            y.MapFrom(z => z.Reviews.Any() ? z.Reviews.Aggregate((a,b) => new Review { BookID = 0,Rating = a.Rating + b.Rating}).Rating / z.Reviews.Count : 0); 
                                                        });

            CreateMap<CategoryDto,Categories>().ForMember(x => x.CategoryID, opt => opt.Ignore());
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

            CreateMap<Review, ReviewForListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                           .ForMember(x => x.FullName, y => { y.MapFrom(z => z.ApplicationUser.FullName); });
            CreateMap<Review, ReviewForUserListDto>().ForMember(x => x.AvatarLink, y => { y.MapFrom(z => z.ApplicationUser.AvatarLink); })
                                                    .ForMember(x => x.userName, y => { y.MapFrom(z => z.ApplicationUser.UserName); });

            CreateMap<Publisher, PublisherForSelectDto>();
            CreateMap<Publisher, PublisherForListDto>();
            CreateMap<PublisherDto, Publisher>();
        }
    }
}
