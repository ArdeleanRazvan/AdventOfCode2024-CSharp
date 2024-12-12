namespace AdventOfCode_2024;

public static class Day05
{
    public static void Run()
    {
        var pageRules = LoadPageRules();
        var updates = LoadUpdates();

        var sumOfMiddlePages = updates
            .Where(update => IsValidUpdate(update, pageRules))
            .Sum(pages => pages[pages.Count / 2]);
        Console.WriteLine($"Valid updates middle pages sum: {sumOfMiddlePages}");

        var sumOfOrderedMiddlePages = updates
            .Where(update => !IsValidUpdate(update, pageRules))
            .Select(update => OrderPages(update, pageRules))
            .Sum(pages => pages[pages.Count / 2]);
        Console.WriteLine($"Ordered updates middle pages sum: {sumOfOrderedMiddlePages}");
    }

    private static List<int> OrderPages(List<int> pages, IEnumerable<(int before, int after)> pageRules)
    {
        var orderedPages = new List<int>(pages);
        var invalidRules = pages
            .SelectMany(page => pageRules
                .Where(rule => (rule.before == page || rule.after == page)
                               && pages.Contains(rule.before)
                               && pages.Contains(rule.after)
                ))
            .ToList();
        invalidRules.ForEach(rule =>
        {
            var indexOfBeforePage = orderedPages.IndexOf(rule.before);
            var indexOfAfterPage = orderedPages.IndexOf(rule.after);
            if (indexOfBeforePage > indexOfAfterPage)
            {
                orderedPages[indexOfBeforePage] = rule.after;
                orderedPages[indexOfAfterPage] = rule.before;
                orderedPages = OrderPages(orderedPages, pageRules);
            }
        });
        return orderedPages;
    }

    private static bool IsValidUpdate(this List<int> pages, IEnumerable<(int before, int after)> pageRules) =>
        pages.All(page => IsValidPage(page, pages, pageRules));

    private static bool IsValidPage(int page, List<int> pages, IEnumerable<(int before, int after)> pageRules) => pageRules
        .Where(rule => (rule.before == page || rule.after == page) &&
                       pages.IndexOf(rule.before) != -1 &&
                       pages.IndexOf(rule.after) != -1)
        .All(rule => pages.IndexOf(rule.before) < pages.IndexOf(rule.after));


    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day05_input.txt";

    private static (int before, int after) ToTuple(this string line)
    {
        var parts = line.Split("|");
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }

    private static List<(int before, int after)> LoadPageRules() => File
        .ReadAllLines(InputFilePath)
        .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
        .Select(ToTuple)
        .ToList();

    private static List<List<int>> LoadUpdates() => File
        .ReadAllLines(InputFilePath)
        .Select(Common.ParseIntNoSign)
        .Where(pages => pages.Count > 2)
        .ToList();
}