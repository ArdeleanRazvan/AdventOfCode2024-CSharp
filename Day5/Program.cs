using Day5;

Console.WriteLine("Advent of Code 2024 - Day 5");

var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "input.txt");
var lines = File.ReadAllLines(inputFilePath);

var pageRules = lines
    .Where(line => line.Contains('|'))
    .Select(line =>
    {
        var splitPages = line
            .Split('|')
            .Select(int.Parse)
            .ToList();
        return new PageRule { PageBefore = splitPages[0], PageAfter = splitPages[1] };
    })
    .ToList();

var updates = lines
    .Where(line => !line.Contains('|') && !string.IsNullOrWhiteSpace(line.Trim()))
    .Select(line => line
        .Split(',')
        .Select(int.Parse)
        .ToList())
    .ToList();

var validUpdates = updates
    .Where(IsValidUpdate)
    .ToList();

Console.WriteLine($"Number of valid updates: {validUpdates.Count}");
Console.WriteLine($"Sum of middle pages numbers: {validUpdates.Sum(update => update[update.Count / 2])}");

var orderedUpdates = updates
    .Where(update => !IsValidUpdate(update))
    .Select(OrderPages)
    .ToList();

Console.WriteLine($"Number of re-ordered updates: {orderedUpdates.Count}");
Console.WriteLine($"Sum of middle pages numbers in re-ordered updates: {orderedUpdates.Sum(update => update[update.Count / 2])}");


List<int> OrderPages(List<int> pages)
{
    var orderedPages = new List<int>(pages);
    var invalidRules = GetInvalidRules(pages);
    invalidRules.ForEach(rule =>
    {
        var indexOfBeforePage = orderedPages.IndexOf(rule.PageBefore);
        var indexOfAfterPage = orderedPages.IndexOf(rule.PageAfter);
        if (indexOfBeforePage > indexOfAfterPage)
        {
            orderedPages[indexOfBeforePage] = rule.PageAfter;
            orderedPages[indexOfAfterPage] = rule.PageBefore;
            orderedPages = OrderPages(orderedPages);
        }
    });
    return orderedPages;
}

List<PageRule> GetInvalidRules(List<int> pages)
{
    return pages
        .SelectMany(page => pageRules
            .Where(rule => (rule.PageBefore == page || rule.PageAfter == page)
                           && pages.Contains(rule.PageBefore)
                           && pages.Contains(rule.PageAfter)
            ))
        .ToList();
}

bool IsValidUpdate(List<int> pages)
{
    return pages.All(page =>
        pageRules
            .Where(rule => (rule.PageBefore == page || rule.PageAfter == page) &&
                           pages.IndexOf(rule.PageBefore) != -1 &&
                           pages.IndexOf(rule.PageAfter) != -1)
            .All(rule => pages.IndexOf(rule.PageBefore) < pages.IndexOf(rule.PageAfter))
    );
}