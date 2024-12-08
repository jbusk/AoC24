using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
int maxpos = lines.Length;
Dictionary<(int x, int y), char> grid = [];
for (int x = 0; x < lines.Length; x++)
    for (int y = 0; y < lines[x].Length; y++)
        grid[(x, y)] = lines[x][y];
(HashSet<(int x, int y)> part1, HashSet<(int x, int y)> part2) = ([], []);
foreach (var pos in grid.Where(x => x.Value != '.'))
{
    var other_antennas = grid.Where(x => x.Key != pos.Key && x.Value == pos.Value);
    foreach (var antenna in other_antennas)
    {
        var anteposition = getAnteposition(pos.Key, antenna.Key);
        var resonances = getAllResonancePositions(pos.Key, antenna.Key, maxpos);
	if (inRange(anteposition, maxpos))
	    part1.Add(anteposition);
        foreach (var resonance in resonances)
            if (grid.ContainsKey(resonance)) 
		part2.Add(resonance);
    }
}
sw.Stop();
Console.WriteLine("Part 1: " + part1.Count);
Console.WriteLine("Part 2: " + part2.Count);
Console.WriteLine("Total time: " + sw.Elapsed);

static bool inRange((int x, int y) pos, int max) => (pos.x < max && pos.y < max && pos.x >= 0 && pos.y >= 0);

static (int x, int y) getAnteposition((int x, int y) me, (int x, int y) them)
{
    var xdiff = Math.Max(me.x, them.x) - Math.Min(me.x, them.x);
    var ydiff = Math.Max(me.y, them.y) - Math.Min(me.y, them.y);
    if (them.x > me.x)
        xdiff = -1 * xdiff;
    if (them.y > me.y)
        ydiff = -1 * ydiff;
    me = (me.x + xdiff, me.y + ydiff);
    return me;
}

static IEnumerable<(int x, int y)> getAllResonancePositions((int x, int y) me, (int x, int y) them, int max)
{
    List<(int x, int y)> retval = [me];
    var xdiff = Math.Max(me.x, them.x) - Math.Min(me.x, them.x);
    var ydiff = Math.Max(me.y, them.y) - Math.Min(me.y, them.y);
    if (them.x > me.x)
        xdiff = -1 * xdiff;
    if (them.y > me.y)
        ydiff = -1 * ydiff;
    while (inRange(me, max))
    {
        me = (me.x + xdiff, me.y + ydiff);
        retval.Add(me);
    }
    return retval;
}
