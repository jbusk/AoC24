using System.Collections.Concurrent;
using System.Diagnostics;
var lines = File.ReadAllLines("input.txt").Select(x => x.Split(':'));
(long sumpart1, long sumpart2) = (0, 0);
long a(long x, long y) => x + y;
long m(long x, long y) => x * y;
long c(long x, long y) => long.Parse($"{x}{y}");
Stopwatch sw = Stopwatch.StartNew();
ConcurrentBag<long> p1 = [];
ConcurrentBag<long> p2 = [];
Parallel.ForEach(lines, line =>
{
    var expected = long.Parse(line[0]);
    var numbers = line[1].Trim().Split(' ').Select(long.Parse).Reverse().ToArray();
    if (Generatesolutions(numbers, [a, m]).Any(x => x == expected))
        p1.Add(expected);
    else if (Generatesolutions(numbers, [a, m, c]).Any(x => x == expected))
        p2.Add(expected);
});
sumpart1 = p1.Sum();
sumpart2 = sumpart1 + p2.Sum();
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine("Time:   " + sw.Elapsed);
static IEnumerable<long> Generatesolutions(long[] numbers, IEnumerable<Func<long, long, long>> funcs)
{
    if (numbers.Length == 1)
    {
	yield return numbers[0];
	yield break;
    }
    foreach (var op in funcs)
	foreach (var num in Generatesolutions(numbers[1..], funcs))
            yield return op(num, numbers[0]);
}
