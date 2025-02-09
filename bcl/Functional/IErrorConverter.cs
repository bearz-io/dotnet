namespace Bearz;

public interface IErrorConverter
{
    string Name { get; }

    bool CanConvert(Exception ex);

    Error Convert(Exception ex);
}