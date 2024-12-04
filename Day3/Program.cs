using System.Text.RegularExpressions;

Console.WriteLine("Advent of Code 2024 - Day 3");
var regexPattern = @"\b(?:mul\((\d+),(\d+)\)|do\(\)|don\'t\(\))";
var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "input.txt");
var corruptedText = File.ReadAllText(inputFilePath);

bool? canExecute = null;
List<(int x, int y)> filteredMatches = Regex
    .Matches(corruptedText, regexPattern)
    .Select(match =>
    {
        if (match.Value == "do()")
        {
            canExecute = true;
            return (0, 0);
        }

        if (match.Value == "don't()")
        {
            canExecute = false;
            return (0, 0);
        }

        if (canExecute == null || canExecute == true)
            return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));

        return (0, 0);
    })
    .Where(match => match is { Item1: > 0, Item2: > 0 })
    .ToList();

var total = filteredMatches.Sum(pair => pair.x * pair.y);
Console.WriteLine($"Total: {total}");