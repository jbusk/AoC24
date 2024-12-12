using Position = (int x, int y);
(int sumpart1, int sumpart2) = (0, 0);
var lines = File.ReadAllLines("sample-1.txt");
Dictionary<Position, char> grid = [];
HashSet<Position> visited = [];
int max = lines.Length; // The grids axis are equal
for (int x = 0; x < max; x++)
    for (int y = 0; max > y; y++)
        grid[(x, y)] = lines[x][y];

foreach (var plot in grid)
{
    var (area, perimeter) = explorePlot1(plot.Key, plot.Value, visited, grid, max);
    sumpart1 += area * perimeter;
}

visited.Clear();
foreach (var plot in grid)
{
    if (visited.Contains(plot.Key))
        continue;
    HashSet<Position> current = [];
    var (area, perimeter) = explorePlot2(plot.Key, plot.Value, visited, current, grid, max);
    var sides = countCorners(grid, max, current);
    sumpart2 += area * sides;
}

Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);

static (int area, int perimeter) explorePlot1(Position pos, char value, HashSet<Position> visited, Dictionary<Position, char> grid, int max)
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
    foreach (var n in getNeighbours(pos))
    {
        var r = explorePlot1(n, value, visited, grid, max);
        (area, perim) = (area + r.area, perim + r.perimeter);
    }
    return (area, perim);
}


static (int area, int perimeter) explorePlot2(Position pos, char value, HashSet<Position> visited, HashSet<Position> current, Dictionary<Position, char> grid, int max)
{
    if (!inRange(pos, max))
        return (0, 1);
    var curr = grid[pos];
    if (curr != value)
        return (0, 1);
    if (visited.Contains(pos))
        return (0, 0);
    visited.Add(pos);
    current.Add(pos);
    (int area, int perim) = (1, 0);
    foreach (var n in getNeighbours(pos))
    {
        var r = explorePlot2(n, value, visited, current, grid, max);
        (area, perim) = (area + r.area, perim + r.perimeter);
    }
    return (area, perim);
}

static int countCorners(Dictionary<Position, char> grid, int max, HashSet<Position> current)
{
    // if there is only one position in the plot, there are 4 corners
    if (current.Count == 1)
        return 4;
    int count = 0;
    foreach (var pos in current)
    {
        var curr = getVal(grid, pos, max);
        var n = getAllNeighbours(pos);

        if (gv(n.lt) == curr && gv(n.rt) == curr && (curr != gv(n.up) || curr != gv(n.dn)))
            continue;
        if (gv(n.up) == curr && gv(n.dn) == curr && (curr != gv(n.lt) || curr != gv(n.rt)))
            continue;
        // check if one immediate neighbour
        if ((curr == gv(n.up) && curr != gv(n.dn) && curr != gv(n.lt) && curr != gv(n.rt)) ||
            (curr == gv(n.dn) && curr != gv(n.up) && curr != gv(n.lt) && curr != gv(n.rt)) ||
            (curr == gv(n.lt) && curr != gv(n.dn) && curr != gv(n.up) && curr != gv(n.rt)) ||
            (curr == gv(n.rt) && curr != gv(n.dn) && curr != gv(n.lt) && curr != gv(n.up)))
        {
            count += 3;
            continue;
        }
        // check if two immediate neighbours
        if (curr == gv(n.up) && curr == gv(n.lt))  // potential L // ⅃ // ⅂ // Γ
            if (curr != gv(n.ul))
            {
                count += 1;
                continue;
            }

        // L
        // +
    }
    return count;
    char gv(Position pos)
    {
        return getVal(grid, pos, max);
    }
}

static char getVal(Dictionary<Position, char> grid, Position pos, int max)
{
    if (!inRange(pos, max))
        return '\0';
    return grid[pos];
}

static (Position up, Position dn, Position lt, Position rt, Position ul, Position ur, Position dl, Position dr) getAllNeighbours(Position p)
{
    return ((p.x - 1, p.y), // up
        (p.x + 1, p.y), // down
        (p.x, p.y - 1), // left
        (p.x, p.y + 1), //right
        (p.x - 1, p.y - 1), // up-left
        (p.x - 1, p.y + 1), // up-right
        (p.x + 1, p.y - 1), // down-left
        (p.x + 1, p.y + 1)); // down-right
}

static IEnumerable<Position> getNeighbours(Position pos)
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