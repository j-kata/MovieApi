using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MovieApi.Models.Dtos.Movie;
using MovieApi.Models.Dtos.Review;
using MovieApi.Models.Entities;

namespace MovieApi.Profiles
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