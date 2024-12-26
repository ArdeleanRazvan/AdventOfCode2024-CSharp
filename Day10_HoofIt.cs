namespace AdventOfCode_2024;

public static class Day10_HoofIt
{
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day10_input.txt";

    private static char[][] LoadTopographicMap() => File
        .ReadAllLines(InputFilePath)
        .Select(line => line.ToCharArray())
        .ToArray();

    public static void Run()
    {
        var map = LoadTopographicMap();
        var trailheads = map.GetTrailHeads();

        var totalTrailsScore = trailheads.Sum(trailhead => trailhead.GetScore(map, new HashSet<(int row, int column)>()));
        Console.WriteLine($"Total Trails Score: {totalTrailsScore}");

        var totalTrailsRating = trailheads.Sum(trailhead => trailhead.GetScore(map, new List<(int row, int column)>()));
        Console.WriteLine($"Total Trails Rating: {totalTrailsRating}");
    }

    private static int GetScore<T>(this (int row, int column) trailhead, char[][] map, T visited)
        where T : ICollection<(int row, int column)> =>
        map.WalkFrom(trailhead, visited).Count(point => map.At(point) == '9');

    private static IEnumerable<(int row, int column)> WalkFrom<T>(this char[][] map, (int row, int column) trailhead, T visited)
        where T : ICollection<(int row, int column)>
    {
        var queue = new Queue<(int row, int column)>();
        queue.Enqueue(trailhead);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            switch (visited)
            {
                case HashSet<(int row, int column)> hashSet when !hashSet.Add(current):
                    continue;
                case List<(int row, int column)>:
                    visited.Add(current);
                    break;
            }


            yield return current;

            foreach (var neighbor in map.GetUphillNeighbours(current)) queue.Enqueue(neighbor);
        }
    }

    private static IEnumerable<(int row, int column)> GetUphillNeighbours(this char[][] map, (int row, int column) pointA) =>
        new[]
        {
            (pointA.row - 1, pointA.column), (pointA.row + 1, pointA.column),
            (pointA.row, pointA.column - 1), (pointA.row, pointA.column + 1)
        }.Where(pointB => map.IsUphill(pointA, pointB));

    private static bool IsUphill(this char[][] map, (int row, int column) pointA, (int row, int column) pointB) =>
        map.IsOnMap(pointA) && map.IsOnMap(pointB) && map.At(pointB) == map.At(pointA) + 1;

    private static bool IsOnMap(this char[][] map, (int row, int column) point) =>
        point.row >= 0 && point.row < map.Length &&
        point.column >= 0 && point.column < map[point.row].Length;

    private static int At(this char[][] map, (int row, int column) point) =>
        map[point.row][point.column];

    private static IEnumerable<(int row, int column)> GetTrailHeads(this char[][] map) =>
        map
            .SelectMany((line, i) =>
                line.Select((height, j) =>
                    height == '0' ? (i, j) : (-1, -1)))
            .Where(point => point.Item1 != -1 && point.Item2 != -1);
}