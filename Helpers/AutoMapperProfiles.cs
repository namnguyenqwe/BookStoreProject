﻿using AutoMapper;
using BookStoreProject.Dtos.Admin;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Dtos.Category;
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
        }
    }
}
