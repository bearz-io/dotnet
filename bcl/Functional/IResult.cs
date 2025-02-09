namespace Bearz;

public interface IResult
{
    bool HasValue { get; }

    object Value { get; }

    object Error { get; }

    bool TryGet(out object? value);

    bool TryGetError(out object? error);
}

public interface IResult<TValue, TError> : IResult, IEquatable<IResult<TValue, TError>>
    where TValue : notnull
    where TError : notnull
{
    new TValue Value { get; }

    new TError Error { get; }

    bool TryGet(out TValue? value);

    bool TryGetError(out TError? error);
}