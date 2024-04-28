using Microsoft.JSInterop;

namespace BlazorCms.ApexCharts;

public class ApexChartModuleLoader(IJSRuntime jsRuntime) : Carfamsoft.JSInterop.LazyJsModuleLoader(jsRuntime)
{
}
