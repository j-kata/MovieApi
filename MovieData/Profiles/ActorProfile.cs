using AutoMapper;
using MovieCore.Dtos.Actor;
using MovieCore.Entities;

namespace MovieData.Profiles
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