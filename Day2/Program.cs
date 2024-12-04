Console.WriteLine("Advent of Code Day 2");

var inputFilePath = $"{Directory.GetCurrentDirectory()}/input.txt";

var lines = File.ReadAllLines(inputFilePath);

var reports = lines
    .Select(line => line
        .Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToList())
    .ToList();

var nrOfSafeReports = reports.Count(report =>
{
    var (isSafe, notSafeLevelIndex) = IsReportSafe(report);
    if (isSafe) return true;

    (isSafe, _) = IsReportSafe(report
        .Where((_, i) => i != notSafeLevelIndex)
        .ToList());
    if (isSafe) return true;

    (isSafe, _) = IsReportSafe(report
        .Where((_, i) => i != notSafeLevelIndex + 1)
        .ToList());
    if (isSafe) return true;

    if (notSafeLevelIndex == 0)
        return isSafe;

    (isSafe, _) = IsReportSafe(report
        .Where((_, i) => i != notSafeLevelIndex - 1)
        .ToList());
    return isSafe;
});

(bool isSafe, int notSafeLevelIndex) IsReportSafe(List<int> report)
{
    bool? isSafe = null;
    var sortOrder = "";
    for (var i = 0; i < report.Count - 1; i++)
    {
        if (isSafe.HasValue) break;

        if (string.IsNullOrWhiteSpace(sortOrder)) sortOrder = report[i] <= report[i + 1] ? "Ascending" : "Descending";

        if (sortOrder == "Ascending" && report[i] >= report[i + 1]) return (false, i);

        if (sortOrder == "Descending" && report[i] <= report[i + 1]) return (false, i);

        if (Math.Abs(report[i + 1] - report[i]) > 3) return (false, i);
    }

    return (true, 0);
}

Console.WriteLine($"Nr of Safe Reports: {nrOfSafeReports}");