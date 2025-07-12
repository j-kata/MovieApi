using MovieApp.Core.Dtos.Actor;

namespace MovieApp.Core.Dtos.Movie;

public class MovieWithActorsDto : MovieDto
{
    public List<ActorWithRoleDto> Actors { get; set; } = [];
}
