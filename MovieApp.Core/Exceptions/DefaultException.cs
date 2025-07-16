namespace MovieApp.Core.Exceptions;

public abstract class ApiException(string message, string title)
    : Exception(message)
{
    public string Title { get; } = title;
}

