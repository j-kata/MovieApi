using MovieAPI.Models.Dtos.Actor;

namespace MovieAPI.Models.Dtos.Movie
{
    public class MovieWithActorsDto : MovieDto
    {
        public List<ActorWithRoleDto> Actors { get; set; } = [];
    }
}