namespace Bearz;

public interface IOption
{
    bool HasValue { get; }

    object Value { get; }

    bool TryGet(out object? value);
}

public interface IOption<T> : IOption, IEquatable<IOption<T>>
    where T : notnull
{
    new T Value { get; }

    bool TryGet(out T? value);
}