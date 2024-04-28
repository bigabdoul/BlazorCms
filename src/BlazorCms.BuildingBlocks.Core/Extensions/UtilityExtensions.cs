namespace BlazorCms.BuildingBlocks.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="CmsThemeColors"/> enumeration and other types.
/// </summary>
public static class UtilityExtensions
{
    /// <summary>
    /// Determines whether the specified nullable <paramref name="color"/> is a dark color.
    /// </summary>
    /// <param name="color">The color to analyze.</param>
    /// <returns></returns>
    public static bool IsDarkColor(this CmsThemeColors? color)
        => color != null && color.Value.IsDarkColor();

    /// <summary>
    /// Determines whether the specified <paramref name="color"/> is a dark color.
    /// </summary>
    /// <param name="color">The color to analyze.</param>
    /// <returns></returns>
    public static bool IsDarkColor (this CmsThemeColors color) => color switch
    {
        CmsThemeColors.Dark or CmsThemeColors.Primary or CmsThemeColors.Danger or CmsThemeColors.Success
        or CmsThemeColors.Black or CmsThemeColors.Secondary => true,
        _ => false
    };

    /// <summary>
    /// Returns the color string for the specified nullable <paramref name="color"/>.
    /// </summary>
    /// <param name="color">The color to analyze.</param>
    /// <param name="prefix">An optional prefix to prepend.</param>
    /// <returns></returns>
    public static string? GetBgColor(this CmsThemeColors? color, string? prefix = "bg-") 
        => color?.GetBgColor(prefix);

    /// <summary>
    /// Returns the color string for the specified <paramref name="color"/>.
    /// </summary>
    /// <param name="color">The color to analyze.</param>
    /// <param name="prefix">An optional prefix to prepend.</param>
    /// <returns></returns>
    public static string? GetBgColor(this CmsThemeColors color, string? prefix = "bg-")
        => color.GetColorName(prefix);

    /// <summary>
    /// Returns the color string for the specified nullable <paramref name="color"/>.
    /// </summary>
    /// <param name="color">The color to analyze.</param>
    /// <param name="prefix">An optional prefix to prepend.</param>
    /// <returns></returns>
    public static string? GetColorName(this CmsThemeColors? color, string? prefix = null)
        => color?.GetColorName(prefix);

    /// <summary>
    /// Returns the color string for the specified <paramref name="color"/>.
    /// </summary>
    /// <param name="color">The color to analyze.</param>
    /// <param name="prefix">An optional prefix to prepend.</param>
    /// <returns></returns>
    public static string? GetColorName(this CmsThemeColors color, string? prefix = null)
        => color switch { CmsThemeColors.None => null, _ => prefix + color.ToString().ToLower() };

    /// <summary>
    /// Returns an object of the specified type and whose value is equivalent to the specified object.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TValue? ChangeType<TValue>(this object? value)
        => (TValue?)Convert.ChangeType(value, typeof(TValue));
}
