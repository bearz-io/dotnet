namespace Bearz.OS;

public class EnvironmentException : Exception
{
    public EnvironmentException()
    {
    }

    public EnvironmentException(string message)
        : base(message)
    {
    }

    public EnvironmentException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}