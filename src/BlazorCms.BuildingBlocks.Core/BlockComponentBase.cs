using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using static BlazorCms.BuildingBlocks.Extensions.CollectionExtensions;

namespace BlazorCms.BuildingBlocks;

/// <summary>
/// Represents the base class for building block-based components.
/// </summary>
public abstract class BlockComponentBase : ComponentBase
{
    protected readonly CssClassBuilder ClassBuilder = CssClassBuilder.Empty();

    /// <summary>
    /// Returns a new string/object dictionary instance.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, object> EmptyDictionary() => [];

    /// <summary>
    /// Gets the component's unique identifier.
    /// </summary>
    public readonly string UniqueId = $"{Guid.NewGuid().GetHashCode():x}";

    /// <summary>
    /// Gets or sets the component's identifier.
    /// </summary>
    [Parameter] public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the child content render fragment.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the CSS class.
    /// </summary>
    [Parameter] public string? Class { get; set; }

    /// <summary>
    /// Gets or sets the CSS style.
    /// </summary>
    [Parameter] public string? Style { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [Parameter] public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the class of the title. Defaults to "fs-1".
    /// </summary>
    [Parameter] public string? TitleClass { get; set; } = "fs-1";

    /// <summary>
    /// Gets or sets the class of the subtitle. Defaults to "fs-3".
    /// </summary>
    [Parameter] public string? SubtitleClass { get; set; } = "fs-3";

    /// <summary>
    /// Gets or sets the title's navigation URL.
    /// </summary>
    [Parameter] public string? TitleUrl { get; set; }

    /// <summary>
    /// Gets or sets the render fragment for the title.
    /// </summary>
    [Parameter] public RenderFragment? TitleContent {  get; set; }

    /// <summary>
    /// Gets or sets the subtitle.
    /// </summary>
    [Parameter] public string? Subtitle { get; set; }

    /// <summary>
    /// Gets or sets the render fragment for the subtitle.
    /// </summary>
    [Parameter] public RenderFragment? SubtitleContent { get; set; }

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    [Parameter] public object? Icon { get; set; }

    /// <summary>
    /// Gets or sets the icon at the end of the content.
    /// </summary>
    [Parameter] public object? IconEnd { get; set; }

    /// <summary>
    /// Determines whether to show the icon. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool ShowIcon { get; set; } = true;

    /// <summary>
    /// Indicates whether to ignore the default attributes (especially CSS classes) when
    /// calling the method <see cref="GetDefaultAttributes(ValueTuple{string, object?}[]?)"/>.
    /// </summary>
    [Parameter] public bool? NoDefaults { get; set; }

    /// <summary>
    /// Indicates whether the tabindex attribute should be set to -1.
    /// </summary>
    [Parameter] public bool NoTabStop { get; set; }

    /// <summary>
    /// Gets or sets additional unmatched attributes for the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object>? AdditionalAttributes { get; set; }
    protected virtual IDictionary<string, object>? DefaultAttributes { get; }

    public IDictionary<string, object>? GetDefaultAttributes(bool? noDefaults = null)
    {
        if (noDefaults ?? NoDefaults ?? false)
            return null;

        // preserve the 'DefaultAttributes' dictionary by merging it into a new one
        var defAttrs = DefaultAttributes.Merge(EmptyDictionary());

        // sanitize the class list
        var classes = CssClassBuilder.Empty().AddClassFromAttributes(defAttrs);

        if (IsNotBlank(classes))
            defAttrs["class"] = classes;

        string styleValue = CssStyleBuilder.Empty()
            .AddStyleFromAttributes(defAttrs)
            .AddStyle(Style);

        if (IsNotBlank(styleValue))
            defAttrs["style"] = styleValue;

        return defAttrs;
    }

    public IDictionary<string, object>? GetDefaultAttributes(IDictionary<string, object>? attributes, bool? noDefaults = null)
        => noDefaults ?? NoDefaults ?? false ? EmptyDictionary() : attributes;

    /// <summary>
    /// Consolidates the set CSS classes and styles with the default <paramref name="attributes"/> 
    /// according to the actual value of the <see cref="NoDefaults"/> property.
    /// </summary>
    /// <param name="attributes">An array of name/value tuples representing the default attributes to set.</param>
    /// <returns>A consolidated dictionary of HTML attribute names and values to render.</returns>
    public IDictionary<string, object> GetDefaultAttributes(params (string name, object? value)[]? attributes)
        => GetDefaultAttributes(NoDefaults, attributes);

    /// <summary>
    /// Consolidates the set CSS classes and styles with the default <paramref name="attributes"/>.
    /// according to the specified <paramref name="noDefaults"/> or fallback 
    /// <see cref="NoDefaults"/> properties.
    /// </summary>
    /// <param name="noDefaults">
    /// <see langword="null"/> to fall back to <see cref="NoDefaults"/>.
    /// <see langword="true"/> to ignore the default attributes specified by <paramref name="attributes"/>.
    /// <see langword="false"/> to include the default attributes specified by <paramref name="attributes"/>.
    /// </param>
    /// <param name="attributes">An array of name/value tuples representing the default attributes to set.</param>
    /// <returns></returns>
    public IDictionary<string, object> GetDefaultAttributes(bool? noDefaults, params (string name, object? value)[]? attributes)
        => noDefaults ?? NoDefaults ?? false ? EmptyDictionary() : GetAttributes(attributes);

    /// <summary>
    /// Consolidates the set CSS classes and styles with the specified <paramref name="attributes"/>.
    /// </summary>
    /// <param name="attributes">An array of name/value tuples representing the attributes to set.</param>
    /// <returns></returns>
    public IDictionary<string, object> GetAllAttributes(params (string name, object? value)[]? attributes)
        => GetAllAttributes(GetAttributes(attributes));

    public IDictionary<string, object>? GetAllAttributes(bool? noDefaults)
         => GetAllAttributes(EmptyDictionary(), noDefaults);

    /// <summary>
    /// Returns a dictionary containing the <see cref="Class"/>, 
    /// <see cref="Style"/>, and <see cref="AdditionalAttributes"/>.
    /// </summary>
    /// <param name="attributes">Existing attributes to merge with others.</param>
    /// <param name="noDefaults">
    /// <see langword="null"/> to fall back to <see cref="NoDefaults"/>.
    /// <see langword="true"/> to ignore the default attributes specified by <see cref="DefaultAttributes"/>.
    /// <see langword="false"/> to include the default attributes specified by <see cref="DefaultAttributes"/>.
    /// </param>
    /// <returns></returns>
    public IDictionary<string, object> GetAllAttributes(IDictionary<string, object>? attributes = null, bool? noDefaults = null)
    {
        var defAttrs = GetDefaultAttributes(noDefaults);
        string css = CssClassBuilder.Empty().AddClassFromAttributes(defAttrs);

        if (defAttrs != null && IsNotBlank(css))
            defAttrs["class"] = css;

        var attrs = GetAttributesToRender(attributes);
        return attrs.Merge(defAttrs);
    }

    private IDictionary<string, object> GetAttributesToRender(IDictionary<string, object>? existingAttrs = null)
    {
        var classToRender = CssClassBuilder.Default(Class)
            .AddClassFromAttributes(AdditionalAttributes)
            .AddClassFromAttributes(existingAttrs)
            .Build();

        // Since we're going to mutate the dictionary, merge the additional
        // attributes into an empty dictionary to preserve the values supplied
        // from the 'AdditionalAttributes' dictionary.
        var attrs = existingAttrs.Merge(AdditionalAttributes.Merge(EmptyDictionary()));

        string styleToRender = CssStyleBuilder.Empty()
            .AddStyleFromAttributes(existingAttrs)
            .AddStyle(Style)
            .AddStyleFromAttributes(attrs);

        existingAttrs?.Remove("style");
        attrs.Remove("style");

        _ = GetAttributes
        (
            //("id", GetId()),
            ("class", classToRender), 
            ("style", styleToRender), 
            ("title", Title),
            ("tabindex", NoTabStop ? "-1" : null)
        ).Merge(attrs);

        return attrs;
    }

    /// <summary>
    /// Returns a sanitized representation (devoid of duplicated class 
    /// names and blank spaces) of the <see cref="DefaultAttributes"/>,
    /// <see cref="Class"/>, and <see cref="AdditionalAttributes"/> properties.
    /// </summary>
    /// <returns></returns>
    public virtual CssClassBuilder GetClass() => ClassBuilder
        .Clear()
        .AddClassFromAttributes(DefaultAttributes)
        .AddClass(Class)
        .AddClassFromAttributes(AdditionalAttributes);

    /// <summary>
    /// Determines whether <see cref="ShowIcon"/> is <see langword="true"/> 
    /// and <see cref="Icon"/> and <see cref="IconEnd"/> are not 
    /// <see langword="null"/> or blank strings.
    /// </summary>
    protected virtual bool HasIcon => ShowIcon && 
    (
        (Icon is string s && IsNotBlank(s) || Icon != null) ||
        (IconEnd is string s1 && IsNotBlank(s1) || IconEnd != null)
    );

    /// <summary>
    /// Determines whether <see cref="TitleContent"/> is not null or <see cref="Title"/> is not blank.
    /// </summary>
    protected virtual bool HasTitle => TitleContent != null || IsNotBlank(Title);

    /// <summary>
    /// Determines whether <see cref="SubtitleContent"/> is not null or <see cref="Subtitle"/> is not blank.
    /// </summary>
    protected virtual bool HasSubtitle => SubtitleContent != null || IsNotBlank(Subtitle);

    /// <summary>
    /// Returns either <see cref="Id"/>, or (if the former is null) <see cref="UniqueId"/>.
    /// </summary>
    /// <param name="prefix">The optional prefix for <see cref="UniqueId"/>.</param>
    /// <returns></returns>
    protected string GetId(string? prefix = null) => Id ?? $"{prefix}{UniqueId}";

    public static bool IsBlank([NotNullWhen(false)] string? value) => string.IsNullOrWhiteSpace(value);
    public static bool IsNotBlank([NotNullWhen(true)] string? value) => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Returns the first item in <paramref name="values"/> whose 
    /// value is not the default of the <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">The type of values.</typeparam>
    /// <param name="values">An array of values to test.</param>
    /// <returns></returns>
    public static T? Coalesce<T>(params T?[]? values) => values.Coalesce();

    /// <summary>
    /// Returns <see langword="true"/> for the first item in <paramref name="values"/> whose
    /// value satisfies the provided <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The type of values.</typeparam>
    /// <param name="predicate">A function that tests each item in <paramref name="values"/>.</param>
    /// <param name="values">An array of values to test.</param>
    /// <returns></returns>
    public static bool Coalesce<T>(Func<T?, bool> predicate, params T?[]? values) => values.Coalesce(predicate);

    /// <summary>
    /// Returns a <see cref="Tuple{T1, T2}"/> for the first item in <paramref name="values"/> 
    /// whose value satisfies the provided <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The type of values.</typeparam>
    /// <param name="predicate">A function that tests each item in <paramref name="values"/>.</param>
    /// <param name="values">A collection of values to test.</param>
    /// <returns></returns>
    /// <remarks>
    /// The components of <see cref="Tuple{T1, T2}"/> are of types <see cref="bool"/> and <typeparamref name="T"/> respectively.
    /// </remarks>
    public static (bool Success, T? Value) CoalesceValue<T>(Func<T?, bool> predicate, params T?[]? values) => values.CoalesceValue(predicate);
}
