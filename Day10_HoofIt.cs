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

        var totalTrailsScore = trailheads.Sum(trailhead => trailhead.GetScore(map, new HashSet<(int xCoord, int yCoord)>()));
        Console.WriteLine($"Total Trails Score: {totalTrailsScore}");

        var totalTrailsRating = trailheads.Sum(trailhead => trailhead.GetScore(map, new List<(int xCoord, int yCoord)>()));
        Console.WriteLine($"Total Trails Rating: {totalTrailsRating}");
    }

    private static int GetScore<T>(this (int xCoord, int yCoord) trailhead, char[][] map, T visited)
        where T : ICollection<(int xCoord, int yCoord)> =>
        map.WalkFrom(trailhead, visited).Count(point => map.At(point) == '9');

    private static IEnumerable<(int xCoord, int yCoord)> WalkFrom<T>(this char[][] map, (int xCoord, int yCoord) trailhead, T visited)
        where T : ICollection<(int xCoord, int yCoord)>
    {
        var queue = new Queue<(int xCoord, int yCoord)>();
        queue.Enqueue(trailhead);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            switch (visited)
            {
                case HashSet<(int xCoord, int yCoord)> hashSet when !hashSet.Add(current):
                    continue;
                case List<(int xCoord, int yCoord)>:
                    visited.Add(current);
                    break;
            }


            yield return current;

            foreach (var neighbor in map.GetUphillNeighbours(current)) queue.Enqueue(neighbor);
        }
    }

    private static IEnumerable<(int xCoord, int yCoord)> GetUphillNeighbours(this char[][] map, (int xCoord, int yCoord) pointA) =>
        new[]
        {
            (pointA.xCoord - 1, pointA.yCoord), (pointA.xCoord + 1, pointA.yCoord),
            (pointA.xCoord, pointA.yCoord - 1), (pointA.xCoord, pointA.yCoord + 1)
        }.Where(pointB => map.IsUphill(pointA, pointB));

    private static bool IsUphill(this char[][] map, (int xCoord, int yCoord) pointA, (int xCoord, int yCoord) pointB) =>
        map.IsOnMap(pointA) && map.IsOnMap(pointB) && map.At(pointB) == map.At(pointA) + 1;

    private static bool IsOnMap(this char[][] map, (int xCoord, int yCoord) point) =>
        point.xCoord >= 0 && point.xCoord < map.Length &&
        point.yCoord >= 0 && point.yCoord < map[point.xCoord].Length;

    private static int At(this char[][] map, (int xCoord, int yCoord) point) =>
        map[point.xCoord][point.yCoord];

    private static IEnumerable<(int xCoord, int yCoord)> GetTrailHeads(this char[][] map) =>
        map
            .SelectMany((line, i) =>
                line.Select((height, j) =>
                    height == '0' ? (i, j) : (-1, -1)))
            .Where(point => point.Item1 != -1 && point.Item2 != -1);
}