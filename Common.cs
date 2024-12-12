using System.Text.RegularExpressions;

namespace AdventOfCode_2024;

internal static class Common
{
    /// <summary>
    ///     Converts a list with exactly two elements into a tuple of two elements
    /// </summary>
    /// <param name="list">list to convert</param>
    /// <typeparam name="T">type of data</typeparam>
    /// <returns>Tuple of elements</returns>
    /// <exception cref="ArgumentException"></exception>
    public static (T a, T b) ToPair<T>(this List<T> list) => list switch
    {
        [T a, T b] => (a, b),
        _ => throw new ArgumentException()
    };

    /// <summary>
    ///     Transposes a jagged array into a two-dimensional array
    /// </summary>
    /// <param name="source">Jagged array to transpose</param>
    /// <param name="rows">number of rows</param>
    /// <param name="cols">number of columns</param>
    /// <typeparam name="T">Type of data</typeparam>
    /// <returns>A two-dimensional array</returns>
    public static T[,] To2DArray<T>(this T[][] source)
    {
        var rows = source.Length;
        var cols = source[0].Length;
        var result = new T[rows, cols];
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
            result[i, j] = source[i][j];
        return result;
    }

    public static List<int> ParseIntNoSign(this string line) => Regex
        .Matches(line, @"\d+")
        .Select(match => int.Parse(match.Value))
        .ToList();

    public static List<long> ParseLongNoSign(this string line) => Regex
        .Matches(line, @"\d+")
        .Select(match => long.Parse(match.Value))
        .ToList();
}