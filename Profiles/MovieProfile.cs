using AutoMapper;

namespace MovieApi.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Models.Entities.Movie, Models.Dtos.MovieDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString(@"hh\:mm")));
        }
    }
}