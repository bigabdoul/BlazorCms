using Microsoft.JSInterop;

namespace BlazorCms.BuildingBlocks.Core;

public class BuildingBlocksModuleLoader(IJSRuntime jsRuntime) : Carfamsoft.JSInterop.LazyJsModuleLoader(jsRuntime)
{
}
