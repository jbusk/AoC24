using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
(HashSet<(int x, int y)> part1, HashSet<(int x, int y)> part2, int maxpos) = ([], [], lines.Length);
HashSet<(char, (int x, int y))> grid = [];
for (int x = 0; x < lines.Length; x++)
    for (int y = 0; y < lines[x].Length; y++)
        if (lines[x][y] != '.')
            grid.Add((lines[x][y], (x, y)));
foreach (var list in grid.ToLookup(k => k.Item1, v => v.Item2).Select(x => getPairs(x)))
{
    foreach (var pair in list)
    {
        if (getAnte(out (int x, int y) ante, pair.Item1, pair.Item2, maxpos))
            part1.Add(ante);
        if (getAnte(out ante, pair.Item2, pair.Item1, maxpos))
            part1.Add(ante);
        part2.UnionWith(getResonance(pair.Item1, pair.Item2, maxpos));
        part2.UnionWith(getResonance(pair.Item2, pair.Item1, maxpos));
    }
}

sw.Stop();
Console.WriteLine("Part 1: " + part1.Count);
Console.WriteLine("Part 2: " + part2.Count);
Console.WriteLine("Total time: " + sw.Elapsed);

static List<((int x, int y), (int x, int y))> getPairs(IEnumerable<(int x, int y)> input)
{
    int length = input.Count();
    List<((int x, int y), (int x, int y))> pairs = [];
    for (int i = 0; i < length; i++)
        for (int j = i + 1; j < length; j++)
            if (j < length)
                pairs.Add((input.ElementAt(i), input.ElementAt(j)));
    return pairs;
}

static bool inRange((int x, int y) pos, int max) => (pos.x < max && pos.y < max && pos.x >= 0 && pos.y >= 0);

static (int x, int y) calculateDiff((int x, int y) me, (int x, int y) them)
{
    (int x, int y) diff = (Math.Abs(me.x - them.x), Math.Abs(me.y - them.y));
    if (them.x > me.x)
        diff.x = -1 * diff.x;
    if (them.y > me.y)
        diff.y = -1 * diff.y;
    return diff;
}

static bool getAnte(out (int x, int y) val, (int x, int y) me, (int x, int y) them, int max)
{
    var diff = calculateDiff(me, them);
    val = (me.x + diff.x, me.y + diff.y);
    return inRange(val, max);
}

static HashSet<(int x, int y)> getResonance((int x, int y) me, (int x, int y) them, int max)
{
    HashSet<(int x, int y)> retval = [];
    var diff = calculateDiff(me, them);
    while (inRange(me, max))
    {
        retval.Add(me);
        me = (me.x + diff.x, me.y + diff.y);
    }
    return retval;
}
