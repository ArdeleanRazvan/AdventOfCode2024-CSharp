namespace AdventOfCode_2024;

public static class Day15_WarehouseWoes
{
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day15_input.txt";

    public static void Run()
    {
        var warehouse = LoadWarehouse();
        var instructions = ReadInstructions().ToList();

        var totalGps = warehouse.Simulate(instructions).ToGps();
        var scaledGps = warehouse.ScaledUp().Simulate(instructions).ToGps();

        Console.WriteLine($"       Total GPS: {totalGps}");
        Console.WriteLine($"Total scaled GPS: {scaledGps}");
    }

    private static Warehouse Simulate(this Warehouse warehouse, IEnumerable<Direction> instructions) =>
        instructions.Aggregate(warehouse.Copy(), (current, direction) => current.Move(direction));

    private static Warehouse Move(this Warehouse warehouse, Direction direction)
    {
        var pressingAt = warehouse.Robot.Move(direction);
        if (warehouse.Walls.Contains(pressingAt)) return warehouse;

        var boxesToPush = warehouse.GetPushingGroup(direction).ToList();
        if (!boxesToPush.CanMoveAll(direction, warehouse)) return warehouse;

        var boxesToStay = warehouse.Boxes.ToHashSet().Except(boxesToPush);

        return warehouse with
        {
            Boxes = boxesToPush.MoveAll(direction).Concat(boxesToStay).ToArray(),
            Robot = pressingAt
        };
    }

    private static IEnumerable<Box> MoveAll(this IEnumerable<Box> boxes, Direction direction) =>
        boxes.Select(box => box.Move(direction));

    private static bool CanMoveAll(this IEnumerable<Box> boxes, Direction direction, Warehouse warehouse) =>
        boxes.All(box => box.CanMove(direction, warehouse));

    private static IEnumerable<Box> GetPushingGroup(this Warehouse warehouse, Direction direction)
    {
        var notPushingBoxes = warehouse.Boxes.ToHashSet();
        Dictionary<Position, Box> pointToBox = warehouse.Boxes.SelectMany(GetPoints).ToDictionary();

        Queue<Position> pressurePoints = new([warehouse.Robot.Move(direction)]);

        while (pressurePoints.Count > 0)
        {
            var current = pressurePoints.Dequeue();
            if (!pointToBox.TryGetValue(current, out var pushingBox)) continue;

            if (notPushingBoxes.Contains(pushingBox))
            {
                notPushingBoxes.Remove(pushingBox);
                yield return pushingBox;
            }

            pushingBox
                .Points
                .Select(point => point.Move(direction))
                .Where(point => !pushingBox.Points.Contains(point))
                .ToList()
                .ForEach(pressurePoints.Enqueue);
        }
    }

    private static IEnumerable<(Position point, Box box)> GetPoints(this Box box) =>
        box.Points.Select(point => (point, box));

    private static HashSet<Box> ToHashSet(this IEnumerable<Box> boxes) =>
        new(boxes,
            EqualityComparer<Box>.Create(
                (a, b) => a?.Points[0] == b?.Points[0],
                box => box?.Points[0].GetHashCode() ?? 0));

    private static Box Move(this Box box, Direction direction) =>
        new(box.Points.Select(point => point.Move(direction)).ToArray());

    private static bool CanMove(this Box box, Direction direction, Warehouse warehouse) =>
        box.Points.All(point => !warehouse.Walls.Contains(point.Move(direction)));

    private static Position Move(this Position point, Direction direction) =>
        new(point.Row + direction.RowStep, point.Col + direction.ColStep);

    private static int ToGps(this Warehouse warehouse) => warehouse.Boxes.Sum(ToGps);

    private static int ToGps(this Box box) => box.Points[0].ToGps();

    private static int ToGps(this Position point) => point.Row * 100 + point.Col;

    private static Warehouse ScaledUp(this Warehouse warehouse) =>
        new(warehouse.Boxes.Select(ScaledUp).ToArray(),
            warehouse.Robot.ScaledUpRobot(),
            warehouse.Walls.SelectMany(ScaledUpWall).ToHashSet());

    private static Box ScaledUp(this Box box) => box.Points switch
    {
        [var (row, col)] => new Box([new Position(row, 2 * col), new Position(row, 2 * col + 1)]),
        _ => throw new ArgumentException("The box is already scaled")
    };

    private static Position ScaledUpRobot(this Position point) =>
        new(point.Row, 2 * point.Col);

    private static Position[] ScaledUpWall(this Position wall) =>
        [new(wall.Row, 2 * wall.Col), new(wall.Row, 2 * wall.Col + 1)];

    private static Warehouse Copy(this Warehouse warehouse) =>
        warehouse with { Boxes = warehouse.Boxes.ToArray() };

    private static Warehouse LoadWarehouse() => File.ReadAllText(InputFilePath).Split("\r\n\r\n")[0].LoadMap().LoadWarehouse();
    private static Warehouse LoadWarehouse(this char[][] map) => new(map.GetBoxes(), map.GetRobot(), map.GetWalls());

    private static Box[] GetBoxes(this char[][] map) => (
        from row in Enumerable.Range(0, map.Length)
        from col in Enumerable.Range(0, map[0].Length)
        where map[row][col] == 'O'
        select new Box([new Position(row, col)])).ToArray();

    private static Position GetRobot(this char[][] map) => (
        from row in Enumerable.Range(0, map.Length)
        from col in Enumerable.Range(0, map[0].Length)
        where map[row][col] == '@'
        select new Position(row, col)).First();

    private static HashSet<Position> GetWalls(this char[][] map) => (
        from row in Enumerable.Range(0, map.Length)
        from col in Enumerable.Range(0, map[0].Length)
        where map[row][col] == '#'
        select new Position(row, col)).ToHashSet();

    private static char[][] LoadMap(this string mapLine) => mapLine
        .Split("\r\n")
        .Select(line => line.ToCharArray())
        .ToArray();

    private static IEnumerable<Direction> ReadInstructions() => File
        .ReadAllText(InputFilePath)
        .Split("\r\n\r\n")[1]
        .Select(ToDirection)
        .Where(d => d.RowStep != 0 || d.ColStep != 0);

    private static Direction ToDirection(this char direction) => direction switch
    {
        '>' => new Direction(0, 1),
        '<' => new Direction(0, -1),
        '^' => new Direction(-1, 0),
        'v' => new Direction(1, 0),
        _ => new Direction(0, 0)
    };

    private record Box(Position[] Points);

    private record Warehouse(Box[] Boxes, Position Robot, HashSet<Position> Walls);

    private record Position(int Row, int Col);

    private record Direction(int RowStep, int ColStep);
}