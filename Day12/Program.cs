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
    int count = 0;
    foreach (var pos in current)
    {
        count += countOuterCorners(grid, pos, max);
        count += countInnerCorners(grid, pos, max);
    }
    return count;
}

static int countOuterCorners(Dictionary<Position, char> grid, Position pos, int max)
{
    // Example
    // _O_
    // OXX
    // _XX
    // if pos is the X in the middle, get the positions up and left
    // then if they both have a different value to our X, it is an outer corner
    // (also if either/both of those are outside of range)
    
    Position[][] corners = [
        [(-1, 0), (0, -1)],
        [(-1, 0), (0, 1)],
        [(1, 0), (0, 1)],
        [(1, 0), (0, 1)]];
    int count = 0;
    var curr = grid[pos];
    foreach (var pair in corners)
    {
        (Position a, Position b) = ((pos.x + pair[0].x, pos.y + pair[0].y), (pos.x + pair[1].x, pos.y + pair[1].y));
        if (inRange(a, max) && inRange(b, max))
        {
            var a_val = grid[a];
            var b_val = grid[b];
            if (curr != a_val && curr != b_val)
                count++;
        }
        else if (!inRange(a, max) && inRange(b, max))
        {
            if (curr != grid[b])
                count++;
        }
        else if (!inRange(b, max) && inRange(a, max))
        {
            if (curr != grid[a])
                count++;
        } else // we're at a grid corner
        {
            count++;
        }
    }
    return count;
}

static int countInnerCorners(Dictionary<Position, char> grid, Position pos, int max)
{
    // Example
    // OX_
    // XX_
    // ___
    // if pos is the lower right X, get the positions up and left and diagonally up-left
    // then if both the up and left positions are X and up-left is not, it is an inner corner
    // repeat for other diagonals and so on
    Position[][] corners = [
        [(-1,0), (0,-1), (-1,-1)], // up, left, upleft
        [(-1, 0), (0, 1), (-1, 1)], // up, right, upright
        [(1, 0), (0, -1), (1, -1)], // down, left, downleft
        [(1, 0), (0, 1), (1,1)]]; // down, right, downright
    int count = 0;
    var curr = grid[pos];
    foreach (var trio in corners)
    {
        (Position a, Position b, Position d) = (trio[0], trio[1], trio[2]);
        if (!inRange(d, max)) // if the diagonal is out of range, this cannot be an inner corner
            continue;
        var diagonal = grid[d];
        var a_val = grid[a];
        var b_val = grid[b];
        if (a_val == curr && b_val == curr && diagonal != curr)
            count++;
    }
    return count;
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