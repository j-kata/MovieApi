using MovieAPI.Models.Dtos.Actor;
using MovieAPI.Models.Dtos.Review;

namespace MovieAPI.Models.Dtos.Movie
{
    public class MovieDetailDto : MovieDto
    {
        public string? Synopsis { get; set; }
        public string? Language { get; set; }
        public int? Budget { get; set; }
        public List<ReviewDto> Reviews { get; set; } = [];
        public List<ActorWithRoleDto> Actors { get; set; } = [];
    }
}