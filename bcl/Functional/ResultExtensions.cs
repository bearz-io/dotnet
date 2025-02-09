namespace Bearz;

public static class ResultExtensions
{
    public static bool IsOk<TValue, TError>(this IResult<TValue, TError> r)
        where TValue : notnull
        where TError : notnull
        => r.HasValue;

    public static bool IsOkAnd<TValue, TError>(this IResult<TValue, TError> r, Func<TValue, bool> predicate)
        where TValue : notnull
        where TError : notnull
        => r.HasValue && predicate(r.Value);

    public static bool IsError<TValue, TError>(this IResult<TValue, TError> r)
        where TValue : notnull
        where TError : notnull
        => !r.HasValue;

    public static bool IsErrorAnd<TValue, TError>(this IResult<TValue, TError> r, Func<TError, bool> predicate)
        where TValue : notnull
        where TError : notnull
        => !r.HasValue && predicate(r.Error);

    public static TValue Expect<TValue, TError>(this IResult<TValue, TError> r, string message)
        where TValue : notnull
        where TError : notnull
    {
        if (r.HasValue)
            return r.Value;

        throw new ResultException(message);
    }

    public static TError ExpectError<TValue, TError>(this IResult<TValue, TError> r, string message)
        where TValue : notnull
        where TError : notnull
    {
        if (!r.HasValue)
            return r.Error;

        throw new ResultException(message);
    }
}