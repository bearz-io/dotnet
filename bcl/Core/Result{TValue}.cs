// ReSharper disable ParameterHidesMember
namespace Bearz;

public class Result<TValue> : IResult<TValue, Error>,
    IEquatable<Result<TValue>>
    where TValue : notnull
{
    private readonly TValue? value;

    private readonly Error? error;

    public Result()
    {
        this.HasValue = false;
        this.value = default;
        this.error = new ResultException("No value was set");
    }

    public Result(TValue value)
    {
        this.HasValue = true;
        this.value = value;
        this.error = default;
    }

    public Result(Error error)
    {
        this.HasValue = false;
        this.value = default;
        this.error = error;
    }

    public bool HasValue { get; }

    public TValue Value
    {
        get
        {
            if (this.HasValue)
                return this.value!;

#pragma warning disable S2372
            throw new ResultException("No value present");
        }
    }

    public Error Error
    {
        get
        {
            if (!this.HasValue)
                return this.error!;

#pragma warning disable S2372
            throw new ResultException("No error present");
        }
    }

    object IResult.Value => this.Value;

    object IResult.Error => this.Error;

    public static implicit operator TValue(Result<TValue> result)
        => result.Value;

    public static implicit operator Error(Result<TValue> result)
        => result.Error;

    public static implicit operator Result<TValue>(TValue value)
        => new(value);

    public static implicit operator Result<TValue>(Error error)
        => new(error);

    public static implicit operator Result<TValue>(Exception error)
        => new(error);

    public static implicit operator Task<Result<TValue>>(Result<TValue> result)
        => Task.FromResult(result);

    public static implicit operator ValueTask<Result<TValue>>(Result<TValue> result)
        => new(result);

    public static implicit operator Result<TValue, Error>(Result<TValue> result)
        => result.HasValue ? new(result.Value) : new(result.Error);

    public static implicit operator Result<TValue>(Result<TValue, Error> result)
        => result.HasValue ? new(result.Value) : new(result.Error);

    public static bool operator ==(Result<TValue> left, Result<TValue> right)
        => left.Equals(right);

    public static bool operator !=(Result<TValue> left, Result<TValue> right)
        => !left.Equals(right);

    public override int GetHashCode()
    {
        if (this.HasValue)
            return this.value!.GetHashCode();
        return this.error!.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return !this.HasValue;

        if (obj is Result<TValue, Error> other2)
            return this.Equals(other2);

        if (obj is IResult<TValue, Error> other)
            return this.Equals(other);

        return false;
    }

    public bool Equals(IResult<TValue, Error>? other)
    {
        if (other is null)
            return !this.HasValue;

        if (!this.HasValue)
            return !other.HasValue;

        return this.value!.Equals(other.Value);
    }

    public bool Equals(Result<TValue>? other)
    {
        if (other is null)
            return false;

        if (this.HasValue != other.HasValue)
            return false;

        if (this.HasValue)
            return this.value!.Equals(other.value);

        return this.error!.Equals(other.error);
    }

    public Result<TValue> Inspect(Action<TValue> action)
    {
        if (this.HasValue)
            action(this.value!);

        return this;
    }

    public Result<TValue> InspectError(Action<Error> action)
    {
        if (!this.HasValue)
            action(this.error!);

        return this;
    }

    public Result<TOther> Map<TOther>(Func<TValue, TOther> func)
        where TOther : notnull
        => this.HasValue ? new(func(this.value!)) : new(this.error!);

    public Result<TOther> MapOrDefault<TOther>(Func<TValue, TOther> func, TOther defaultValue)
        where TOther : notnull
        => this.HasValue ? new(func(this.value!)) : new(defaultValue);

    public Result<TOther> MapOrDefault<TOther>(Func<TValue, TOther> func, Func<TOther> defaultValue)
        where TOther : notnull
        => this.HasValue ? new(func(this.value!)) : new(defaultValue());

    public Result<TValue, TError> MapError<TError>(Func<Error, TError> func)
        where TError : notnull
        => this.HasValue ? new(this.value!) : new(func(this.error!));

    public Result<TValue, TError> MapErrorOrDefault<TError>(Func<Error, TError> func, TError defaultValue)
        where TError : notnull
        => this.HasValue ? new(this.value!) : new(defaultValue);

    public Result<TValue, TError> MapErrorOrDefault<TError>(Func<Error, TError> func, Func<TError> defaultValue)
        where TError : notnull
        => this.HasValue ? new(this.value!) : new(defaultValue());

    public Result<TValue> Or(TValue other)
        => this.HasValue ? this : new(other);

    public Result<TValue> Or(Func<TValue> other)
        => this.HasValue ? this : new(other());

    public TValue OrDefault(TValue defaultValue)
        => this.HasValue ? this.value! : defaultValue;

    public TValue OrDefault(Func<TValue> defaultValue)
        => this.HasValue ? this.value! : defaultValue();

    public Error OrDefaultError(Error defaultError)
        => !this.HasValue ? this.error! : defaultError;

    public Error OrDefaultError(Func<Error> defaultError)
        => !this.HasValue ? this.error! : defaultError();

    public Result<TValue> OrError(Error error)
        => !this.HasValue ? this : new(error);

    public Result<TValue> OrError(Func<Error> error)
        => !this.HasValue ? this : new(error());

    bool IResult.TryGet(out object? value)
    {
        var res = this.TryGet(out var v);
        value = v;
        return res;
    }

    bool IResult.TryGetError(out object? error)
    {
        var res = this.TryGetError(out var e);
        error = e;
        return res;
    }

    public bool TryGet(out TValue? value)
    {
        value = this.value;
        return this.HasValue;
    }

    public bool TryGetError(out Error? error)
    {
        error = this.error;
        return !this.HasValue;
    }
}