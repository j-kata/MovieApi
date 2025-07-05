using AutoMapper;
using MovieApi.Models.Dtos.Movie;
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

            CreateMap<Movie, MovieDetailDto>()
                .IncludeBase<Movie, MovieDto>()
                .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.MovieDetail.Synopsis))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.MovieDetail.Language))
                .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.MovieDetail.Budget));

            CreateMap<MovieCreateDto, Movie>()
                // .ForMember(dest => dest.MovieDetails, opt => opt.MapFrom(src => src))
                .ForPath(dest => dest.MovieDetail.Budget, opt => opt.MapFrom(src => src.Budget))
                .ForPath(dest => dest.MovieDetail.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForPath(dest => dest.MovieDetail.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => new TimeSpan(src.Hours, src.Minutes, 0)));

            CreateMap<MovieUpdateDto, Movie>()
                .IncludeBase<MovieCreateDto, Movie>();
        }
    }
}