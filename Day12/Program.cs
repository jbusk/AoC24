using Position = (int x, int y);
(int sumpart1, int sumpart2) = (0, 0);
var lines = File.ReadAllLines("input.txt");
Dictionary<Position, char> grid = [];
HashSet<Position> visited = [];
List<HashSet<Position>> regions = [];
int max = lines.Length; 
for (int x = 0; x < max; x++)
    for (int y = 0; max > y; y++)
        grid[(x, y)] = lines[x][y];

foreach (var plot in grid)
{
    if (visited.Contains(plot.Key))
        continue;
    HashSet<Position> current = [];
    Dictionary<Position, int> neighbourtouches = [];
    var (area, perimeter) = explorePlot(plot.Key, plot.Value, current, grid, max);
    sumpart1 += area * perimeter;
    var sides = countCorners(current);
    sumpart2 += area * sides;
    visited.UnionWith(current);
}

Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + (sumpart2));

static (int area, int perimeter) explorePlot(Position pos, char value, HashSet<Position> visited, Dictionary<Position, char> grid, int max)
{
    if (!inRange(pos, max))
        return (0, 1);
    var curr = grid[pos];
    if (curr != value)
        return (0, 1);
    if (visited.Contains(pos))
        return (0, 0);
    visited.Add(pos);
    (int area, int perim) = (1, 0);
    foreach (var n in getImmediateNeighbours(pos))
    {
        var r = explorePlot(n, value, visited, grid, max);
        (area, perim) = (area + r.area, perim + r.perimeter);
    }
    return (area, perim);
}

static int countCorners(HashSet<Position> region)
{
    int corners = 0;
    foreach (var pos in region)
    {
        var n = getAllNeighbours(pos);
        (bool up, bool dn, bool lt, bool rt, bool ul, bool ur, bool dl, bool dr) =
            (c(n.up), c(n.dn), c(n.lt), c(n.rt), c(n.ul), c(n.ur), c(n.dl), c(n.dr));
        if (up && lt && !ul) // inside upleft corner
            corners++;
        if (!up && !lt) // outside upleft corner
            corners++;
        if (up && rt && !ur) // inside upright corner
            corners++;
        if (!up && !rt) // outside upright corner
            corners++;
        if (dn && lt && !dl) //inside downleft corner
            corners++;
        if (!dn && !lt) //outside downleft corner
            corners++;
        if (dn && rt && !dr) //inside downright corner
            corners++;
        if (!dn && !rt) //outside downright corner
            corners++;
    }
    return corners;
    bool c(Position p) => region.Contains(p);
}

static (Position up, Position dn, Position lt, Position rt, Position ul, Position ur, Position dl, Position dr) getAllNeighbours(Position p)
{
    return (
        (p.x - 1, p.y), // up
        (p.x + 1, p.y), // down
        (p.x, p.y - 1), // left
        (p.x, p.y + 1), //right
        (p.x - 1, p.y - 1), // up-left
        (p.x - 1, p.y + 1), // up-right
        (p.x + 1, p.y - 1), // down-left
        (p.x + 1, p.y + 1)); // down-right
}

static IEnumerable<Position> getImmediateNeighbours(Position pos)
{
    Position[] rels = [(1, 0), (0, 1), (-1, 0), (0, -1)];
    foreach (var (x, y) in rels)
    {
        var npos = (pos.x + x, pos.y + y);
        yield return npos;
    }
    yield break;
}

static bool inRange(Position pos, int max) => (pos.x < max && pos.x >= 0 && pos.y < max && pos.y >= 0);
