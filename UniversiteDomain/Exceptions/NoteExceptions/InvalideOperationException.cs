namespace UniversiteDomain.Exceptions.NoteExceptions;

public class InvalideOperationException : Exception
{
    public InvalideOperationException() : base() { }
    public InvalideOperationException(string message) : base(message) { }
    public InvalideOperationException(string message, Exception inner) : base(message, inner) { }
}