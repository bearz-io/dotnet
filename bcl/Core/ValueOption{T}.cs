using System.Diagnostics.CodeAnalysis;

namespace Bearz;

public readonly struct ValueOption<T> : IOption<T>, IEquatable<ValueOption<T>>
    where T : notnull
{
    private readonly T? value;

    public ValueOption()
    {
        this.HasValue = false;
    }

    public ValueOption(T value)
    {
        this.value = value;
        this.HasValue = true;
    }

    public static ValueOption<T> None { get; } = new();

    public bool HasValue { get; }

    public T Value
    {
        get
        {
            if (this.HasValue)
                return this.value!;

#pragma warning disable S2372
            throw new OptionException("No value present");
        }
    }

    object IOption.Value => this.Value;

    public static implicit operator T(ValueOption<T> option)
        => option.Value;

    public static implicit operator ValueOption<T>(T? value)
        => value is null ? new() : new(value);

    public static implicit operator ValueOption<T>(ValueTuple value)
        => new();

    public static implicit operator Task<ValueOption<T>>(ValueOption<T> option)
        => Task.FromResult(option);

    public static implicit operator ValueTask<Option<T>>(ValueOption<T> option)
        => new(option);

    public static bool operator ==(ValueOption<T> left, ValueOption<T> right)
        => left.Equals(right);

    public static bool operator !=(ValueOption<T> left, ValueOption<T> right)
        => !left.Equals(right);

    public static ValueOption<T> From(T? value)
        => value is null ? new() : new(value);

    public static ValueOption<T> Some(T value)
        => new(value);

    public bool Equals(IOption<T>? other)
    {
        if (other is null)
            return !this.HasValue;

        if (!this.HasValue)
            return !other.HasValue;

        return this.value!.Equals(other.Value);
    }

    public bool Equals(ValueOption<T> other)
    {
        if (!this.HasValue)
            return !other.HasValue;

        return this.value!.Equals(other.value);
    }

    public bool Equals(T? other)
    {
        if (!this.HasValue)
            return other is null;

        return this.value!.Equals(other);
    }

    public override bool Equals(object? obj)
    {
        if (obj is ValueOption<T> other)
            return this.Equals(other);

        if (obj is T otherValue)
            return this.Equals(otherValue);

        if (obj is IOption<T> otherOption)
            return this.Equals(otherOption);

        return false;
    }

    public T Expect(string message)
    {
        if (this.HasValue)
            return this.value!;

        throw new OptionException(message);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.HasValue, this.value);
    }

    public ValueOption<T> Inspect(Action<T> action)
    {
        if (this.HasValue)
            action(this.value!);

        return this;
    }

    public bool IsNone()
        => !this.HasValue;

    public bool IsNoneAnd(Func<T, bool> predicate)
    {
        if (this.HasValue)
            return false;

        return predicate(this.value!);
    }

    public bool IsSome()
        => this.HasValue;

    public bool IsSomeAnd(Func<T, bool> predicate)
    {
        if (!this.HasValue)
            return false;

        return predicate(this.value!);
    }

    public ValueOption<TOther> Map<TOther>(Func<T, TOther> map)
        where TOther : notnull
    {
        if (this.HasValue)
            return new(map(this.value!));

        return new();
    }

    public ValueOption<TOther> MapOrDefault<TOther>(Func<T, TOther> map, TOther defaultValue)
        where TOther : notnull
    {
        if (this.HasValue)
            return new(map(this.value!));

        return new(defaultValue);
    }

    public ValueOption<TOther> MapOrDefault<TOther>(Func<T, TOther> map, Func<TOther> defaultValue)
        where TOther : notnull
    {
        if (this.HasValue)
            return new(map(this.value!));

        return new(defaultValue());
    }

    public ValueOption<T> Or(ValueOption<T> other)
    {
        if (this.HasValue)
            return this;

        return other;
    }

    public ValueOption<T> Or(Func<ValueOption<T>> other)
    {
        if (this.HasValue)
            return this;

        return other();
    }

    public bool TryGet([NotNullWhen(true)] out T? value)
    {
        if (this.HasValue)
        {
            value = this.value!;
            return true;
        }

        value = default!;
        return false;
    }

    bool IOption.TryGet(out object? value)
    {
        var res = this.TryGet(out var v);
        value = v;
        return res;
    }
}