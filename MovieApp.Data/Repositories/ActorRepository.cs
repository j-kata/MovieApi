using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class ActorRepository(MovieContext context)
    : BaseRepository<Actor>(context), IActorRepository
{

}