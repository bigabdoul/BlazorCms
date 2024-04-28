using Microsoft.AspNetCore.Components;

namespace BlazorCms.BuildingBlocks;

public abstract class BreakpointBlockComponentBase : BlockComponentBase
{
#pragma warning disable IDE1006
    [Parameter] public int? sm { get; set; }
    [Parameter] public int? md { get; set; }
    [Parameter] public int? lg { get; set; }
    [Parameter] public int? xl { get; set; }
    [Parameter] public int? xxl { get; set; }
    [Parameter] public int? all { get; set; }
    [Parameter] public int? offsetsm { get; set; }
    [Parameter] public int? offsetmd { get; set; }
    [Parameter] public int? offsetlg { get; set; }
    [Parameter] public int? offsetxl { get; set; }
    [Parameter] public int? offsetxxl { get; set; }
#pragma warning restore IDE1006
    protected string GetBreakpoints(string prefix = "", string? suffixWhenNone = null)
    {
        var sb = string.Empty;

        if (all != null) sb += $"{prefix}-{all} ";
        if (sm != null) sb += $"{prefix}-sm-{sm} ";
        if (md != null) sb += $"{prefix}-md-{md} ";
        if (lg != null) sb += $"{prefix}-lg-{lg} ";
        if (xl != null) sb += $"{prefix}-xl-{xl} ";
        if (xxl != null) sb += $"{prefix}-xxl-{xxl} ";

        return sb != string.Empty ? sb.Trim() : prefix + suffixWhenNone;
    }

    protected string GetOffsets()
    {
        var sb = string.Empty;

        if (offsetsm != null) sb += $" offset-sm-{offsetsm}";
        if (offsetmd != null) sb += $" offset-md-{offsetmd}";
        if (offsetlg != null) sb += $" offset-lg-{offsetlg}";
        if (offsetxl != null) sb += $" offset-xl-{offsetxl}";
        if (offsetxxl != null) sb += $" offset-xxl-{offsetxxl}";

        return sb;
    }
}