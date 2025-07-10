using AutoMapper;
using MovieAPI.Models.Dtos.Actor;
using MovieAPI.Models.Entities;

namespace MovieAPI.Profiles
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<Actor, ActorDto>();
            CreateMap<ActorCreateDto, Actor>();
            CreateMap<ActorUpdateDto, Actor>();
        }
    }
}