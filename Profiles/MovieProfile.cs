using AutoMapper;
using Humanizer;

namespace MovieApi.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Models.Entities.Movie, Models.Dtos.MovieDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString(@"hh\:mm")));

            CreateMap<Models.Entities.Movie, Models.Dtos.MovieDetailsDto>()
                .IncludeBase<Models.Entities.Movie, Models.Dtos.MovieDto>();
            // .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.MovieDetails.Synopsis))
            // .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.MovieDetails.Language))
            // .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.MovieDetails.Budget));
        }
    }
}