namespace Bearz;

public readonly struct Void
{
    public static Void Value { get; } = default;

    public static bool IsVoid(object? value)
    {
        return value is Void;
    }

    public static bool IsNil(object? value)
    {
        switch (value)
        {
            case null:
            case DBNull:
            case Void:
                return true;
            case IOption option:
                return !option.HasValue;
            default:
                return false;
        }
    }
}