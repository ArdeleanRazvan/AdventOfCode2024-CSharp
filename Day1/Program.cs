Console.WriteLine("Advent of Code Day 1 - First Star");

var inputFilePath = $"{Directory.GetCurrentDirectory()}\\input.txt";

var lines = File.ReadAllLines(inputFilePath);

var leftNumbers = new List<int>();
var rightNumbers = new List<int>();

lines
    .ToList()
    .ForEach(line =>
    {
        var numbers = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
        leftNumbers.Add(int.Parse(numbers[0]));
        rightNumbers.Add(int.Parse(numbers[1]));
    });


var pairs = leftNumbers
    .Order()
    .Zip(rightNumbers.Order(), (x, y) => (x, y));

var total = pairs.Sum(pair => pair.x > pair.y ? pair.x - pair.y : pair.y - pair.x);
Console.WriteLine($"Total is: {total}");

Console.WriteLine("Advent of Code Day 1 - 2nd Star");

var similarityScore = leftNumbers.Sum(x => x * rightNumbers.Count(y => y == x));
Console.WriteLine($"Similarity Score is: {similarityScore}");