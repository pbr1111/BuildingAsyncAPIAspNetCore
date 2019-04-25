using AutoMapper;
using System.Collections.Generic;

namespace Books.API.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Entities.Book, Entities.DTO.Book>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));

            CreateMap<Entities.DTO.BookForCreation, Entities.Book>();

            CreateMap<Entities.Book, Entities.DTO.BookWithCovers>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));

            CreateMap<IEnumerable<Entities.DTO.BookCover>, Entities.DTO.BookWithCovers>()
                .ForMember(dest => dest.BookCovers, opt => opt.MapFrom(src => src));
        }
    }
}
