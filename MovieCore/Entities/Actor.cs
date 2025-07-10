namespace MovieCore.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BirthYear { get; set; }
        public ICollection<Role> Roles { get; set; } = [];
    }
}
