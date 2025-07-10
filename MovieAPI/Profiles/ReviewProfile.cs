using AutoMapper;
using MovieAPI.Models.Dtos.Review;
using MovieAPI.Models.Entities;

namespace MovieAPI.Profiles
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