// ReSharper disable ParameterHidesMember
namespace Bearz;

public readonly struct ValueResult<TValue, TError> : IResult<TValue, TError>,
    IEquatable<ValueResult<TValue, TError>>
    where TValue : notnull
    where TError : notnull
{
    private readonly TValue? value;

    private readonly TError? error;

    public ValueResult()
    {
        this.HasValue = false;
        this.value = default;
        this.error = default;
    }

    public ValueResult(TValue value)
    {
        this.HasValue = true;
        this.value = value;
        this.error = default;
    }

    public ValueResult(TError error)
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

    public TError Error
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

    public static implicit operator ValueResult<TValue, TError>(TValue value)
        => new(value);

    public static implicit operator TValue(ValueResult<TValue, TError> result)
        => result.Value;

    public static implicit operator Task<ValueResult<TValue, TError>>(ValueResult<TValue, TError> result)
        => Task.FromResult(result);

    public static implicit operator ValueTask<ValueResult<TValue, TError>>(ValueResult<TValue, TError> result)
        => new(result);

    public static bool operator ==(ValueResult<TValue, TError> left, ValueResult<TValue, TError> right)
        => left.Equals(right);

    public static bool operator !=(ValueResult<TValue, TError> left, ValueResult<TValue, TError> right)
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

        if (obj is Result<TValue, TError> other2)
            return this.Equals(other2);

        if (obj is IResult<TValue, TError> other)
            return this.Equals(other);

        return false;
    }

    public bool Equals(IResult<TValue, TError>? other)
    {
        if (other is null)
            return !this.HasValue;

        if (!this.HasValue)
            return !other.HasValue;

        return this.value!.Equals(other.Value);
    }

    public bool Equals(ValueResult<TValue, TError> other)
    {
        if (this.HasValue != other.HasValue)
            return false;

        if (this.HasValue)
            return this.value!.Equals(other.value);

        return this.error!.Equals(other.error);
    }

    public ValueResult<TValue, TError> Inspect(Action<TValue> action)
    {
        if (this.HasValue)
            action(this.value!);

        return this;
    }

    public ValueResult<TValue, TError> InspectError(Action<TError> action)
    {
        if (!this.HasValue)
            action(this.error!);

        return this;
    }

    public ValueResult<TOther, TError> Map<TOther>(Func<TValue, TOther> map)
        where TOther : notnull
        => this.HasValue ? new(map(this.value!)) : new(this.error!);

    public ValueResult<TOther, TError> MapOrDefault<TOther>(Func<TValue, TOther> map, TOther defaultValue)
        where TOther : notnull
        => this.HasValue ? new(map(this.value!)) : new(defaultValue);

    public ValueResult<TOther, TError> MapOrDefault<TOther>(Func<TValue, TOther> map, Func<TOther> defaultValue)
        where TOther : notnull
        => this.HasValue ? new(map(this.value!)) : new(defaultValue());

    public ValueResult<TValue, TOther> MapError<TOther>(Func<TError, TOther> map)
        where TOther : notnull
        => this.HasValue ? new(this.value!) : new(map(this.error!));

    public ValueResult<TValue, TOther> MapErrorOrDefault<TOther>(Func<TError, TOther> map, TOther defaultValue)
        where TOther : notnull
        => this.HasValue ? new(this.value!) : new(defaultValue);

    public ValueResult<TValue, TOther> MapErrorOrDefault<TOther>(Func<TError, TOther> map, Func<TOther> defaultValue)
        where TOther : notnull
        => this.HasValue ? new(this.value!) : new(defaultValue());

    public ValueResult<TValue, TError> Or(TValue other)
        => this.HasValue ? this : new(other);

    public ValueResult<TValue, TError> Or(Func<TValue> other)
        => this.HasValue ? this : new(other());

    public TValue OrDefault(TValue other)
        => this.HasValue ? this.value! : other;

    public TValue OrDefault(Func<TValue> other)
        => this.HasValue ? this.value! : other();

    public TError OrDefaultError(TError other)
        => !this.HasValue ? this.error! : other;

    public TError OrDefaultError(Func<TError> other)
        => !this.HasValue ? this.error! : other();

    public ValueResult<TValue, TError> OrError(TError error)
        => !this.HasValue ? this : new(error);

    public ValueResult<TValue, TError> OrError(Func<TError> error)
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

    public bool TryGetError(out TError? error)
    {
        error = this.error;
        return !this.HasValue;
    }
}