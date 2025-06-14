﻿using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Features.Commands.AddBook;
using Application.Features.Commands.UpdateBook;

namespace Application.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src => src.BookCategories.Select(bc => bc.Category.Name).ToList()));

            CreateMap<BookDTO, Book>()
                .ForMember(dest => dest.BookCategories, opt => opt.MapFrom(src => src.CategoryNames.Select(name => new BookCategory { Category = new Category { Name = name } }).ToList()));

            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<AddBookDTO, AddBookCommand>();

            CreateMap<UpdateBookDTO, UpdateBookCommand>();
        }
    }
}
