using AutoMapper;
using MovieApi.Models;
using MovieApi.Models.Dtos;
using MovieApi.Models.Entities;

namespace MovieApi.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString(@"hh\:mm")));

            CreateMap<Movie, MovieDetailsDto>()
                .IncludeBase<Movie, MovieDto>()
                .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.MovieDetails.Synopsis))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.MovieDetails.Language))
                .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.MovieDetails.Budget));

            CreateMap<MovieCreateDto, MovieDetails>();

            CreateMap<MovieCreateDto, Movie>()
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
                .ForMember(dest => dest.MovieDetails, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => new TimeSpan(src.Hours, src.Minutes, 0)));
        }
    }
}