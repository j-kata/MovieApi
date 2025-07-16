namespace MovieApp.Core.Exceptions;

public interface INotFoundException // to check type without generic
{ }

public class NotFoundException<T> : ApiException, INotFoundException
{
    private const string DefaultTitle = "Object not found";

    public NotFoundException(string message, string title = DefaultTitle)
        : base(message, title) { }

    public NotFoundException(string propName, int id, string title = DefaultTitle)
        : base($"{typeof(T).Name} with {propName} {id} was not found", title) { }

    public NotFoundException(int id, string title = DefaultTitle)
        : base($"{typeof(T).Name} with Id {id} was not found", title) { }
}
