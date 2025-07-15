using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Role;
using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Services.Extensions;
using MovieApp.Core.Exceptions;

namespace MovieApp.Services;

public class RoleService(IUnitOfWork uow, IMapper mapper) : IRoleService
{
    public async Task<PagedResult<ActorWithRoleDto>> GetMovieActorsAsync(PageParameters parameters, int movieId)
    {
        if (!await uow.Movies.AnyByIdAsync(movieId))
            throw new NotFoundException<Movie>(movieId);

        var result = await uow.Roles.GetMovieRolesAsync(parameters, movieId);
        return result.Map<Role, ActorWithRoleDto>(mapper);
    }

    public async Task PostMovieActorAsync(int movieId, RoleCreateDto createDto)
    {
        var movieMissing = !await uow.Movies.AnyByIdAsync(movieId);
        var actorMissing = !await uow.Actors.AnyByIdAsync(createDto.ActorId);

        if (movieMissing)
            throw new NotFoundException<Movie>(movieId);
        if (actorMissing)
            throw new NotFoundException<Actor>(createDto.ActorId);

        if (await uow.Roles.AnyAsync(role => role.MovieId == movieId && role.ActorId == createDto.ActorId))
            throw new ConflictException($"Actor {createDto.ActorId} has already a role in movie {movieId}");

        var role = mapper.Map<Role>(createDto);
        role.MovieId = movieId;

        uow.Roles.Add(role);
        await uow.CompleteAsync();
    }
}