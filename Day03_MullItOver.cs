using System.Text.RegularExpressions;

namespace AdventOfCode_2024;

public static class Day03_MullItOver
{
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day03_input.txt";

    public static void Run()
    {
        var input = File.ReadAllText(InputFilePath);

        var sumOfMuls = Regex
            .Matches(input, @"\bmul\((\d+),(\d+)\)")
            .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
        Console.WriteLine($"First Star result: {sumOfMuls}");

        var canExecute = true;
        var sumOfExecutableMuls = Regex
            .Matches(input, @"\b(?:mul\((\d+),(\d+)\)|do\(\)|don\'t\(\))")
            .Where(match =>
            {
                switch (match.Value)
                {
                    case "do()":
                        canExecute = true;
                        return false;
                    case "don't()":
                        canExecute = false;
                        return false;
                    default: return canExecute;
                }
            })
            .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
        Console.WriteLine($"Second Star result: {sumOfExecutableMuls}");
    }
}