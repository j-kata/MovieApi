using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Role;
using MovieApp.Core.Entities;

namespace MovieApp.Services;

public class RoleService(IUnitOfWork uow, IMapper mapper) : IRoleService
{
    public async Task<IEnumerable<ActorWithRoleDto>> GetMovieActorsAsync(int movieId)
    {
        if (!await uow.Movies.AnyByIdAsync(movieId))
            return null!; // TODO: throw exception

        var roles = await uow.Roles.GetMovieRolesAsync(movieId);
        return mapper.Map<IEnumerable<ActorWithRoleDto>>(roles);
    }

    public async Task PostMovieActorAsync(int movieId, RoleCreateDto createDto)
    {
        var movieMissing = !await uow.Movies.AnyByIdAsync(movieId);
        var actorMissing = !await uow.Actors.AnyByIdAsync(createDto.ActorId);

        if (movieMissing || actorMissing)
            return;// TODO: throw exception

        if (await uow.Roles.AnyAsync(role => role.MovieId == movieId && role.ActorId == createDto.ActorId))
            return; // TODO: throw exception

        var role = mapper.Map<Role>(createDto);
        role.MovieId = movieId;

        uow.Roles.Add(role);
        await uow.CompleteAsync();
    }
}