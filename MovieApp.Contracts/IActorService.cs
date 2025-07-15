using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Contracts;

public interface IActorService
{
    public Task<PagedResult<ActorDto>> GetActorsAsync(PageParameters parameters, string? name);
    public Task<ActorDto> GetActorAsync(int id);
    public Task PutActorAsync(int id, ActorUpdateDto updateDto);
    public Task<ActorDto> PostActorAsync(ActorCreateDto createDto);
}