using AutoMapper;
using MovieApp.Core.Dtos.Review;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Profiles
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