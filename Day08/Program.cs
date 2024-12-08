using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
int maxpos = lines.Length;
Dictionary<(int x, int y), char> grid = [];
for (int x = 0; x < lines.Length; x++)
    for (int y = 0; y < lines[x].Length; y++)
	if (lines[x][y] != '.')
	    grid[(x, y)] = lines[x][y];
(HashSet<(int x, int y)> part1, HashSet<(int x, int y)> part2) = ([], []);
foreach (var pos in grid)
{
    var other_antennas = grid.Where(x => x.Key != pos.Key && x.Value == pos.Value);
    foreach (var antenna in other_antennas)
    {
        var ante = getAnte(pos.Key, antenna.Key);
        var resonances = getResonance(pos.Key, antenna.Key, maxpos);
	if (inRange(ante, maxpos))
	    part1.Add(ante);
        foreach (var node in resonances)
		part2.Add(node);
    }
}
sw.Stop();
Console.WriteLine("Part 1: " + part1.Count);
Console.WriteLine("Part 2: " + part2.Count);
Console.WriteLine("Total time: " + sw.Elapsed);

static bool inRange((int x, int y) pos, int max) => (pos.x < max && pos.y < max && pos.x >= 0 && pos.y >= 0);

static (int x, int y) getAnte((int x, int y) me, (int x, int y) them)
{
    var xdiff = Math.Abs(me.x - them.x);
    var ydiff = Math.Abs(me.y - them.y);
    if (them.x > me.x)
        xdiff = -1 * xdiff;
    if (them.y > me.y)
        ydiff = -1 * ydiff;
    me = (me.x + xdiff, me.y + ydiff);
    return me;
}

static IEnumerable<(int x, int y)> getResonance((int x, int y) me, (int x, int y) them, int max)
{
    HashSet<(int x, int y)> retval = [];
    var xdiff = Math.Abs(me.x - them.x);
    var ydiff = Math.Abs(me.y - them.y);
    if (them.x > me.x)
        xdiff = -1 * xdiff;
    if (them.y > me.y)
        ydiff = -1 * ydiff;
    while (inRange(me, max))
    {
        retval.Add(me);
        me = (me.x + xdiff, me.y + ydiff);
    }
    return retval;
}
