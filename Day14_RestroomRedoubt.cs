namespace AdventOfCode_2024;

public static class Day14_RestroomRedoubt
{
    private const int RoomWidth = 101;
    private const int RoomHeight = 103;
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day14_input.txt";

    public static void Run()
    {
        var robots = LoadRobots().ToList();
        var safetyFactor = robots.Move(100).ToList().CalculateSafetyFactor();
        Console.WriteLine($"Safety Factor: {safetyFactor}");

        foreach (var candidate in robots.GetChristmasTreeCandidates())
        {
            Console.WriteLine($"Christmas Tree Easter Egg found after {candidate.time} seconds.");
            break;
        }
    }

    private static IEnumerable<(List<Robot> robotsState, int time)> GetChristmasTreeCandidates(this List<Robot> robots)
    {
        var time = 0;
        while (true)
        {
            if (time == int.MaxValue) yield break;
            time++;
            if (robots.Move(time).MaybeChristmasTree()) yield return (robots.Move(time).ToList(), time);
        }
    }

    private static bool MaybeChristmasTree(this IEnumerable<Robot> robots) =>
        robots.GetGroupSizes().Max() >= robots.Count() / 3;

    private static IEnumerable<int> GetGroupSizes(this IEnumerable<Robot> robots)
    {
        var positions = robots.Select(robot => robot.Position).Distinct().ToHashSet();
        while (positions.Count > 0)
        {
            var startPos = positions.First();
            positions.Remove(startPos);

            var positionsToCheck = new Queue<Position>();
            positionsToCheck.Enqueue(startPos);

            var groupSize = 0;

            while (positionsToCheck.Count > 0)
            {
                var currentPos = positionsToCheck.Dequeue();
                groupSize++;

                var neighbours = new[]
                {
                    currentPos with { X = currentPos.X - 1 }, currentPos with { X = currentPos.X + 1 },
                    currentPos with { Y = currentPos.Y - 1 }, currentPos with { Y = currentPos.Y + 1 }
                };

                foreach (var neighbour in neighbours.Where(positions.Contains))
                {
                    positions.Remove(neighbour);
                    positionsToCheck.Enqueue(neighbour);
                }
            }

            yield return groupSize;
        }
    }

    private static int CalculateSafetyFactor(this List<Robot> robots) =>
        robots.Count(robot => robot.InFirstQuadrant()) *
        robots.Count(robot => robot.InSecondQuadrant()) *
        robots.Count(robot => robot.InThirdQuadrant()) *
        robots.Count(robot => robot.InFourthQuadrant());

    private static bool InFirstQuadrant(this Robot robot) =>
        robot.Position is { X: < RoomWidth / 2, Y: < RoomHeight / 2 };

    private static bool InSecondQuadrant(this Robot robot) =>
        robot.Position is { X: > RoomWidth / 2, Y: < RoomHeight / 2 };

    private static bool InThirdQuadrant(this Robot robot) =>
        robot.Position is { X: < RoomWidth / 2, Y: > RoomHeight / 2 };

    private static bool InFourthQuadrant(this Robot robot) =>
        robot.Position is { X: > RoomWidth / 2, Y: > RoomHeight / 2 };

    private static IEnumerable<Robot> Move(this IEnumerable<Robot> robots, int seconds) =>
        robots.Select(robot => robot.Move(seconds));

    private static Robot Move(this Robot robot, int seconds) =>
        robot with { Position = robot.Position.Move(robot.Velocity, seconds) };

    private static Position Move(this Position position, Velocity velocity, int seconds) =>
        new(position.X.Move(velocity.X, seconds, RoomWidth), position.Y.Move(velocity.Y, seconds, RoomHeight));

    private static int Move(this int position, int velocity, int seconds, int roomSize) =>
        ((position + velocity * seconds) % roomSize + roomSize) % roomSize;

    private static IEnumerable<Robot> LoadRobots() => File
        .ReadAllLines(InputFilePath)
        .SelectMany(Common.ParseInt)
        .Chunk(4)
        .Select(data => new Robot(new Position(data[0], data[1]), new Velocity(data[2], data[3])));

    private record Robot(Position Position, Velocity Velocity);

    private record Velocity(int X, int Y);

    private record Position(int X, int Y);
}