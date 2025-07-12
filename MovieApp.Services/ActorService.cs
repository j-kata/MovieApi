using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Entities;

namespace MovieApp.Services;

public class ActorService(IUnitOfWork uow, IMapper mapper) : IActorService
{
    public async Task<IEnumerable<ActorDto>> GetActorsAsync(string? name)
    {
        var actors = await uow.Actors.GetActorsAsync(name);
        return mapper.Map<IEnumerable<ActorDto>>(actors);
    }

    public async Task<ActorDto> GetActorAsync(int id)
    {
        var actor = await uow.Actors.GetByIdAsync(id);
        if (actor == null)
            return null!; // TODO: throw exception;

        return mapper.Map<ActorDto>(actor);
    }

    public async Task PutActorAsync(int id, ActorUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return; // TODO: throw exception;

        var actor = await uow.Actors.GetByIdAsync(id, true);

        if (actor is null)
            return; // TODO: throw exception;

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