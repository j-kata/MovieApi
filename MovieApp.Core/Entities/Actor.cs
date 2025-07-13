using MovieApp.Core.Contracts;

namespace MovieApp.Core.Entities;

public class Actor : IHasId
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int BirthYear { get; set; }
    public ICollection<Role> Roles { get; set; } = [];
}
