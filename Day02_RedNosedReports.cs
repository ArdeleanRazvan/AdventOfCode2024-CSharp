namespace AdventOfCode_2024;

public static class Day02_RedNosedReports
{
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day02_input.txt";

    public static void Run()
    {
        var reports = LoadReports();
        Console.WriteLine($"Nr of Reports: {reports.Count}");
        var safeReportsCount = reports.Count(IsSafeReport);
        Console.WriteLine($"Nr of SafeReports: {safeReportsCount}");
        var tolerantSafeReportsCount = reports.Count(report => report
            .Expand()
            .Any(IsSafeReport));
        Console.WriteLine($"Nr of Tolerant SafeReports: {tolerantSafeReportsCount}");
    }

    private static IEnumerable<List<int>> Expand(this List<int> values) =>
        new[] { values }.Concat(Enumerable
            .Range(0, values.Count)
            .Select(values.ExceptAt));

    private static List<int> ExceptAt(this List<int> values, int index) =>
        values
            .Take(index)
            .Concat(values.Skip(index + 1))
            .ToList();

    private static bool IsSafeReport(this List<int> values) =>
        values.Count < 2 || values.IsSafeReport(Math.Sign(values[1] - values[0]));

    private static bool IsSafeReport(this List<int> values, int signValue) =>
        values
            .ToPairs()
            .All(pair => pair.IsSafeReport(signValue));

    private static IEnumerable<(int prev, int next)> ToPairs(this List<int> values) =>
        values.Zip(values.Skip(1), (prev, next) => (prev, next));

    private static bool IsSafeReport(this (int a, int b) pair, int signValue) =>
        Math.Abs(pair.b - pair.a) >= 1 && Math.Abs(pair.b - pair.a) <= 3 && Math.Sign(pair.b - pair.a) == signValue;

    private static List<List<int>> LoadReports() => File
        .ReadAllLines(InputFilePath)
        .Select(Common.ParseIntNoSign)
        .ToList();
}