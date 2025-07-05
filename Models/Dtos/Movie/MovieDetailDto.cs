using MovieApi.Models.Dtos.Actor;
using MovieApi.Models.Dtos.Review;

namespace MovieApi.Models.Dtos.Movie
{
    public class MovieDetailDto : MovieDto
    {
        public string? Synopsis { get; set; }
        public string? Language { get; set; }
        public int? Budget { get; set; }
        public List<ReviewDto> Reviews { get; set; } = [];
        public List<ActorDto> Actors { get; set; } = [];
    }
}