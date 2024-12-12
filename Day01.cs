using System.Text.RegularExpressions;

namespace AdventOfCode_2024;

public static class Day01
{
    public static void Run()
    {
        var (left, right) = LoadLists();

        var dist = left
            .Order()
            .Zip(right.Order(), (x, y) => Math.Abs(x - y))
            .Sum();

        var similarity = right
            .Where(left.Contains)
            .GroupBy(x => x)
            .Sum(group => group.Key * group.Count());

        Console.WriteLine($"Total items: {left.Count}");
        Console.WriteLine($"Distance: {dist}");
        Console.WriteLine($"Similarity: {similarity}");
    }

    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day01_input.txt";

    private static (List<int> left, List<int> right) LoadLists()
    {
        var left = new List<int>();
        var right = new List<int>();
        File
            .ReadAllLines(InputFilePath)
            .Select(Common.ParseIntNoSign)
            .ToList()
            .ForEach(pair =>
            {
                left.Add(pair[0]);
                right.Add(pair[1]);
            });
        return (left, right);
    }
}