namespace BlazorCms.BuildingBlocks.Extensions;

/// <summary>
/// Provides extension methods for instances of a <see cref="CssStyleBuilder"/> class.
/// </summary>
public static class CssStyleBuilderExtensions
{
    /// <summary>
    /// Adds a conditional CSS style to a new builder.
    /// </summary>
    /// <param name="initial">The initial CSS style.</param>
    /// <param name="value">CSS style to conditionally add.</param>
    /// <param name="when">Condition in which the CSS style is added.</param>
    /// <returns>A reference to an initialized <see cref="CssStyleBuilder"/> instance.</returns>
    public static CssStyleBuilder AddStyle(this string initial, string? value, bool? when)
        => new CssStyleBuilder(initial).AddStyle(value, when);

    /// <summary>
    /// Adds a conditional CSS style to a new builder.
    /// </summary>
    /// <param name="initial">The initial CSS style.</param>
    /// <param name="value">CSS style to conditionally add.</param>
    /// <param name="when">Condition in which the CSS style is added.</param>
    /// <returns>A reference to an initialized <see cref="CssStyleBuilder"/> instance.</returns>
    public static CssStyleBuilder AddStyle(this string initial, string? value, Func<bool>? when = null) =>
        new CssStyleBuilder(initial).AddStyle(value, when);

    /// <summary>
    /// Adds an array of conditional CSS styles to a new builder.
    /// </summary>
    /// <param name="styles">The CSS style list.</param>
    /// <param name="args">
    /// An array of tuples consisting of a CSS style to conditionally 
    /// add, and a condition in which the CSS style is added.
    /// </param>
    /// <returns>A reference to an initialized <see cref="CssStyleBuilder"/> instance.</returns>
    public static CssStyleBuilder AddStyles(this string styles, params (string? value, bool? when)[]? args)
        => new CssStyleBuilder(styles).AddStyles(args);


    /// <summary>
    /// Adds an array of conditional CSS styles to a new builder.
    /// </summary>
    /// <param name="styles">The CSS style list.</param>
    /// <param name="args">
    /// An array of tuples consisting of a CSS style to conditionally add, and a 
    /// predicate function evaluating the condition in which the CSS style is added.
    /// </param>
    /// <returns>A reference to an initialized <see cref="CssStyleBuilder"/> instance.</returns>
    public static CssStyleBuilder AddStyles(this string styles, params (string? value, Func<bool>? when)[]? args)
        => new CssStyleBuilder(styles).AddStyles(args);

    /// <summary>
    /// Adds an array of conditional CSS styles to the builder.
    /// </summary>
    /// <param name="builder">The CSS style builder.</param>
    /// <param name="args">
    /// An array of tuples consisting of a CSS style to conditionally 
    /// add, and a condition in which the CSS style is added.
    /// </param>
    /// <returns>A reference to the <paramref name="builder"/>.</returns>
    public static CssStyleBuilder AddStyles(this CssStyleBuilder builder, params (string? value, bool? when)[]? args)
    {
        if (args?.Length > 0)
        {
            foreach (var (value, when) in args)
                if (when ?? true)
                    builder.AddStyle(value);
        }
        return builder;
    }

    /// <summary>
    /// Adds an array of conditional CSS styles to the builder.
    /// </summary>
    /// <param name="builder">The CSS style builder.</param>
    /// <param name="args">
    /// An array of tuples consisting of a CSS style to conditionally add, and a 
    /// predicate function evaluating the condition in which the CSS style is added.
    /// </param>
    /// <returns>A reference to the <paramref name="builder"/>.</returns>
    public static CssStyleBuilder AddStyles(this CssStyleBuilder builder, params (string? value, Func<bool>? when)[]? args)
    {
        if (args?.Length > 0)
        {
            foreach (var (value, when) in args)
                if (when is null || when.Invoke())
                    builder.AddStyle(value);
        }
        return builder;
    }
}