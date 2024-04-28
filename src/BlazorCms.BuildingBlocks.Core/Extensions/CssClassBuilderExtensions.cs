namespace BlazorCms.BuildingBlocks.Extensions;

/// <summary>
/// Provides extension methods for instances of a <see cref="CssClassBuilder"/> class.
/// </summary>
public static class CssClassBuilderExtensions
{
    /// <summary>
    /// Adds a conditional CSS Class to a new builder.
    /// </summary>
    /// <param name="initial">The initial CSS class.</param>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns>A reference to an initialized <see cref="CssClassBuilder"/> instance.</returns>
    public static CssClassBuilder AddClass(this string? initial, string? value, bool? when)
        => new CssClassBuilder(initial).AddClass(value, when);

    /// <summary>
    /// Adds a conditional CSS Class to a new builder.
    /// </summary>
    /// <param name="initial">The initial CSS class.</param>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns>A reference to an initialized <see cref="CssClassBuilder"/> instance.</returns>
    public static CssClassBuilder AddClass(this string? initial, string? value, Func<bool>? when = null) => 
        new CssClassBuilder(initial).AddClass(value, when);

    /// <summary>
    /// Adds an array of conditional CSS classes to a new builder.
    /// </summary>
    /// <param name="classes">The CSS class list.</param>
    /// <param name="args">
    /// An array of tuples consisting of a CSS class to conditionally 
    /// add, and a condition in which the CSS class is added.
    /// </param>
    /// <returns>A reference to an initialized <see cref="CssClassBuilder"/> instance.</returns>
    public static CssClassBuilder AddClasses(this string? classes, params (string? value, bool? when)[]? args)
        => new CssClassBuilder(classes).AddClasses(args);


    /// <summary>
    /// Adds an array of conditional CSS classes to a new builder.
    /// </summary>
    /// <param name="classes">The CSS class list.</param>
    /// <param name="args">
    /// An array of tuples consisting of a CSS class to conditionally add, and a 
    /// predicate function evaluating the condition in which the CSS class is added.
    /// </param>
    /// <returns>A reference to an initialized <see cref="CssClassBuilder"/> instance.</returns>
    public static CssClassBuilder AddClasses(this string? classes, params (string? value, Func<bool>? when)[]? args)
        => new CssClassBuilder(classes).AddClasses(args);

    /// <summary>
    /// Adds an array of conditional CSS classes to the builder.
    /// </summary>
    /// <param name="builder">The CSS class builder.</param>
    /// <param name="args">
    /// An array of tuples consisting of a CSS class to conditionally 
    /// add, and a condition in which the CSS class is added.
    /// </param>
    /// <returns>A reference to the <paramref name="builder"/>.</returns>
    public static CssClassBuilder AddClasses(this CssClassBuilder builder, params (string? value, bool? when)[]? args)
    {
        if (args?.Length > 0)
        {
            foreach (var (value, when) in args)
                if (when ?? true)
                    builder.AddClass(value);
        }
        return builder;
    }

    /// <summary>
    /// Adds an array of conditional CSS classes to the builder.
    /// </summary>
    /// <param name="builder">The CSS class builder.</param>
    /// <param name="args">
    /// An array of tuples consisting of a CSS class to conditionally add, and a 
    /// predicate function evaluating the condition in which the CSS class is added.
    /// </param>
    /// <returns>A reference to the <paramref name="builder"/>.</returns>
    public static CssClassBuilder AddClasses(this CssClassBuilder builder, params (string? value, Func<bool>? when)[]? args)
    {
        if (args?.Length > 0)
        {
            foreach (var (value, when) in args)
                if (when is null || when.Invoke())
                    builder.AddClass(value);
        }
        return builder;
    }
}
