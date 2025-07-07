using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MovieApi.Models.Dtos.Actor;
using MovieApi.Models.Dtos.Movie;
using MovieApi.Models.Entities;

namespace MovieApi.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, ActorWithRoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Actor.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Actor.Name))
                .ForMember(dest => dest.BirthYear, opt => opt.MapFrom(src => src.Actor.BirthYear))
                .ForMember(dest => dest.Role, obj => obj.MapFrom(src => src.Title));

            CreateMap<RoleCreateDto, Role>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Role));
        }
    }
}