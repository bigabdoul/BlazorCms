using BlazorCms.BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Components;

namespace BlazorCms.BuildingBlocks.Forms;

public class CmsButtonBase : BlockComponentBase
{
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public string? Text { get; set; }
    [Parameter] public string? NavigateToUri { get; set; }
    [Parameter] public bool ForceLoad { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Visible { get; set; } = true;
	[Parameter] public bool Loading { get; set; }
    [Parameter] public object? LoadingIcon { get; set; }
    [Parameter] public CmsThemeColors Color { get; set; } = CmsThemeColors.Primary;
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public EventCallback OnNavigated { get; set; }

    /// <summary>
    /// Whether to render a small button.
    /// </summary>
    [Parameter] public bool Small { get; set; }

    /// <summary>
    /// Whether to render a large button.
    /// </summary>
    [Parameter] public bool Large { get; set; }

    [Parameter] public bool Outline { get; set; } = true;

    [Inject] NavigationManager NavigationManager { get; set; } = default!;

    protected override IDictionary<string, object> DefaultAttributes => new Dictionary<string, object>
    {
        { "class", GetDefaultClass() }
    };

    protected virtual Task HandleOnClick()
    {
        if (OnClick.HasDelegate)
        {
            return OnClick.InvokeAsync();
        }
        else if (IsNotBlank(NavigateToUri))
        {
            NavigationManager.NavigateTo(NavigateToUri, ForceLoad);
            if (OnNavigated.HasDelegate)
            {
                return OnNavigated.InvokeAsync();
            }
        }
        return Task.CompletedTask;
    }

    CssClassBuilder GetDefaultClass()
    {
        var color = Color.GetColorName();
        return "btn"
            .AddClass("btn-sm", Small)
            .AddClass($"btn-{color}", !Outline)
            .AddClass($"btn-outline-{color}", Outline);
    }
}
