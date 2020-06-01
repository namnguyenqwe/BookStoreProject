using AutoMapper;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Dtos.Category;
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
            CreateMap<BookForDetailDto, Book>().ForMember(x => x.BookID, opt => opt.Ignore());
            CreateMap<Book, BookForListDto>().ForMember(x => x.Category, y => { y.MapFrom(z => z.Category.Category); })
                                            .ForMember(x => x.publisher, y => { y.MapFrom(z => z.Publisher.publisher); });
            CreateMap<CategoryDto,Categories>().ForMember(x => x.CategoryID, opt => opt.Ignore());
            CreateMap<Categories, CategoryForListDto>();
            CreateMap<Review, ReviewForListDto>().ForMember(x => x.NameBook, y => { y.MapFrom(z => z.Book.NameBook); })
                                           .ForMember(x => x.FullName, y => { y.MapFrom(z => z.ApplicationUser.FullName); });
        }
    }
}
