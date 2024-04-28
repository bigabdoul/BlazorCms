namespace BlazorCms.ApexCharts;

public class RealtimeChartOptions
{
    public string? Title { get; set; }
    public string? Selector { get; set; }
    public TimelineChartDataSet[]? Data { get; set; }
    public DateTime? XaxisMin { get; set; }
    public DateTime? XaxisMax { get; set; }
    public decimal? YaxisMin { get; set; }
    public decimal? YaxisMax { get; set; }
    public int? Height { get; set; }
    public int? Interval { get; set; }
    public string? Type { get; set; }
    public string? Curve { get; set; }
    public int? PlotRange { get; set; }

    public virtual RealtimeChartOptions Evaluate()
    {
        var data = Data;
        if (data?.Length > 0)
        {
            XaxisMin = data.Min(d => d.X);
            XaxisMax = data.Max(d => d.X);
            YaxisMin = data.Min(d => d.Y);
            YaxisMax = data.Max(d => d.Y).SmallestMultipleOf(factor: 10);
        }
        return this;
    }
}

public class TimelineChartDataSet
{
    public DateTime X { get; set; }
    public decimal Y { get; set; }
}

public enum ChartType
{
    Area,
    Bar,
    BoxPlot,
    Bubble,
    Candlestick,
    Heatmap,
    Line,
    Pie,
    PolarArea,
    Radar,
    RadialBar,
    RangeArea,
    Scatter,
    Treemap,
}

/// <summary>
/// In line / area charts, whether to draw smooth lines or straight lines.
/// </summary>
public enum ChartStrokeCurve
{
    /// <summary>
    /// Connect the points in straight lines.
    /// </summary>
    Straight,

    /// <summary>
    /// Uses the simplest smoothing function which produces flowing lines that are accurate.
    /// </summary>
    Smooth,

    /// <summary>
    /// Connects the points to create a monotone cubic spline.
    /// </summary>
    MonotoneCubic,

    /// <summary>
    /// Points are connected by horizontal and vertical line segments, looking like steps of a staircase.
    /// </summary>
    Stepline,
}