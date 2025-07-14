using AutoMapper;
using MovieApp.Core.Dtos.Reports;
using MovieApp.Core.ValueObjects;

namespace MovieApp.Data.Profiles;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<TopMoviesByGenre, TopMoviesByGenreDto>();
        CreateMap<MovieWithRating, MovieWithRatingDto>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => Math.Round(src.Rating, 2)));
        CreateMap<GenreWithRating, GenreWithRatingDto>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => Math.Round(src.Rating, 2)));
        CreateMap<ActorWithRolesCount, ActorWithRolesCountDto>();
        CreateMap<MovieWithReviewsCount, MovieWithReviewsCountDto>();
        CreateMap<GenreWithMoviesCount, GenreWithMoviesCountDto>();
    }
}