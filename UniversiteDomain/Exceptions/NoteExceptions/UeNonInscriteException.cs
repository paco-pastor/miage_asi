namespace UniversiteDomain.Exceptions.NoteExceptions;

public class UeNonInscriteException : Exception
{
    public UeNonInscriteException() : base() { }
    public UeNonInscriteException(string message) : base(message) { }
    public UeNonInscriteException(string message, Exception inner) : base(message, inner) { }
}