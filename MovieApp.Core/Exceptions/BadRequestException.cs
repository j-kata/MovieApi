namespace MovieApp.Core.Exceptions;

public class BadRequestException(string message, string title = "Conflict")
    : DefaultException(message, title)
{ }