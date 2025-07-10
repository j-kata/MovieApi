using AutoMapper;
using MovieCore.Dtos.Review;
using MovieCore.Entities;

namespace MovieData.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewCreateDto, Review>();
        }
    }
}