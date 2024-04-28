namespace BlazorCms.BuildingBlocks.Extensions;
public static class CollectionExtensions
{
    /// <summary>
    /// Makes sure that only HTML attributes with non-whitespace values are returned for the name/value tuples.
    /// </summary>
    /// <param name="parameters">A collection of name/value tuples.</param>
    /// <returns></returns>
    public static IDictionary<string, object> GetAttributes(params (string name, object? value)[]? parameters)
    {
        var attributes = new Dictionary<string, object>();

        if (parameters?.Length > 0)
            foreach (var (name, value) in parameters)
                if (!string.IsNullOrWhiteSpace($"{value}"))
                    attributes.Add(name, value!);

        return attributes;
    }

    public static IDictionary<string, object> GetAttributes(params (string name, object? value, bool condition)[]? parameters)
    {
        var attributes = new Dictionary<string, object>();
        if (parameters?.Length > 0)
        {
            foreach (var (name, value, condition) in parameters)
            {
                if (condition && !string.IsNullOrWhiteSpace($"{value}") && condition)
                    attributes.Add(name, value!);
            }
        }
        return attributes;
    }

    public static IDictionary<string, object> GetAttributes(params (string name, object? value, Func<bool> condition)[]? parameters)
    {
        var attributes = new Dictionary<string, object>();
        if (parameters?.Length > 0)
        {
            foreach (var (name, value, condition) in parameters)
            {
                if (!string.IsNullOrWhiteSpace($"{value}") && condition())
                    attributes.Add(name, value!);
            }
        }
        return attributes;
    }

    /// <summary>
    /// Merges the <paramref name="source"/> dictionary with the <paramref name="target"/> dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="target">The target dictionary.</param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue>? source, IDictionary<TKey, TValue>? target)
    {
        if (source == null)
            return target ?? throw new ArgumentNullException(nameof(target),
                $"{nameof(source)} and {nameof(target)} cannot be null.");

        if (target == null)
            return source;

        foreach (var kv in source)
        {
            var value = kv.Value;

            if (value is null || (value is string s && string.IsNullOrWhiteSpace(s)))
                continue;

            if (string.Equals(kv.Key?.ToString(), "class", StringComparison.OrdinalIgnoreCase))
            {
                target.TryGetValue(kv.Key, out var existing);
                var cls = (TValue)(object)CssClassBuilder.Default(value.ToString())
                    .AddClass(existing?.ToString())
                    .Build();
                target[kv.Key] = cls;
            }
            else
            {
                target[kv.Key] = value;
            }
        }

        return target;
    }


    /// <summary>
    /// Returns the first item in <paramref name="values"/> whose 
    /// value is not the default of the <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">The type of values.</typeparam>
    /// <param name="values">A collection of values to test.</param>
    /// <returns></returns>
    public static T? Coalesce<T>(this IEnumerable<T?>? values)
    {
        if (values?.Any() == true)
        {
            foreach (var item in values)
            {
                if (!EqualityComparer<T>.Default.Equals(item, default))
                    return item;
            }
        }

        return default;
    }

    /// <summary>
    /// Returns <see langword="true"/> for the first item in <paramref name="values"/> 
    /// whose value satisfies the provided <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The type of values.</typeparam>
    /// <param name="values">A collection of values to test.</param>
    /// <param name="predicate">A function that tests each item in <paramref name="values"/>.</param>
    /// <returns></returns>
    public static bool Coalesce<T>(this IEnumerable<T?>? values, Func<T?, bool> predicate)
    {
        if (values?.Any() == true)
        {
            foreach (var value in values)
            {
                if (predicate(value))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns a <see cref="Tuple{T1, T2}"/> for the first item in <paramref name="values"/> 
    /// whose value satisfies the provided <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The type of values.</typeparam>
    /// <param name="values">A collection of values to test.</param>
    /// <param name="predicate">A function that tests each item in <paramref name="values"/>.</param>
    /// <returns></returns>
    /// <remarks>
    /// The components of <see cref="Tuple{T1, T2}"/> are of types <see cref="bool"/> and <typeparamref name="T"/> respectively.
    /// </remarks>
    public static (bool Success, T? Value) CoalesceValue<T>(this IEnumerable<T?>? values, Func<T?, bool> predicate)
    {
        if (values?.Any() == true)
        {
            foreach (var value in values)
            {
                if (predicate(value))
                    return (true, value);
            }
        }
        return default;
    }
}
