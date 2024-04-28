using BlazorCms.BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using static BlazorCms.BuildingBlocks.Extensions.CollectionExtensions;

namespace BlazorCms.BuildingBlocks.Components;

public partial class CmsTabs
{
    [Parameter] public bool HideHeaders { get; set; }
    [Parameter] public string? ContentStyle { get; set; }
    [Parameter] public string? TabPaneStyle { get; set; }
    [Parameter] public int TabPaneMinHeight { get; set; } = 550;
    [Parameter] public ICmsTabsOptions Options { get; set; } = default!;
    [Parameter] public EventCallback<(CmsTab Tab, int Index)> OnTabChange { get; set; }
    [Parameter] public string? TabContentClass { get; set; } = "pt-3";

    readonly List<CmsTab> Pages = [];

    string NavStyle => $"nav nav-{Options.NavStyle}{Alignment}{Adjustment}".ToLower();
    
    string Alignment => string.Empty.AddClass($"justify-content-{Options.Align}".ToLower(), 
        !Options.Vertical && Options.Align != CmsTabAlign.Start);

    string Adjustment => string.Empty.AddClass($" nav-{Options.Adjustment}".ToLower(), 
        !Options.Vertical && Options.Adjustment != CmsTabAdjustment.Default);

    public int TabCount => Pages.Count;

    public CmsTab? PreviousTab
    {
        get
        {
            var index = ActiveTabIndex - 1;
            return index > -1 ? Pages[index] : null;
        }
    }

    public CmsTab? NextTab
    {
        get
        {
            var next = ActiveTabIndex + 1;
            return next < Pages.Count ? Pages[next] : null;
        }
    }

    public CmsTab? ActiveTab { get; private set; }

    public int ActiveTabIndex => ActiveTab is null ? -1 : Pages.IndexOf(ActiveTab);

    public bool IsActive(CmsTab tab) => EqualityComparer<CmsTab>.Default.Equals(ActiveTab, tab);

    protected override void OnInitialized()
    {
        Options ??= new CmsTabsOptions();
    }

    protected internal void AddPage(CmsTab tab)
    {
        Pages.Add(tab);
        if (Pages.Count == 1) ActiveTab = tab;
        StateHasChanged();
    }

    protected internal void RemovePage(CmsTab tab)
    {
        if (Pages.Remove(tab))
            StateHasChanged();
    }

    protected async Task ActivateTabCoreAsync(CmsTab tab, int? index)
    {
        if (!IsActive(tab))
        {
            ActiveTab = tab;
            if (OnTabChange.HasDelegate)
            {
                index ??= GetTabIndex(tab);
                await OnTabChange.InvokeAsync((tab, index.Value));
            }
        }
    }

    public Task ActivateTabAsync(int index) => ActivateTabCoreAsync(Pages[index], index);

    public Task ActivateTabAsync(CmsTab tab) => ActivateTabCoreAsync(tab, null);

    public int GetTabIndex(CmsTab tab) => Pages.IndexOf(tab);
    public CmsTab GetTab(int index) => Pages[index];

    IDictionary<string, object> GetContentAttributes(string? extraClass = null) =>
        GetAttributes(("class", $"tab-content{extraClass}"), ("style", ContentStyle));

    internal IDictionary<string, object> GetTabPaneAttributes(string? extraClass = null) =>
        GetAttributes(("class", $"tab-pane fade {extraClass}".TrimEnd()), ("style", TabPaneStyle));
}

/// <summary>
/// Encapsulates event data for a wizard component.
/// </summary>
public class WizardStepEventArgs : CancelEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WizardStepEventArgs"/> class.
    /// </summary>
    public WizardStepEventArgs()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WizardStepEventArgs"/> class
    /// using the specified parameters.
    /// </summary>
    /// <param name="current">The current wizard step.</param>
    public WizardStepEventArgs(int current) : this(current, false) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WizardStepEventArgs"/> class
    /// using the specified parameters.
    /// </summary>
    /// <param name="current">The current wizard step.</param>
    /// <param name="cancel">The initial value of the <see cref="CancelEventArgs.Cancel"/> property.</param>
    public WizardStepEventArgs(int current, bool cancel) : base(cancel)
    {
        Current = current;
    }

    /// <summary>
    /// The current wizard step.
    /// </summary>
    public int Current { get; init; }

    /// <summary>
    /// The previous or next wizard step to show when the user clicks a button.
    /// </summary>
    public int? GotoStep { get; set; }

    /// <summary>
    /// If set, disables or enables a button.
    /// </summary>
    public bool? Disabled { get; set; }
}

public class WizardOptions
{
    public bool Disabled { get; set; }
    public CmsTabAlign ButtonsAlign { get; set; }
    public string? ButtonsStyle { get; set; } = "min-width:120px;";
    public string? ButtonsClass { get; set; } = "btn btn-sm btn-primary mx-1 text-capitalize";
    public EventCallback<WizardStepEventArgs> OnStepButtonInitialize { get; set; }
    public EventCallback<WizardStepEventArgs> OnStepValidate { get; set; }
    public bool StepCaptionAsButtonText { get; set; }
    public bool StepNumberBeforeButtonText { get; set; }
    public string? PreviousText { get; set; }
    public object? PreviousIcon { get; set; } = "fas fa-arrow-left";
    public string? CurrentText { get; set; }
    public object? CurrentIcon { get; set; } = "fas fa-edit";
    public string? CurrentTextClass { get; set; } = "btn btn-outline-success btn-sm mx-1 text-capitalize";
    public string? CurrentTextStyle { get; set; } = "cursor:initial;min-width:100px;";
    public bool CurrentTextBetweenButtons { get; set; }
    public string? NextText { get; set; }
    public object? NextIcon { get; set; } = "fas fa-arrow-right";
    public bool HideButtonWhenStepUnavailable { get; set; }
    public string ButtonsAlignment => $"d-flex justify-content-{ButtonsAlign} px-3".ToLower();
    public IDictionary<string, object?>? AdditionalAttributes { get; set; }

    public WizardOptions SetStepButtonInitializer(Func<WizardStepEventArgs, Task> handler)
    {
        OnStepButtonInitialize = EventCallback.Factory.Create(this, handler);
        return this;
    }

    public WizardOptions SetStepValidator(Func<WizardStepEventArgs, Task> handler)
    {
        OnStepValidate = EventCallback.Factory.Create(this, handler);
        return this;
    }
}

public enum CmsTabAlign
{
    Start,
    Center,
    End,
}

public enum CmsTabStyle
{
    Tabs,
    Pills,
}

public enum CmsTabAdjustment
{
    Default,
    Fill,
    Justify,
}

public interface ICmsTabsOptions
{
    string ActiveTabClass { get; set; }
    CmsTabAdjustment Adjustment { get; set; }
    CmsTabAlign Align { get; set; }
    bool BootstrapTabs { get; set; }
    bool DisableTabChangeNotification { get; set; }
    string InactiveTabClass { get; set; }
    CmsTabStyle NavStyle { get; set; }
    string TabGroupClass { get; set; }
    bool Vertical { get; set; }
}

public class CmsTabsOptions : ICmsTabsOptions
{
    public string TabGroupClass { get; set; } = "btn-group mb-lg-5";
    public string ActiveTabClass { get; set; } = "btn btn-primary";
    public string InactiveTabClass { get; set; } = "btn btn-secondary";
    public bool BootstrapTabs { get; set; } = true;
    public bool DisableTabChangeNotification { get; set; }
    public bool Vertical { get; set; }
    public CmsTabStyle NavStyle { get; set; }
    public CmsTabAlign Align { get; set; }
    public CmsTabAdjustment Adjustment { get; set; }
}
