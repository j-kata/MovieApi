using AutoMapper;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Profiles;

public class ActorProfile : Profile
{
    public ActorProfile()
    {
        CreateMap<Actor, ActorDto>();
        CreateMap<ActorCreateDto, Actor>();
        CreateMap<ActorUpdateDto, Actor>();
    }
}
