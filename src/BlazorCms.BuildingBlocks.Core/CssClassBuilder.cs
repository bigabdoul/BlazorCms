namespace BlazorCms.BuildingBlocks;

/// <summary>
/// Creates a <see cref="CssClassBuilder" />> used to define conditional CSS classes used in a component.
/// Call <see cref="Build"/> to return the completed CSS classes as a <see cref="string"/>. 
/// </summary>
/// <param name="value">The default value to supply.</param>
public struct CssClassBuilder(string? value)
{
    private string stringBuffer = value ?? string.Empty;
    private string prefix = string.Empty;

    /// <summary>
    /// Sets the value to be prepended to all classes added following the this statement.
    /// When SetPrefix is called it will overwrite any previous prefix set for this instance.
    /// Prefixes are not applied when using <see cref="AddValue(string?)"/>.
    /// </summary>
    /// <param name="value">The value representing the prefix.</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder SetPrefix(string? value)
    {
        prefix = value ?? string.Empty;
        return this;
    }

    /// <summary>
    /// Creates a <see cref="CssClassBuilder" />> used to define conditional CSS classes used in a component.
    /// Call <see cref="Build"/> to return the completed CSS classes as a string. 
    /// </summary>
    /// <param name="value">The default value to supply.</param>
    public static CssClassBuilder Default(string? value) => new(value);

    /// <summary>
    /// Creates an Empty <see cref="CssClassBuilder" />> used to define conditional CSS classes used in a component.
    /// Call <see cref="Build"/> to return the completed CSS classes as a string. 
    /// </summary>
    public static CssClassBuilder Empty() => new();

    /// <summary>
    /// Adds a raw string to the builder that will be concatenated with the next class or value added to the builder.
    /// </summary>
    /// <param name="value">The value to add.</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddValue(string? value)
    {
        stringBuffer += value;
        return this;
    }

    /// <summary>
    /// Adds a CSS Class to the builder.
    /// </summary>
    /// <param name="value">CSS Class to add</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddClass(string? value) => string.IsNullOrWhiteSpace(value) ? this : AddValue(" " + prefix + value);

    /// <summary>
    /// Adds a conditional CSS Class to the builder.
    /// </summary>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddClass(string? value, bool? when = true) => when is null or true ? AddClass(value) : this;

    /// <summary>
    /// Adds a conditional CSS Class to the builder.
    /// </summary>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddClass(string? value, Func<bool>? when = null) => AddClass(value, when?.Invoke());

    /// <summary>
    /// Adds a conditional CSS Class to the builder.
    /// </summary>
    /// <param name="value">Function that returns a CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddClass(Func<string> value, bool? when = true) => when == true ? AddClass(value()) : this;

    /// <summary>
    /// Adds a conditional CSS Class to the builder.
    /// </summary>
    /// <param name="value">Function that returns a CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddClass(Func<string> value, Func<bool>? when = null) => AddClass(value, when?.Invoke());

    /// <summary>
    /// Adds a conditional nested <see cref="CssClassBuilder" />> to the builder.
    /// </summary>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddClass(CssClassBuilder builder, bool? when = true) => when == true ? AddClass(builder.Build()) : this;

    /// <summary>
    /// Adds a conditional CSS Class to the builder.
    /// </summary>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddClass(CssClassBuilder builder, Func<bool>? when = null) => AddClass(builder, when?.Invoke());

    /// <summary>
    /// Adds a conditional CSS Class when it exists in a dictionary to the builder.
    /// Null safe operation.
    /// </summary>
    /// <param name="additionalAttributes">Additional Attribute splat parameters</param>
    /// <returns><see cref="CssClassBuilder" />></returns>
    public CssClassBuilder AddClassFromAttributes(IDictionary<string, object>? additionalAttributes) =>
        additionalAttributes == null
            ? this
            : additionalAttributes.TryGetValue("class", out var c) && c != null ? AddClass(c.ToString()) : this;

    /// <summary>
    /// Finalize the completed CSS classes as a string.
    /// </summary>
    /// <returns>string</returns>
    public readonly string Build()
    {
        // String buffer finalization code
        if (stringBuffer is null)
            return string.Empty;

        var parts = string.Join(' ', stringBuffer.Trim().Split(' ', 
            StringSplitOptions.RemoveEmptyEntries | 
            StringSplitOptions.TrimEntries)
            .Distinct());

        return parts.Trim();
    }

    /// <summary>
    /// Clear the string buffer.
    /// </summary>
    /// <returns></returns>
    public CssClassBuilder Clear()
    {
        stringBuffer = string.Empty;
        return this;
    }

    // ToString should only and always call Build to finalize the rendered string.
    public override readonly string ToString() => Build();

    /// <summary>
    /// Performs an implicit conversion from a <see cref="string"/> 
    /// to an instance of the <see cref="CssClassBuilder"/> struct.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    public static implicit operator CssClassBuilder(string? value) => new(value);

    /// <summary>
    /// Performs an implicit conversion from an instance of a 
    /// <see cref="CssClassBuilder"/> struct to a <see cref="string"/>.
    /// </summary>
    /// <param name="builder">The class builder to convert.</param>
    public static implicit operator string(CssClassBuilder builder) => builder.ToString();
}
