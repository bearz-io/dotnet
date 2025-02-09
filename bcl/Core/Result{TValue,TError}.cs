// ReSharper disable ParameterHidesMember
namespace Bearz;

public class Result<TValue, TError> : IResult<TValue, TError>,
    IEquatable<Result<TValue, TError>>
    where TValue : notnull
    where TError : notnull
{
    private readonly TValue? value;

    private readonly TError? error;

    public Result()
    {
        this.HasValue = false;
        this.value = default;
        this.error = default;
    }

    public Result(TValue value)
    {
        this.HasValue = true;
        this.value = value;
        this.error = default;
    }

    public Result(TError error)
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

    public static implicit operator Result<TValue, TError>(TValue value)
        => new(value);

    public static implicit operator TValue(Result<TValue, TError> result)
        => result.Value;

    public static implicit operator Task<Result<TValue, TError>>(Result<TValue, TError> result)
        => Task.FromResult(result);

    public static implicit operator ValueTask<Result<TValue, TError>>(Result<TValue, TError> result)
        => new(result);

    public static bool operator ==(Result<TValue, TError> left, Result<TValue, TError> right)
        => left.Equals(right);

    public static bool operator !=(Result<TValue, TError> left, Result<TValue, TError> right)
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

    public bool Equals(Result<TValue, TError>? other)
    {
        if (other is null)
            return false;

        if (this.HasValue != other.HasValue)
            return false;

        if (this.HasValue)
            return this.value!.Equals(other.value);

        return this.error!.Equals(other.error);
    }

    public Result<TValue, TError> Inspect(Action<TValue> action)
    {
        if (this.HasValue)
            action(this.value!);

        return this;
    }

    public Result<TValue, TError> InspectError(Action<TError> action)
    {
        if (!this.HasValue)
            action(this.error!);

        return this;
    }

    public Result<TOther, TError> Map<TOther>(Func<TValue, TOther> map)
        where TOther : notnull
        => this.HasValue ? new(map(this.value!)) : new(this.error!);

    public Result<TOther, TError> MapOrDefault<TOther>(Func<TValue, TOther> map, TOther defaultValue)
        where TOther : notnull
        => this.HasValue ? new(map(this.value!)) : new(defaultValue);

    public Result<TOther, TError> MapOrDefault<TOther>(Func<TValue, TOther> map, Func<TOther> defaultValue)
        where TOther : notnull
        => this.HasValue ? new(map(this.value!)) : new(defaultValue());

    public Result<TValue, TOther> MapError<TOther>(Func<TError, TOther> map)
        where TOther : notnull
        => this.HasValue ? new(this.value!) : new(map(this.error!));

    public Result<TValue, TOther> MapErrorOrDefault<TOther>(Func<TError, TOther> map, TOther defaultValue)
        where TOther : notnull
        => this.HasValue ? new(this.value!) : new(defaultValue);

    public Result<TValue, TOther> MapErrorOrDefault<TOther>(Func<TError, TOther> map, Func<TOther> defaultValue)
        where TOther : notnull
        => this.HasValue ? new(this.value!) : new(defaultValue());

    public Result<TValue, TError> Or(TValue other)
        => this.HasValue ? this : new(other);

    public Result<TValue, TError> Or(Func<TValue> other)
        => this.HasValue ? this : new(other());

    public TValue OrDefault(TValue other)
        => this.HasValue ? this.value! : other;

    public TValue OrDefault(Func<TValue> other)
        => this.HasValue ? this.value! : other();

    public TError OrDefaultError(TError other)
        => !this.HasValue ? this.error! : other;

    public TError OrDefaultError(Func<TError> other)
        => !this.HasValue ? this.error! : other();

    public Result<TValue, TError> OrError(TError error)
        => !this.HasValue ? this : new(error);

    public Result<TValue, TError> OrError(Func<TError> error)
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