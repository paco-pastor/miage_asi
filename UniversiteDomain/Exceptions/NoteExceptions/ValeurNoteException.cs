namespace UniversiteDomain.Exceptions.NoteExceptions;

public class ValeurNoteException : Exception
{
    public ValeurNoteException() : base() { }
    public ValeurNoteException(string message) : base(message) { }
    public ValeurNoteException(string message, Exception inner) : base(message, inner) { }
}