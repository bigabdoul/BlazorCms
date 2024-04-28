using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;
using System.Reflection;

namespace BlazorCms.BuildingBlocks.Extensions;

public static class ReflectionExtensions
{
    static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> PropertyCache = [];

    /// <summary>
    /// Retrieves through reflection properties of the <paramref name="component"/>
    /// decorated with the <see cref="ParameterAttribute"/> custom attribute having
    /// <see cref="ParameterAttribute.CaptureUnmatchedValues"/> set to <see langword="false"/>.
    /// </summary>
    /// <param name="component">The component whose parameters to retrieve.</param>
    /// <returns></returns>
    public static IDictionary<string, object?> GetParameters(this IComponent component)
    {
        Dictionary<string, object?> parameters = [];

        var type = component.GetType();

        if (!PropertyCache.TryGetValue(type, out var values))
        {
            values = component.GetType().GetProperties()
                .Where(c => c.GetCustomAttributes<ParameterAttribute>().Where(p => p.CaptureUnmatchedValues == false).Any())
                .ToArray();
            PropertyCache.TryAdd(type, values);
        }

        foreach ( var prop in values)
        {
            parameters.Add(prop.Name, prop.GetValue(component));
        }

        return parameters;
    }
}