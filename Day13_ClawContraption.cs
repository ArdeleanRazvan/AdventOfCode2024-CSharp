namespace AdventOfCode_2024;

public static class Day13_ClawContraption
{
    private const int ButtonACost = 3;
    private const int ButtonBCost = 1;
    private const long ConversionError = 10_000_000_000_000;
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day13_input.txt";

    public static void Run()
    {
        var machines = LoadMachines().ToList();

        var tokens = machines
            .SelectMany(CheapestWinnableCombo)
            .Sum(ToTokens);
        Console.WriteLine($"Minimum required tokens to get winnable prizes are: {tokens}");

        var correctedTokens = machines
            .Select(ToCorrectedMachine)
            .SelectMany(CheapestWinnableCombo)
            .Sum(ToTokens);
        Console.WriteLine($"Minimum required tokens to get winnable prizes after correction are: {correctedTokens}");
    }

    private static Machine ToCorrectedMachine(this Machine machine) =>
        machine with { Prize = new Prize(machine.Prize.X + ConversionError, machine.Prize.Y + ConversionError) };

    private static IEnumerable<(long pressesA, long pressesB)> CheapestWinnableCombo(this Machine machine)
    {
        //See day13_logic.md file for formula and math logic, it was generated with ChatGPT
        long determinant = machine.ButtonA.MoveX * machine.ButtonB.MoveY - machine.ButtonA.MoveY * machine.ButtonB.MoveX;
        return determinant == 0 ? [] : machine.ToButtonPresses(determinant);
    }

    private static long ToTokens(this (long pressesA, long pressesB) combo) =>
        combo.pressesA * ButtonACost + combo.pressesB * ButtonBCost;

    private static IEnumerable<Machine> LoadMachines() => File
        .ReadAllLines(InputFilePath)
        .SelectMany(Common.ParseIntNoSign)
        .Chunk(6)
        .Select(data => new Machine(
            new Button(data[0], data[1]),
            new Button(data[2], data[3]),
            new Prize(data[4], data[5])
        ));

    private record Machine(Button ButtonA, Button ButtonB, Prize Prize)
    {
        public IEnumerable<(long pressesA, long pressesB)> ToButtonPresses(long determinant)
        {
            var pressesA = (Prize.X * ButtonB.MoveY - Prize.Y * ButtonB.MoveX) / determinant;
            var pressesB = (Prize.X - pressesA * ButtonA.MoveX) / ButtonB.MoveX;

            if (pressesA * ButtonA.MoveX + pressesB * ButtonB.MoveX != Prize.X) yield break;
            if (pressesA * ButtonA.MoveY + pressesB * ButtonB.MoveY != Prize.Y) yield break;

            yield return (pressesA, pressesB);
        }
    }

    private record Button(int MoveX, int MoveY);

    private record Prize(long X, long Y);
}