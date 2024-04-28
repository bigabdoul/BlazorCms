namespace BlazorCms.ApexCharts;

public static class ChartExtensions
{
    public static string ToCamelCase(this ChartType value) => value.ToString().ToCamelCase()!;
    public static string ToCamelCase(this ChartStrokeCurve value) => value.ToString().ToCamelCase()!;

    public static string? ToCamelCase(this string? s) => s == null ? null : char.ToLower(s[0]) + s[1..];

    /// <summary>
    /// Make <paramref name="value"/> the smallest multiple of <paramref name="factor"/> 
    /// where the result is either greater then <paramref name="value"/> 
    /// when positive, or lower then <paramref name="value"/> when negative.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    public static decimal SmallestMultipleOf(this decimal value, int factor)
    {
        var positive = Math.Sign(value) == 1;

        // if ymax is a positive number, e.g. ymax = 113 => YaxisMax = 120
        // if ymax is a negative number, e.g. ymax = -113 => YaxisMax = -120
        return factor * ((positive ? 1 : 0) + Math.Floor(value / factor));
    }
}