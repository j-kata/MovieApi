using AutoMapper;
using MovieApi.Models.Dtos.Actor;
using MovieApi.Models.Entities;

namespace MovieApi.Profiles
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