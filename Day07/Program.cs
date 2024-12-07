using System.Diagnostics;
var lines = File.ReadAllLines("input.txt").Select(x => x.Split(':'));
(long sumpart1, long sumpart2) = (0, 0);
long a(long x, long y) => x + y;
long m(long x, long y) => x * y;
long c(long x, long y) => long.Parse($"{x}{y}");
Stopwatch sw = Stopwatch.StartNew();
foreach (var line in lines)
{
    var expected = long.Parse(line[0]);
    var numbers = line[1].Trim().Split(' ').Select(long.Parse).Reverse();
    if (Generatesolutions(numbers, [a, m]).Any(x => x == expected))
        sumpart1 += expected;
    if (Generatesolutions(numbers, [a, m, c]).Any(x => x == expected))
        sumpart2 += expected;
}
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine("Time:   " + sw.Elapsed);
static IEnumerable<long> Generatesolutions(IEnumerable<long> numbers, IEnumerable<Func<long, long, long>> funcs)
{
    if (!numbers.Any())
        yield break;
    if (numbers.Count() == 1)
        yield return numbers.First();
    foreach (var op in funcs)
        foreach (var num in Generatesolutions(numbers.Skip(1), funcs))
            yield return op(num, numbers.First());
}
