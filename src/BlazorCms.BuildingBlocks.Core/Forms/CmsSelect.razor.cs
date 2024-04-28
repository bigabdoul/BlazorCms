using BlazorCms.BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Components;

namespace BlazorCms.BuildingBlocks.Forms;

public class CmsSelectBase<TItem, TValue> : BlockListComponentBase<TItem>
{
    [Parameter] public TValue? Value { get; set; }
    //[Parameter] public EventCallback<TItem> ItemChanged { get; set; }
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
    [Parameter] public Func<TItem, TValue?>? ValueGetter { get; set; }
    [Parameter] public Func<TItem, string?>? TextGetter { get; set; }
    [Parameter] public Func<string?, TValue?>? Converter { get; set; }
    [Parameter] public bool Multiple { get; set; }
    [Parameter] public int Size { get; set; }
    [Parameter] public TValue?[]? SelectedValues { get; set; }
    [Parameter] public EventCallback<TValue?[]?> SelectedValuesChanged { get; set; }

    protected override IDictionary<string, object> DefaultAttributes => new Dictionary<string, object>
    {
        { "class", "form-select" },
        //{ "aria-label", Title! },
    };

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Id = GetId("cms-select-");
    }

    protected virtual void OnValueChanged(ChangeEventArgs e)
    {
        if (!Multiple)
        {
            Value = GetValue(e.Value);
            ValueChanged.InvokeAsync(Value);
        }
        else if (e.Value is string[] values)
        {
            SelectedValues = values.Select(GetValue).ToArray();
            SelectedValuesChanged.InvokeAsync(SelectedValues);
        }
    }

    protected virtual bool TryGetValueCore(TItem item, out TValue? result)
    {
        result = default!;

        if (ValueGetter is null)
            return false;

        result = ValueGetter.Invoke(item);
        return true;
    }

    protected virtual TValue? GetValue(object? value)
    {
        var convertedValue = Converter != null
            ? Converter.Invoke(value?.ToString())
            : value.ChangeType<TValue>();
        return convertedValue;
    }

    protected virtual string? GetText(TItem item) => TextGetter != null ? TextGetter.Invoke(item) : item?.ToString();
}