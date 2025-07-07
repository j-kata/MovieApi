using MovieApi.Models.Dtos.Actor;

namespace MovieApi.Models.Dtos.Movie
{
    public class MovieWithActorsDto : MovieDto
    {
        public List<ActorWithRoleDto> Actors { get; set; } = [];
    }
}