namespace MovieApp.Core.Contracts;

public interface IHasId : IEntity
{
    int Id { get; set; }
}