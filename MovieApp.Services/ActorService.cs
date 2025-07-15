using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Services.Extensions;
using MovieApp.Core.Exceptions;

namespace MovieApp.Services;

public class ActorService(IUnitOfWork uow, IMapper mapper) : IActorService
{
    public async Task<PagedResult<ActorDto>> GetActorsAsync(PageParameters parameters, string? name)
    {
        var result = await uow.Actors.GetActorsAsync(parameters, name);
        return result.Map<Actor, ActorDto>(mapper);
    }

    public async Task<ActorDto> GetActorAsync(int id)
    {
        var actor = await uow.Actors.GetByIdAsync(id)
            ?? throw new NotFoundException<Actor>(id);
        return mapper.Map<ActorDto>(actor);
    }

    public async Task PutActorAsync(int id, ActorUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            throw new BadRequestException("Id in URL does not match Id in body");

        var actor = await uow.Actors.GetByIdAsync(id, true)
            ?? throw new NotFoundException<Actor>(id);

        mapper.Map(updateDto, actor);
        await uow.CompleteAsync();
    }

    public async Task<ActorDto> PostActorAsync(ActorCreateDto createDto)
    {
        var actor = mapper.Map<Actor>(createDto);

        uow.Actors.Add(actor);
        await uow.CompleteAsync();

        return mapper.Map<ActorDto>(actor);
    }
}