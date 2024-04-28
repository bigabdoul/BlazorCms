namespace BlazorCms.BuildingBlocks;

/// <summary>
/// Creates a <see cref="CssStyleBuilder" />> used to define conditional CSS styles used in a component.
/// </summary>
/// <param name="value">The default value to supply.</param>
public struct CssStyleBuilder(string? value)
{
    private string stringBuffer = value ?? string.Empty;

    /// <summary>
    /// Creates an Empty <see cref="CssStyleBuilder" />> used to define conditional CSS styles used in a component.
    /// </summary>
    public static CssStyleBuilder Empty() => new();

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="value">CSS style to conditionally add.</param>
    /// <param name="when">Condition in which the CSS style is added.</param>
    /// <returns><see cref="CssStyleBuilder" />></returns>
    public CssStyleBuilder AddStyle(string? value, bool? when = true) => when == true ? AddStyle(value) : this;

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="value">CSS style to conditionally add.</param>
    /// <param name="when">Condition in which the CSS style is added.</param>
    /// <returns><see cref="CssStyleBuilder" />></returns>
    public CssStyleBuilder AddStyle(string? value, Func<bool>? when = null) => AddStyle(value, when?.Invoke());

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="value">Function that returns a CSS style to conditionally add.</param>
    /// <param name="when">Condition in which the CSS style is added.</param>
    /// <returns><see cref="CssStyleBuilder" />></returns>
    public CssStyleBuilder AddStyle(Func<string> value, bool? when = true) => when == true ? AddStyle(value()) : this;

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="value">Function that returns a CSS style to conditionally add.</param>
    /// <param name="when">Condition in which the CSS style is added.</param>
    /// <returns><see cref="CssStyleBuilder" />></returns>
    public CssStyleBuilder AddStyle(Func<string> value, Func<bool>? when = null) => AddStyle(value, when?.Invoke());

    /// <summary>
    /// Adds a conditional nested <see cref="CssStyleBuilder" />> to the builder.
    /// </summary>
    /// <param name="value">CSS style to conditionally add.</param>
    /// <param name="when">Condition in which the CSS style is added.</param>
    /// <returns><see cref="CssStyleBuilder" />></returns>
    public CssStyleBuilder AddStyle(CssStyleBuilder builder, bool? when = true) => when == true ? AddStyle(builder.ToString()) : this;

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="value">CSS style to conditionally add.</param>
    /// <param name="when">Condition in which the CSS style is added.</param>
    /// <returns><see cref="CssStyleBuilder" />></returns>
    public CssStyleBuilder AddStyle(CssStyleBuilder builder, Func<bool>? when = null) => AddStyle(builder, when?.Invoke());

    /// <summary>
    /// Adds the specified value to the styles buffer, if it doesn't contain it yet.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns></returns>
    public CssStyleBuilder AddStyle(string? value)
    {
        if (value is null) return this;

        if (string.IsNullOrWhiteSpace(stringBuffer))
            stringBuffer = value;
        else if (stringBuffer.Contains(value) == false)
            // don't add duplicate values
            stringBuffer = stringBuffer.EndsWith(';') ? stringBuffer + value : "; " + value;

        return this;
    }

    /// <summary>
    /// Adds a conditional CSS Style when it exists in a dictionary to the builder.
    /// Null safe operation.
    /// </summary>
    /// <param name="attributes">Additional Attribute splat parameters</param>
    /// <returns><see cref="CssStyleBuilder" />></returns>
    public CssStyleBuilder AddStyleFromAttributes(IDictionary<string, object>? attributes) =>
        true == attributes?.TryGetValue("style", out var v) && v != null ? AddStyle(v.ToString()) : this;

    /// <summary>
    /// Returns the built string buffer.
    /// </summary>
    /// <returns></returns>
    public override readonly string ToString() => stringBuffer;

    /// <summary>
    /// Performs an implicit conversion from a <see cref="string"/> 
    /// to an instance of the <see cref="CssStyleBuilder"/> struct.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    public static implicit operator CssStyleBuilder(string? value) => new(value);

    /// <summary>
    /// Performs an implicit conversion from an instance of a 
    /// <see cref="CssStyleBuilder"/> struct to a <see cref="string"/>.
    /// </summary>
    /// <param name="builder">The style builder to convert.</param>
    public static implicit operator string(CssStyleBuilder builder) => builder.ToString();
}