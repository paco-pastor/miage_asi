namespace UniversiteDomain.Exceptions.ParcoursExceptions;

public class OutRangeAnneeFormationException : Exception
{
    public OutRangeAnneeFormationException() : base() { }
    public OutRangeAnneeFormationException(string message) : base(message) { }
    public OutRangeAnneeFormationException(string message, Exception inner) : base(message, inner) { }
}