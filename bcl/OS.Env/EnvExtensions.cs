using System.Diagnostics.CodeAnalysis;

namespace Bearz.OS;

internal static class EnvExtensions
{
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? value)
        => string.IsNullOrWhiteSpace(value);
}