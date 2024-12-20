namespace AdventOfCode_2024;

public static class Day06_GuardGallivant
{
    private static readonly string Orientations = "^>v<";
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day06_input.txt";

    public static void Run()
    {
        var map = LoadMap();
        var distinctPositionsCount = map
            .Path()
            .Select(state => (state.row, state.col))
            .Distinct()
            .Count();

        Console.WriteLine($"Distinct positions the guard has been in: {distinctPositionsCount}");

        var startingPosition = map.GetStartingPosition();
        var obstructionPointsCount = map
            .Path()
            .Select(state => (state.row, state.col))
            .Where(state => state != (startingPosition.row, startingPosition.col))
            .Distinct()
            .Count(state => map.WhatIf(state, map => map.ContainsLoop()));
        Console.WriteLine($"Obstruction Points that can be placed to create infinite loop: {obstructionPointsCount}");
    }

    private static bool ContainsLoop(this char[][] map) =>
        map
            .Path()
            .Count() != map
            .Path()
            .Distinct()
            .Count();

    private static T WhatIf<T>(this char[][] map, (int row, int col) newObstacle, Func<char[][], T> func)
    {
        var originalState = map[newObstacle.row][newObstacle.col];
        map[newObstacle.row][newObstacle.col] = '#';

        var result = func(map);

        map[newObstacle.row][newObstacle.col] = originalState;
        return result;
    }

    private static IEnumerable<(int row, int col, char orientation)> Path(this char[][] map)
    {
        var state = map.GetStartingPosition();
        yield return state;

        HashSet<(int row, int col, char orientation)> visitedSpots = [state];

        while (true)
        {
            state = state.orientation switch
            {
                '^' when map.ContainsObstacle(state.row - 1, state.col) =>
                    (state.row, state.col, '>'),
                '^' => (state.row - 1, state.col, state.orientation),
                '>' when map.ContainsObstacle(state.row, state.col + 1) =>
                    (state.row, state.col, 'v'),
                '>' => (state.row, state.col + 1, state.orientation),
                'v' when map.ContainsObstacle(state.row + 1, state.col) =>
                    (state.row, state.col, '<'),
                'v' => (state.row + 1, state.col, state.orientation),
                '<' when map.ContainsObstacle(state.row, state.col - 1) =>
                    (state.row, state.col, '^'),
                _ => (state.row, state.col - 1, state.orientation)
            };
            if (map.IsGuardStillInside(state.row, state.col)) yield return state;
            else yield break;

            if (!visitedSpots.Add(state)) yield break;
        }
    }

    private static bool ContainsObstacle(this char[][] map, int row, int col) =>
        map.IsGuardStillInside(row, col) && map[row][col] == '#';

    private static bool IsGuardStillInside(this char[][] map, int row, int col) =>
        row >= 0 && row < map.Length && col >= 0 && col < map[0].Length;

    private static (int row, int col, char orientation) GetStartingPosition(this char[][] map) => map
        .SelectMany((row, rowIndex) =>
            row.Select((cell, colIndex) => (rowIndex, colIndex, cell)))
        .First(tuple => Orientations.Contains(tuple.cell));

    private static char[][] LoadMap() => File
        .ReadAllLines(InputFilePath)
        .Select(line => line.ToCharArray())
        .ToArray();
}