using System.Collections.Concurrent;
using System.Diagnostics;

var lines = File.ReadAllLines("input.txt");
var rulelist = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Split('|').Select(int.Parse));
var pagelist = lines.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(1).Select(x => x.Split(',').Select(int.Parse));
HashSet<(int, int)> rules = [];
foreach (var line in rulelist)
{
    rules.Add((line.First(), line.Last()));
}
var comp = new MyComparer(rules);
(ConcurrentBag<int> part1, ConcurrentBag<int> part2) = ([], []);
Stopwatch sw = Stopwatch.StartNew();
Parallel.ForEach(pagelist, pages =>
{
    var sorted = pages.OrderBy(x => x, comp);
    if (pages.SequenceEqual(sorted))
        part1.Add(halfway(sorted));
    else
        part2.Add(halfway(sorted));
    static int halfway(IEnumerable<int> pages)
    {
        return pages.ElementAt(pages.Count() / 2);
    }
});

sw.Stop();
Console.WriteLine("Part 1: " + part1.Sum());
Console.WriteLine("Part 2: " + part2.Sum());
Console.WriteLine(sw.Elapsed);

class MyComparer(HashSet<(int i, int j)> rules) : IComparer<int>
{
    public int Compare(int x, int y)
    {
        if (rules.Contains((x, y)))
            return -1;
        if (rules.Contains((y, x)))
            return 1;
        else
            return 0;
    }
}