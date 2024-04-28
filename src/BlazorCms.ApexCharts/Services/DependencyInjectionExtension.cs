using BlazorCms.ApexCharts;
namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjectionExtension
{
    public static IServiceCollection AddApexCharts(this IServiceCollection services)
    {
        services.AddScoped<ApexChartModuleLoader>();
        return services;
    }
}