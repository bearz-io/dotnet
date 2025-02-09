// ReSharper disable ParameterHidesMember
namespace Bearz;

public readonly struct ValueResult<TValue> : IResult<TValue, Error>,
    IEquatable<ValueResult<TValue>>
    where TValue : notnull
{
    private readonly TValue? value;

    private readonly Error? error;

    public ValueResult()
    {
        this.HasValue = false;
        this.value = default;
        this.error = new ResultException("No value was set");
    }

    public ValueResult(TValue value)
    {
        this.HasValue = true;
        this.value = value;
        this.error = default;
    }

    public ValueResult(Error error)
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

    public static implicit operator TValue(ValueResult<TValue> valueResult)
        => valueResult.Value;

    public static implicit operator Error(ValueResult<TValue> valueResult)
        => valueResult.Error;

    public static implicit operator ValueResult<TValue>(TValue value)
        => new(value);

    public static implicit operator ValueResult<TValue>(Error error)
        => new(error);

    public static implicit operator ValueResult<TValue>(Exception error)
        => new(error);

    public static implicit operator Task<ValueResult<TValue>>(ValueResult<TValue> valueResult)
        => Task.FromResult(valueResult);

    public static implicit operator ValueTask<ValueResult<TValue>>(ValueResult<TValue> valueResult)
        => new(valueResult);

    public static implicit operator ValueResult<TValue, Error>(ValueResult<TValue> valueResult)
        => valueResult.HasValue ? new(valueResult.Value) : new(valueResult.Error);

    public static implicit operator ValueResult<TValue>(ValueResult<TValue, Error> result)
        => result.HasValue ? new(result.Value) : new(result.Error);

    public static bool operator ==(ValueResult<TValue> left, ValueResult<TValue> right)
        => left.Equals(right);

    public static bool operator !=(ValueResult<TValue> left, ValueResult<TValue> right)
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

    public bool Equals(ValueResult<TValue> other)
    {
        if (this.HasValue != other.HasValue)
            return false;

        if (this.HasValue)
            return this.value!.Equals(other.value);

        return this.error!.Equals(other.error);
    }

    public ValueResult<TValue> Inspect(Action<TValue> action)
    {
        if (this.HasValue)
            action(this.value!);

        return this;
    }

    public ValueResult<TValue> InspectError(Action<Error> action)
    {
        if (!this.HasValue)
            action(this.error!);

        return this;
    }

    public ValueResult<TOther> Map<TOther>(Func<TValue, TOther> func)
        where TOther : notnull
        => this.HasValue ? new(func(this.value!)) : new(this.error!);

    public ValueResult<TOther> MapOrDefault<TOther>(Func<TValue, TOther> func, TOther defaultValue)
        where TOther : notnull
        => this.HasValue ? new(func(this.value!)) : new(defaultValue);

    public ValueResult<TOther> MapOrDefault<TOther>(Func<TValue, TOther> func, Func<TOther> defaultValue)
        where TOther : notnull
        => this.HasValue ? new(func(this.value!)) : new(defaultValue());

    public ValueResult<TValue, TError> MapError<TError>(Func<Error, TError> func)
        where TError : notnull
        => this.HasValue ? new(this.value!) : new(func(this.error!));

    public ValueResult<TValue, TError> MapErrorOrDefault<TError>(Func<Error, TError> func, TError defaultValue)
        where TError : notnull
        => this.HasValue ? new(this.value!) : new(defaultValue);

    public ValueResult<TValue, TError> MapErrorOrDefault<TError>(Func<Error, TError> func, Func<TError> defaultValue)
        where TError : notnull
        => this.HasValue ? new(this.value!) : new(defaultValue());

    public ValueResult<TValue> Or(TValue other)
        => this.HasValue ? this : new(other);

    public ValueResult<TValue> Or(Func<TValue> other)
        => this.HasValue ? this : new(other());

    public TValue OrDefault(TValue defaultValue)
        => this.HasValue ? this.value! : defaultValue;

    public TValue OrDefault(Func<TValue> defaultValue)
        => this.HasValue ? this.value! : defaultValue();

    public Error OrDefaultError(Error defaultError)
        => !this.HasValue ? this.error! : defaultError;

    public Error OrDefaultError(Func<Error> defaultError)
        => !this.HasValue ? this.error! : defaultError();

    public ValueResult<TValue> OrError(Error error)
        => !this.HasValue ? this : new(error);

    public ValueResult<TValue> OrError(Func<Error> error)
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