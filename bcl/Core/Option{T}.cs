using System.Diagnostics.CodeAnalysis;

namespace Bearz;

public class Option<T> : IOption<T>, IEquatable<T>, IEquatable<Option<T>>
    where T : notnull
{
    private readonly T? value;

    public Option()
    {
        this.HasValue = false;
    }

    public Option(T value)
    {
        this.value = value;
        this.HasValue = true;
    }

    public static Option<T> None { get; } = new();

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

    public static implicit operator T(Option<T> option)
        => option.Value;

    public static implicit operator Option<T>(T? value)
        => value is null ? new() : new(value);

    public static implicit operator Option<T>(ValueType value)
        => new();

    public static implicit operator Task<Option<T>>(Option<T> option)
        => Task.FromResult(option);

    public static implicit operator ValueTask<Option<T>>(Option<T> option)
        => new(option);

    public static bool operator ==(Option<T> left, Option<T> right)
        => left.Equals(right);

    public static bool operator !=(Option<T> left, Option<T> right)
        => !left.Equals(right);

    public static Option<T> From(T? value)
        => value is null ? new() : new(value);

    public static Option<T> Some(T value)
        => new(value);

    public bool Equals(IOption<T>? other)
    {
        if (other is null)
            return !this.HasValue;

        if (!this.HasValue)
            return !other.HasValue;

        return this.value!.Equals(other.Value);
    }

    public bool Equals(Option<T>? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (other is null)
            return !this.HasValue;

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
        if (obj is Option<T> other)
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

    public Option<T> Inspect(Action<T> action)
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

    public Option<TOther> Map<TOther>(Func<T, TOther> map)
        where TOther : notnull
    {
        if (this.HasValue)
            return new(map(this.value!));

        return new();
    }

    public Option<TOther> MapOrDefault<TOther>(Func<T, TOther> map, TOther defaultValue)
        where TOther : notnull
    {
        if (this.HasValue)
            return new(map(this.value!));

        return new(defaultValue);
    }

    public Option<TOther> MapOrDefault<TOther>(Func<T, TOther> map, Func<TOther> defaultValue)
        where TOther : notnull
    {
        if (this.HasValue)
            return new(map(this.value!));

        return new(defaultValue());
    }

    public Option<T> OrDefault(Option<T> other)
    {
        if (this.HasValue)
            return this;

        return other;
    }

    public Option<T> OrDefault(Func<Option<T>> other)
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