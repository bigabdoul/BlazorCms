using Microsoft.AspNetCore.Components;

namespace BlazorCms.ApexCharts.Components;

public partial class RealtimeChart : IDisposable
{
    [Parameter] public TimelineChartDataSet[]? Data { get; set; }
    [Parameter] public int? Height { get; set; } = 320;
    [Parameter] public int? Interval { get; set; } = 1000;
    [Parameter] public int? PlotRange { get; set; }
    [Parameter] public ChartStrokeCurve Curve { get; set; } = ChartStrokeCurve.Smooth;
    [Parameter] public ChartType Type { get; set; } = ChartType.Line;

    [Parameter] public IReadOnlyList<ChartStrokeCurve>? SelectCurves { get; set; }
    [Parameter] public IReadOnlyList<ChartType>? SelectTypes { get; set; }
    [Parameter] public string? RefreshButtonText { get; set; }
    [Parameter] public string? RefreshButtonTitle { get; set; } = "Apply";
    [Parameter] public string? RefreshButtonIcon { get; set; } = "fas fa-refresh-alt";
    [Parameter] public bool ShowMetadata { get; set; }
    [Parameter] public bool ShuffleCurve { get; set; }
    [Parameter] public bool ShuffleType { get; set; }
    [Parameter] public string? ShuffleCurveText { get; set; } = "Shuffle Curve";
    [Parameter] public string? ShuffleTypeText { get; set; } = "Shuffle Type";
    [Parameter] public string? NoData { get; set; }
    [Parameter] public RenderFragment? NoDataContent { get; set; }

    /// <summary>
    /// Gets or sets the interval in milliseconds at which to shuffle 
    /// the <see cref="Curve"/> and/or <see cref="Type"/> properties.
    /// Setting to 0 or less will disable the periodic timer.
    /// </summary>
    [Parameter] public int ShuffleInterval { get; set; }

    /// <summary>
    /// Gets the unique chart identifier.
    /// </summary>
    public string ChartId => $"realtime-chart-{UniqueId}";

    bool rendered;
    bool disposed;
    string? lastShuffled;
    PeriodicTimer? periodicTimer;
    [Inject] ApexChartModuleLoader JsLoader { get; set; } = default!;

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            JsLoader.ModuleSource = "./_content/BlazorCms.ApexCharts/js/chartsmodule.js";
            await JsLoader.InvokeVoidAsync("setup");
        }
        if (!rendered && Data != null)
        {
            await RenderAsync();
            rendered = true;
        }
    }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        switch (ShuffleInterval)
        {
            case > 0:
                if (Data is not null)
                    _ = StartPeriodicTimerWithoutAwait();
                break;
            default:
                periodicTimer?.Dispose();
                break;
        }
    }

    /// <summary>
    /// Renders the chart with the current <see cref="Data"/>.
    /// </summary>
    /// <returns></returns>
    protected virtual Task RenderAsync() => RenderAsync(Data!);

    /// <summary>
    /// Renders the chart using the specified <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The dataset used to render the chart.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
    public virtual Task RenderAsync(TimelineChartDataSet[] data)
    {
        ArgumentNullException.ThrowIfNull(nameof(data));
        if (data.Length == 0) return Task.CompletedTask;

        var options = new RealtimeChartOptions
        {
            Selector = $"#{ChartId}",
            Title = Title,
            Height = Height,
            Interval = ShuffleInterval > 0 ? 0 : Interval,
            Data = data,
            Curve = Curve.ToCamelCase(),
            Type = Type.ToCamelCase(),
            PlotRange = PlotRange,
        }.Evaluate();

        return JsLoader.InvokeVoidAsync("realtime", options).AsTask();
    }

    /// <summary>
    /// Runs the <see cref="PeriodicTimer"/> on a background task. Don't await this method!
    /// </summary>
    /// <returns></returns>
    protected virtual async Task StartPeriodicTimerWithoutAwait()
    {
        periodicTimer?.Dispose();
        periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(ShuffleInterval));

        while (await periodicTimer.WaitForNextTickAsync())
        {
            var data = Data;

            if (ShuffleInterval <= 0 || data is null || data.Length == 0)
                break;

            var curves = SelectCurves;
            var types = SelectTypes;
            var count1 = curves?.Count ?? 0;
            var count2 = types?.Count ?? 0;

            if (count1 == 0 && count2 == 0)
                break;

            var hasChanges = false;

            if (ShuffleCurve && count1 > 0)
            {
                var index = Random.Shared.Next() % count1;
                if (Curve != curves![index])
                {
                    Curve = curves[index];
                    hasChanges = true;
                }
            }
            if (ShuffleType && count2 > 0)
            {
                var index = Random.Shared.Next() % count2;
                if (Type != types![index])
                {
                    Type = types[index];
                    hasChanges = true;
                }
            }
            if (hasChanges)
            {
                await InvokeAsync(async () =>
                {
                    await RenderAsync(data);
                    lastShuffled = $"{DateTime.Now:HH:mm:ss}";
                    StateHasChanged();
                });
            }
        }
    }

    /// <summary>
    /// Disposes the <see cref="PeriodicTimer"/>.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (!disposed)
            {
                periodicTimer?.Dispose();
                disposed = true;
            }
        }
    }

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
