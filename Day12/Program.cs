using Position = (int x, int y);
(int sumpart1, int sumpart2) = (0, 0);
var lines = File.ReadAllLines("input.txt");
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