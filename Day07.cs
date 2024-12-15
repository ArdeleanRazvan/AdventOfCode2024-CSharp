using System.Text.RegularExpressions;

namespace AdventOfCode_2024;

public static class Day07
{
    public static void Run()
    {
        var equations = LoadEquations();

        var calibrationResult = equations
            .Where(IsValidEquation)
            .ToList()
            .Sum(equation => equation.testValue);
        Console.WriteLine($"Calibration result for *: {calibrationResult}");

        Operators = Operators.Concat(['|']).ToList();
        calibrationResult = equations
            .Where(IsValidEquation)
            .ToList()
            .Sum(equation => equation.testValue);
        Console.WriteLine($"Calibration result for **: {calibrationResult}");
    }

    private static List<char> Operators = ['+', '*'];

    private static bool IsValidEquation(this (long testValue, int[] numbers) equation)
    {
        var validCombinations = Enumerable
            .Repeat(Operators, equation.numbers.Length - 1)
            .Aggregate(
                new[] { "" }.AsEnumerable(),
                (acc, current) => acc.SelectMany(combo => current.Select(op => combo + op))
            )
            .ToList();

        return
            validCombinations
                .Any(combo =>
                {
                    var result = combo
                        .Select((op, i) => op switch
                        {
                            '+' => (Func<long, long>)(x => x + equation.numbers[i + 1]),
                            '*' => x => x * equation.numbers[i + 1],
                            '|' => (Func<long, long>)(x => long.Parse($"{x}{equation.numbers[i + 1]}"))
                        })
                        .Aggregate((long)equation.numbers[0], (acc, operation) => operation(acc));
                    return result == equation.testValue;
                });
    }

    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day07_input.txt";

    private static List<(long testValue, int[] numbers)> LoadEquations() => Regex
        .Matches(File.ReadAllText(InputFilePath), @"(?<testValue>\d+)\:\s(?<numbers>.+)")
        .Select(match => (
                long.Parse(match.Groups["testValue"].Value),
                match
                    .Groups["numbers"]
                    .Value.Split(' ')
                    .Select(int.Parse)
                    .ToArray()
            )
        )
        .ToList();
}