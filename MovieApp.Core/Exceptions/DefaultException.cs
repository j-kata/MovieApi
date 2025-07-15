namespace MovieApp.Core.Exceptions;

public abstract class DefaultException(string message, string title)
    : Exception(message)
{
    public string Title { get; } = title;
}

