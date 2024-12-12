namespace AdventOfCode_2024;

public static class Day04
{
    public static void Run()
    {
        var grid = LoadGrid();
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        #region *

        var xmasCounter = 0;

        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
        {
            if (grid[i, j] != 'X') continue;

            //Horizontal Right Search
            if (j + 3 < cols)
                if (grid[i, j + 1] == 'M' && grid[i, j + 2] == 'A' && grid[i, j + 3] == 'S')
                    xmasCounter++;

            //Horizontal Left Search
            if (j >= 3)
                if (grid[i, j - 1] == 'M' && grid[i, j - 2] == 'A' && grid[i, j - 3] == 'S')
                    xmasCounter++;

            //Vertical Top Search
            if (i >= 3)
                if (grid[i - 1, j] == 'M' && grid[i - 2, j] == 'A' && grid[i - 3, j] == 'S')
                    xmasCounter++;

            //Vertical Bottom Search
            if (i + 3 < rows)
                if (grid[i + 1, j] == 'M' && grid[i + 2, j] == 'A' && grid[i + 3, j] == 'S')
                    xmasCounter++;

            //Diagonal TopLeft Search
            if (i >= 3 && j >= 3)
                if (grid[i - 1, j - 1] == 'M' && grid[i - 2, j - 2] == 'A' && grid[i - 3, j - 3] == 'S')
                    xmasCounter++;

            //Diagonal TopRight Search
            if (i >= 3 && j + 3 < cols)
                if (grid[i - 1, j + 1] == 'M' && grid[i - 2, j + 2] == 'A' && grid[i - 3, j + 3] == 'S')
                    xmasCounter++;

            //Diagonal BottomRight Search
            if (i + 3 < cols && j + 3 < rows)
                if (grid[i + 1, j + 1] == 'M' && grid[i + 2, j + 2] == 'A' && grid[i + 3, j + 3] == 'S')
                    xmasCounter++;

            //Diagonal BottomLeft Search
            if (i + 3 < rows && j >= 3)
                if (grid[i + 1, j - 1] == 'M' && grid[i + 2, j - 2] == 'A' && grid[i + 3, j - 3] == 'S')
                    xmasCounter++;
        }

        Console.WriteLine($"XMAS Counter: {xmasCounter}");

        #endregion

        #region **

        var masCounter = 0;

        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
        {
            if (grid[i, j] != 'A') continue;
            if (i >= 1 && j >= 1 && i + 1 < cols && j + 1 < rows && j + 1 < cols && i + 1 < rows &&
                (grid[i - 1, j - 1] == 'M' || grid[i + 1, j + 1] == 'M') &&
                (grid[i - 1, j - 1] == 'S' || grid[i + 1, j + 1] == 'S') &&
                (grid[i - 1, j + 1] == 'M' || grid[i + 1, j - 1] == 'M') &&
                (grid[i - 1, j + 1] == 'S' || grid[i + 1, j - 1] == 'S'))
                masCounter++;
        }

        Console.WriteLine($"X-MAS Counter: {masCounter}");

        #endregion*/
    }

    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day04_input.txt";

    private static char[,] LoadGrid() => File
        .ReadAllLines(InputFilePath)
        .Select(line => line.ToCharArray())
        .ToArray()
        .To2DArray();
}