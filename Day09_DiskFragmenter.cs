namespace AdventOfCode_2024;

public static class Day09_DiskFragmenter
{
    private static readonly string InputFilePath = $"{Directory.GetCurrentDirectory()}\\inputs\\day09_input.txt";

    public static void Run()
    {
        var diskMap = LoadDiskMap().ToList();
        var moveBlocksChecksum = diskMap.Compact(MoveBlocks).Sum(file => file.Checksum);
        var moveFilesChecksum = diskMap.Compact(MoveFiles).Sum(file => file.Checksum);

        Console.WriteLine($"Moving blocks checksum: {moveBlocksChecksum}");
        Console.WriteLine($"Moving files checksum: {moveFilesChecksum}");
    }

    private static IEnumerable<FileSection> Compact(this IEnumerable<Fragment> fragments, BlocksConstraint blocksConstraint)
    {
        var files = fragments.OfType<FileSection>().OrderByDescending(file => file.Position);
        var gaps = fragments.OfType<Gap>().OrderBy(gap => gap.Position).ToList();

        foreach (var file in files)
        {
            var remainingGaps = new List<Gap>();
            var blocksNotMoved = file.BlockSize;

            foreach (var gap in gaps.Where(gap => gap.Position < file.Position))
            {
                var blocksToMove = Math.Min(blocksNotMoved, gap.BlockSize);

                if (blocksToMove < blocksConstraint(file))
                {
                    remainingGaps.Add(gap);
                    continue;
                }

                if (blocksToMove > 0) yield return file with { Position = gap.Position, BlockSize = blocksToMove };
                blocksNotMoved -= blocksToMove;

                if (gap.Remove(blocksToMove) is Gap remainingGap) remainingGaps.Add(remainingGap);
            }

            if (blocksNotMoved > 0) yield return file with { BlockSize = blocksNotMoved };
            gaps = remainingGaps;
        }
    }

    private static int MoveBlocks(FileSection file) => 0;

    private static int MoveFiles(FileSection file) => file.BlockSize;

    private static IEnumerable<Fragment> LoadDiskMap()
    {
        var position = 0;
        foreach (var (fileId, blocks) in ReadSpec())
        {
            if (fileId.HasValue) yield return new FileSection(fileId.Value, position, blocks);
            else yield return new Gap(position, blocks);
            position += blocks;
        }
    }

    private static IEnumerable<(int? fileId, int blocks)> ReadSpec() => File
        .ReadAllText(InputFilePath)
        .Select(c => c - '0')
        .Select((value, i) => (i % 2 == 0 ? (int?)(i / 2) : null, value));

    private delegate int BlocksConstraint(FileSection file);

    private abstract record Fragment(int Position, int BlockSize);

    private record FileSection(int FileId, int Position, int BlockSize) : Fragment(Position, BlockSize)
    {
        public long Checksum => (long)FileId * BlockSize * (2 * Position + BlockSize - 1) / 2;
    }

    private record Gap(int Position, int BlockSize) : Fragment(Position, BlockSize)
    {
        public Gap? Remove(int blocks) =>
            blocks >= BlockSize ? null : new Gap(Position + blocks, BlockSize - blocks);
    }
}