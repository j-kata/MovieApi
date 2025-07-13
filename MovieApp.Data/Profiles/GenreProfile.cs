using AutoMapper;
using MovieApp.Core.Dtos.Genre;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Profiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDto>();
    }
}