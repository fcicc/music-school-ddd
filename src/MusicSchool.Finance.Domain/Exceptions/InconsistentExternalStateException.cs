namespace MusicSchool.Finance.Domain.Exceptions;

public class InconsistentExternalStateException : Exception
{
    public InconsistentExternalStateException(string message)
        : base(message)
    {
    }
}
