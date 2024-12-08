using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
(int sumpart1, int sumpart2, int maxpos) = (0, 0, lines.Length);
Dictionary<(int x, int y), Position> grid = [];
for (int x = 0; x < lines.Length; x++)
    for (int y = 0; y < lines[x].Length; y++)
        grid[(x, y)] = new Position(lines[x][y]);

foreach (var pos in grid.Where(x => x.Value.Antenna))
{
    var other_antennas = grid.Where(x => x.Key != pos.Key && x.Value.Value == pos.Value.Value);
    foreach (var antenna in other_antennas)
    {
        var anteposition = getAnteposition(pos.Key, antenna.Key);
        var resonances = getAllResonancePositions(pos.Key, antenna.Key, maxpos);
        if (grid.TryGetValue(anteposition, out Position? antegrid) && antegrid is not null)
            antegrid.Antinode = true;
        foreach (var resonance in resonances)
            if (grid.TryGetValue(resonance, out antegrid) && antegrid is not null)
                antegrid.Resonance = true;
    }
}
sw.Stop();
Console.WriteLine("Part 1: " + grid.Count(x => x.Value.Antinode));
Console.WriteLine("Part 2: " + grid.Count(x => x.Value.Resonance));
Console.WriteLine("Total time: " + sw.Elapsed);

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
    while (true)
    {
        me = (me.x + xdiff, me.y + ydiff);
        if (me.x > max || me.y > max || me.x < 0 || me.y < 0)
            break;
        retval.Add(me);
    }
    return retval;
}

public class Position(char value)
{
    public char Value { get; set; } = value;
    public bool Antinode { get; set; }
    public bool Resonance { get; set; }
    public bool Antenna => (Value != '.');
}