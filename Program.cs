﻿using AdventOfCode_2024;

Action[] problemSolutions =
[
    Day01_HistorianHysteria.Run,
    Day02_RedNosedReports.Run,
    Day03_MullItOver.Run,
    Day04_CeresSearch.Run,
    Day05_PrintQueue.Run,
    Day06_GuardGallivant.Run,
    Day07_BridgeRepair.Run,
    Day08_ResonantCollinearity.Run,
    Day09_DiskFragmenter.Run
];

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