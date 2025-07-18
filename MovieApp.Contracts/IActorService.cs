using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Contracts;

public interface IActorService
{
    public Task<PagedResult<ActorDto>> GetActorsAsync(ActorParameters parameters);
    public Task<ActorDto> GetActorAsync(int id);
    public Task PutActorAsync(int id, ActorUpdateDto updateDto);
    public Task<ActorDto> PostActorAsync(ActorCreateDto createDto);
}