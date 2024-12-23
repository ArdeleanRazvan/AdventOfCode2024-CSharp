namespace AdventOfCode_2024;

public static class Day11_PlutonianPebbles
{
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day11_input.txt";

    private static Dictionary<(long pebble, int blinkCounter), long> Cache { get; } = new();

    public static void Run()
    {
        var pebbles = LoadPebbles();

        Console.Write("Insert number of blinks: ");
        if (int.TryParse(Console.ReadLine(), out var blinkCounter))
        {
            var pebblesCount = pebbles.CountPebbles(blinkCounter);
            Console.WriteLine($"Nr of Pebbles after blinking {blinkCounter} times: {pebblesCount}");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private static long CountPebbles(this IEnumerable<long> pebbles, int blinkCounter) => pebbles.Sum(pebble => pebble.Blink(blinkCounter));

    private static long Blink(this long pebble, int blinkCounter) =>
        Cache.TryGetValue((pebble, blinkCounter), out var pebblesCount)
            ? pebblesCount
            : Cache[(pebble, blinkCounter)] = pebble.CountAllPebbles(blinkCounter);

    private static long CountAllPebbles(this long pebble, int blinkCounter) =>
        blinkCounter == 0 ? 1 : pebble.ApplyRules().CountPebbles(blinkCounter - 1);

    private static int CountDigits(this long number) => number < 10 ? 1 : 1 + (number / 10).CountDigits();

    private static IEnumerable<long> ApplyRules(this long pebble) => pebble switch
    {
        0 => FirstRule(),
        var n when n.CountDigits() is int count && count % 2 == 0 => SecondRule(pebble, (count / 2).Power10()),
        _ => ThirdRule(pebble)
    };

    private static IEnumerable<long> FirstRule() => [1L];

    private static IEnumerable<long> SecondRule(long pebble, long divizor) =>
        [pebble / divizor, pebble % divizor];


    private static IEnumerable<long> ThirdRule(long pebble) => [pebble * 2024];


    private static long Power10(this int number) => number == 0 ? 1 : 10 * (number - 1).Power10();

    private static IEnumerable<long> LoadPebbles() =>
        File.ReadAllText(InputFilePath).ParseLongNoSign();
}