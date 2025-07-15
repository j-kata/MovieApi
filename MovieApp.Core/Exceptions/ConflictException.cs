namespace MovieApp.Core.Exceptions;

public class ConflictException(string message, string title = "Conflict")
    : DefaultException(message, title)
{ }