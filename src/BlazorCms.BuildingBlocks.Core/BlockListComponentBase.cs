using Microsoft.AspNetCore.Components;

namespace BlazorCms.BuildingBlocks;

/// <summary>
/// Represents the base class for list-based building block components.
/// </summary>
public abstract class BlockListComponentBase : BlockComponentBase
{
    /// <summary>
    /// Gets or sets the list items.
    /// </summary>
    [Parameter] public IList<object>? Items { get; set; }

    /// <summary>
    /// Gets or sets the CSS class for a list item.
    /// </summary>
    [Parameter] public string? ItemClass { get; set; }

    /// <summary>
    /// Gets or sets the CSS style for a list item.
    /// </summary>
    [Parameter] public string? ItemStyle { get; set; }
}

/// <summary>
/// Represents the base class for strongly typed  list-based building block components.
/// </summary>
/// <typeparam name="T">The type of list items.</typeparam>
public abstract class BlockListComponentBase<T> : BlockListComponentBase
{
    /// <summary>
    /// Gets or sets the strongly typed list items.
    /// </summary>
    [Parameter] public IList<T> ListItems { get; set; } = [];
}
