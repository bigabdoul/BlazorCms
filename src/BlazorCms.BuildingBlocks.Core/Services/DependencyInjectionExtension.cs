using BlazorCms.BuildingBlocks.Core;
namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjectionExtension
{
    public static IServiceCollection AddCmsBuildingBlocks(this IServiceCollection services)
    {
        services.AddScoped<BuildingBlocksModuleLoader>();
        return services;
    }
}