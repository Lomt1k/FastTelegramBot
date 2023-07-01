using System.Runtime.CompilerServices;

namespace FastTelegramBot;

/// <summary>
/// Extension Methods
/// </summary>
internal static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T ThrowIfNull<T>(this T? value, [CallerArgumentExpression(nameof(value))] string? parameterName = default)
    {
        return value ?? throw new ArgumentNullException(parameterName);
    }
}
