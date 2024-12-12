using AdventOfCode_2024;

Action[] problemSolutions = [Day01.Run, Day02.Run, Day03.Run, Day04.Run, Day05.Run, Day06.Run, Day07.Run];

foreach (var index in ProblemIndices()) problemSolutions[index]();

IEnumerable<int> ProblemIndices()
{
    var prompt = $"{Environment.NewLine}Enter the day number [1-{problemSolutions.Length}] (ENTER to quit): ";
    Console.Write(prompt);
    while (true)
    {
        var input = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(input)) yield break;
        if (int.TryParse(input, out var number) && number >= 1 && number <= problemSolutions.Length) yield return number - 1;
        Console.Write(prompt);
    }
}