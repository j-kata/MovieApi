using MovieCore.Dtos.Actor;

namespace MovieCore.Dtos.Movie
{
    public class MovieWithActorsDto : MovieDto
    {
        public List<ActorWithRoleDto> Actors { get; set; } = [];
    }
}