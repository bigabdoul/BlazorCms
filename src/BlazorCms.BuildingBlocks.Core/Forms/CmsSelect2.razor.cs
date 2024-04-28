using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCms.BuildingBlocks.Forms;

public class CmsSelect2Base<TItem, TValue> : CmsSelectBase<TItem, TValue>, IDisposable
{
    [Inject] Core.BuildingBlocksModuleLoader JsLoader { get; set; } = default!;

    DotNetObjectReference<CmsSelect2Base<TItem, TValue>>? dotNet;
    readonly IList<Select2EventArgs> selections = [];

    protected override void OnInitialized()
    {
        dotNet = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            JsLoader.ModuleSource = "./_content/BlazorCms.BuildingBlocks.Core/js/dist/cms-select2.js";
            var options = new
            { 
                selector = $"#{Id}", 
                dotNet,
                dotNetMethod = nameof(NotifySelectionChanged)
            };
            await JsLoader.InvokeVoidAsync("init", options);
        }
    }

    [JSInvokable]
    public async Task NotifySelectionChanged(Select2EventArgs e)
    {
        if (e.Disabled) return;

        using var task = e.EventType switch
        {
            Select2EventType.Select or Select2EventType.Unselect => HandleSelectAsync(e),
            _ => Task.CompletedTask,
        };

        await task;
    }

    async Task HandleSelectAsync(Select2EventArgs e)
    {
        if (Multiple)
        {
            if (e.Selected)
                selections.Add(e);
            else
                selections.Remove(e);
            SelectedValues = selections.Select(item => GetValue(item.Id)).ToArray();
            await SelectedValuesChanged.InvokeAsync(SelectedValues);
        }
        else
        {
            Value = GetValue(e.Id);
            if (e.Selected)
            {
                await ValueChanged.InvokeAsync(Value);
            }
        }
    }

    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            dotNet?.Dispose();
        }
    }

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}

public class Select2EventArgs : IEquatable<Select2EventArgs>
{
    public Select2EventType EventType { get; set; }
    public string? Id { get; set; }
    public string? Text { get; set; }
    public string? Title { get; set; }
    public bool Disabled { get; set; }
    public bool Selected { get; set; }

    public bool Equals(Select2EventArgs? other)
        => other is not null && true == Id?.Equals(other.Id);

    public override int GetHashCode() => $"{Id}".GetHashCode();

    public override bool Equals(object? obj)
        => obj is Select2EventArgs args && Equals(args);
}

public enum Select2EventType
{
    Change,
    ChangeSelect2,
    Closing,
    Close,
    Opening,
    Open,
    Selecting,
    Select,
    Unselecting,
    Unselect,
    Clearing,
    Clear,
}