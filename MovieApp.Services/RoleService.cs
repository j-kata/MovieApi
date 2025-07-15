using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Role;
using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Services.Extensions;

namespace MovieApp.Services;

public class RoleService(IUnitOfWork uow, IMapper mapper) : IRoleService
{
    public async Task<PagedResult<ActorWithRoleDto>> GetMovieActorsAsync(PageParameters parameters, int movieId)
    {
        if (!await uow.Movies.AnyByIdAsync(movieId))
            return null!; // TODO: throw exception

        var result = await uow.Roles.GetMovieRolesAsync(parameters, movieId);
        return result.Map<Role, ActorWithRoleDto>(mapper);
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