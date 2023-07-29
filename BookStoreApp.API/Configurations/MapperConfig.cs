using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Author;
using BookStoreApp.API.DTOs.Book;
using BookStoreApp.API.DTOs.User;

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
        CreateMap<Book, BookReadOnlyDto>()
            .ForMember(a => a.AuthorName, d => d.MapFrom(map => $"{map.Author!.FirstName} {map.Author.LastName}"))
            .ReverseMap();
        CreateMap<Book, BookDetailDto>()
            .ForMember(a => a.AuthorName, d => d.MapFrom(map => $"{map.Author!.FirstName} {map.Author.LastName}"))
            .ReverseMap();
        CreateMap<BookUpdateDto, Book>().ReverseMap();

        //Authentication Map Configuration
        CreateMap<ApiUser, UserDto>().ReverseMap();
        CreateMap<ApiUser, LoginUserDto>().ReverseMap();
    }
}
