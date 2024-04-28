namespace BlazorCms.BuildingBlocks;

/// <summary>
/// Convey meaning through color.
/// </summary>
public enum CmsThemeColors
{
    /// <summary>
    /// No color.
    /// </summary>
    None,

    /// <summary>
    /// Primary color.
    /// </summary>
    Primary,

    /// <summary>
    /// Secondary color.
    /// </summary>
    Secondary,

    /// <summary>
    /// Info color.
    /// </summary>
    Info,

    /// <summary>
    /// Success color.
    /// </summary>
    Success,

    /// <summary>
    /// Warning color.
    /// </summary>
    Warning,

    /// <summary>
    /// Danger color.
    /// </summary>
    Danger,

    /// <summary>
    /// Dark color.
    /// </summary>
    Dark,

    /// <summary>
    /// Light color.
    /// </summary>
    Light,

    /// <summary>
    /// Black color.
    /// </summary>
    Black,

    /// <summary>
    /// White color.
    /// </summary>
    White,
}

/// <summary>
/// Dark or Light modes.
/// </summary>
public enum CmsThemeMode
{
    /// <summary>
    /// No mode.
    /// </summary>
    None,

    /// <summary>
    /// Dark mode.
    /// </summary>
    Dark,

    /// <summary>
    /// Light mode.
    /// </summary>
    Light,
}

public record class CmsRadioItem<T>(T? Value, string? Label = null, string? Icon = null);

