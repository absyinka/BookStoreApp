using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Author;
using BookStoreApp.API.DTOs.Book;

namespace BookStoreApp.API.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<CreateAuthorDto, Author>().ReverseMap();
        CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
        CreateMap<UpdateAuthorDto, Author>().ReverseMap();

        //Book Map Configuration
        CreateMap<BookCreateDto, Book>().ReverseMap();
        CreateMap<BookReadOnlyDto, Book>().ReverseMap();
        CreateMap<BookUpdateDto,  Book>().ReverseMap();
    }
}
