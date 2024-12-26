namespace AdventOfCode_2024;

public static class Day12_GardenGroups
{
    private const int MaxNrOfFencesPerPlot = 4;
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day12_input.txt";

    public static void Run()
    {
        var garden = LoadGardenPlots();
        var regions = GetRegions(garden);
        var totalCost = regions.Sum(region => TotalFenceCost(region, garden));
        Console.WriteLine($"Total Fence Cost *: {totalCost}");

        var discountedCost = regions.Sum(region => DiscountedFenceCost(region, garden));
        Console.WriteLine($"Discounted Fence Cost **: {discountedCost}");
    }

    private static int DiscountedFenceCost(List<(int xCoord, int yCoord)> region, char[][] garden) =>
        region.Area() * region.Sides(garden);

    private static int TotalFenceCost(this List<(int xCoord, int yCoord)> region, char[][] garden) =>
        region.Area() * region.Perimeter(garden);

    private static int Area(this List<(int xCoord, int yCoord)> region) => region.Count;

    private static int Perimeter(this List<(int xCoord, int yCoord)> region, char[][] garden) =>
        region.Sum(plot => MaxNrOfFencesPerPlot - plot.GetSameCropsNearby(garden).Count());

    private static int Sides(this List<(int xCoord, int yCoord)> region, char[][] garden) =>
        region.Perimeter(garden) - region.ContinousFences(garden);

    private static int ContinousFences(this List<(int xCoord, int yCoord)> region, char[][] garden) =>
        region.Count(plot => plot.IsContinousDown(garden)) +
        region.Count(plot => plot.IsContinousUp(garden)) +
        region.Count(plot => plot.IsContinousLeft(garden)) +
        region.Count(plot => plot.IsContinousRight(garden));

    private static bool IsSamePlot(this char[][] garden, (int xCoord, int yCoord) plotA, (int xCoord, int yCoord) plotB) =>
        garden.IsPlotInside(plotA) && garden.IsPlotInside(plotB) && garden.At(plotA) == garden.At(plotB);

    private static bool IsContinousDown(this (int xCoord, int yCoord) plot, char[][] garden) =>
        garden.IsPlotInside(plot) && garden.IsSamePlot(plot, plot.Up()) &&
        !garden.IsSamePlot(plot, plot.Up().Right()) && !garden.IsSamePlot(plot, plot.Right());

    private static bool IsContinousUp(this (int xCoord, int yCoord) plot, char[][] garden) =>
        garden.IsPlotInside(plot) && garden.IsSamePlot(plot, plot.Down()) &&
        !garden.IsSamePlot(plot, plot.Down().Left()) && !garden.IsSamePlot(plot, plot.Left());

    private static bool IsContinousLeft(this (int xCoord, int yCoord) plot, char[][] garden) =>
        garden.IsPlotInside(plot) && garden.IsSamePlot(plot, plot.Right()) &&
        !garden.IsSamePlot(plot, plot.Right().Up()) && !garden.IsSamePlot(plot, plot.Up());

    private static bool IsContinousRight(this (int xCoord, int yCoord) plot, char[][] garden) =>
        garden.IsPlotInside(plot) && garden.IsSamePlot(plot, plot.Left()) &&
        !garden.IsSamePlot(plot, plot.Left().Down()) && !garden.IsSamePlot(plot, plot.Down());


    private static (int xCoord, int yCoord) Down(this (int xCoord, int yCoord) plot) =>
        (plot.xCoord + 1, plot.yCoord);

    private static (int xCoord, int yCoord) Up(this (int xCoord, int yCoord) plot) =>
        (plot.xCoord - 1, plot.yCoord);

    private static (int xCoord, int yCoord) Left(this (int xCoord, int yCoord) plot) =>
        (plot.xCoord, plot.yCoord - 1);

    private static (int xCoord, int yCoord) Right(this (int xCoord, int yCoord) plot) =>
        (plot.xCoord, plot.yCoord + 1);

    private static IEnumerable<List<(int xCoord, int yCoord)>> GetRegions(this char[][] garden)
    {
        var uncheckedPlots = garden.GetAllPlots().ToHashSet();
        while (uncheckedPlots.Count > 0)
        {
            var currentPlot = uncheckedPlots.First();
            uncheckedPlots.Remove(currentPlot);

            var plotsToCheck = new Queue<(int xCoord, int yCoord)>();
            plotsToCheck.Enqueue(currentPlot);

            var region = new List<(int xCoord, int yCoord)>();

            while (plotsToCheck.Count > 0)
            {
                var plotToCheck = plotsToCheck.Dequeue();
                region.Add(plotToCheck);

                foreach (var sameCropNearby in plotToCheck.GetSameCropsNearby(garden).Where(uncheckedPlots.Contains))
                {
                    uncheckedPlots.Remove(sameCropNearby);
                    plotsToCheck.Enqueue(sameCropNearby);
                }
            }

            yield return region;
        }
    }

    private static IEnumerable<(int xCoord, int yCoord)> GetAllPlots(this char[][] garden) =>
        garden.SelectMany((row, i) => row.Select((col, j) => (i, j)));

    private static IEnumerable<(int xCoord, int yCoord)> GetSameCropsNearby(this (int xCoord, int yCoord) plotA, char[][] garden) =>
        new[]
        {
            (plotA.xCoord - 1, plotA.yCoord), (plotA.xCoord + 1, plotA.yCoord),
            (plotA.xCoord, plotA.yCoord - 1), (plotA.xCoord, plotA.yCoord + 1)
        }.Where(plotB => garden.SameCropNearby(plotA, plotB));

    private static bool SameCropNearby(this char[][] garden, (int xCoord, int yCoord) plotA, (int xCoord, int yCoord) plotB) =>
        garden.IsPlotInside(plotA) && garden.IsPlotInside(plotB) && garden.At(plotA) == garden.At(plotB);

    private static bool IsPlotInside(this char[][] garden, (int xCoord, int yCoord) plot) =>
        plot.xCoord >= 0 && plot.xCoord < garden.Length && plot.yCoord >= 0 && plot.yCoord < garden[0].Length;

    private static char At(this char[][] garden, (int xCoord, int yCoord) plot) => garden[plot.xCoord][plot.yCoord];

    private static char[][] LoadGardenPlots() => File
        .ReadAllLines(InputFilePath)
        .Select(line => line.ToCharArray())
        .ToArray();
}