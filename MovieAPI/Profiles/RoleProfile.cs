using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MovieAPI.Models.Dtos.Actor;
using MovieAPI.Models.Dtos.Movie;
using MovieAPI.Models.Entities;

namespace MovieAPI.Profiles
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