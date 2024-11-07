namespace UniversiteDomain.Exceptions.InscriptionExceptions;

public class DuplicateInscriptionException : Exception
{
    public DuplicateInscriptionException() : base() { }
    public DuplicateInscriptionException(string message) : base(message) { }
    public DuplicateInscriptionException(string message, Exception inner) : base(message, inner) { }
}