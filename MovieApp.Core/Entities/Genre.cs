using MovieApp.Core.Contracts;

namespace MovieApp.Core.Entities;

public class Genre : IHasId
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Movie> Movies { get; set; } = [];
}

