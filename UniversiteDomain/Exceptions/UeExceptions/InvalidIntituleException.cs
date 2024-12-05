namespace UniversiteDomain.Exceptions.UeExceptions;

public class InvalidIntituleException : Exception
{
    public InvalidIntituleException() : base() { }
    public InvalidIntituleException(string message) : base(message) { }
    public InvalidIntituleException(string message, Exception inner) : base(message, inner) { }
}