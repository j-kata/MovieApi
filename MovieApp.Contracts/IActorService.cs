using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Review;

namespace MovieApp.Contracts;

public interface IActorService
{
    public Task<IEnumerable<ActorDto>> GetActorsAsync(string? name);
    public Task<ActorDto> GetActorAsync(int id);
    public Task PutActorAsync(int id, ActorUpdateDto updateDto);
    public Task<ActorDto> PostActorAsync(ActorCreateDto createDto);
}