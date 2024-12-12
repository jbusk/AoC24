using Position = (int x, int y);
(int sumpart1, int sumpart2) = (0, 0);
var lines = File.ReadAllLines("sample.txt");
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
    //  OOO
    //  OXO   = 4 region om 1 element, så vi returnerar 4 direkt
    //  OOO
    if (current.Count == 1)
        return 4;
    int count = 0;

    foreach (var pos in current)
    {
        var c = gv(pos);
        var (up, dn, lt, rt, ul, ur, dl, dr) = getAllNeighbours(pos);
        //  OOO
        //  OXO   = 2 x+- y- olika mot c
        //  OXO
        if (gv(dn) == c && gv(up) != c && gv(lt) != c && gv(rt) != c)
        {
            count += 2;
            continue;
        }
        //  OOO
        //  XXO   = 2 x- och y+- olika mot c
        //  OOO
        if (gv(lt) == c && gv(up) != c && gv(up) != c && gv(rt) != c)
        {
            count += 2;
            continue;
        }
        //  OXO
        //  OXO   = 2 x+- och y+ olika mot c
        //  OOO
        if (gv(up) == c && gv(dn) != c && gv(lt) != c && gv(rt) != c)
        {
            count += 2;
            continue;
        }
        //  OOO
        //  OXX   = 2 x- och y+- olika mot c
        //  OOO
        if (gv(rt) == c && gv(dn) != c && gv(lt) != c && gv(up) != c)
        {
            count += 2;
            continue;
        }
        //  OXO
        //  OXO   = 0 y+- samma som c, x+- inte samma som c
        //  OXO
        if (gv(up) == c && gv(dn) == c && gv(lt) != c && gv(rt) != c)
            continue;
        //  OOO
        //  XXX   = 0 x+- samma som c, y+- inte samma
        //  OOO
        if (gv(lt) == c && gv(rt) == c && gv(up) != c && gv(dn) != c)
            continue;
        //  OOO
        //  XXO   = 2 x- och y+ samma som c, x+ och y- inte samma, x-y+ inte samma (om samma = 1 )
        //  ¤XO
        if (gv(lt) == c && gv(dn) == c && gv(up) != c && gv(rt) != c)
        {
            count++;
            if (gv(dl) != c)
                count++;
            continue;
        }
        //  ¤XO
        //  XXO   = 2 x- och y- samma som c, x+ och y+ inte samma, x-y- inte samma (om samma = 1 )
        //  OOO
        if (gv(lt) == c && gv(up) == c && gv(dn) != c && gv(rt) != c)
        {
            count++;
            if (gv(ul) != c)
                count++;
            continue;
        }
        //  OOO
        //  OXX   = 2 x+ och y+ samma som c, x- och y- inte samma, x+y+ inte samma (om samma = 1 )
        //  OX¤
        if (gv(lt) == c && gv(rt) == c && gv(up) != c && gv(up) != c)
        {
            count++;
            if (gv(dr) != c)
                count++;
            continue;
        }
        //  OX¤
        //  OXX   = 2 x+ och y- samma som c, x- och y+ inte samma, x+y- inte samma (om samma = 1 )
        //  OOO
        if (gv(rt) == c && gv(up) == c && gv(lt) != c && gv(dn) != c)
        {
            count++;
            if (gv(ur) != c)
                count++;
            continue;
        }
        //  OX¤
        //  OXX   = 2 x+ y+- samma som c, x- inte samma, x+y- och x+y+ inte samma (en mindre per icke samma)
        //  OX¤
        if (gv(rt) == c && gv(up) == c && gv(dn) == c && gv(rt) != c)
        {
            if (gv(ur) != c)
                count++;
            if (gv(dr) != c)
                count++;
            continue;
        }

        //  ¤XO
        //  XXO   = 2 x- y+- samma som c, x- inte samma, x-y- och x-y+ inte samma (en mindre per icke samma)
        //  ¤XO
        if (gv(lt) == c && gv(up) == c && gv(dn) == c && gv(rt) != c)
        {
            if (gv(ul) != c)
                count++;
            if (gv(dl) != c)
                count++;
            continue;
        }
        //  OOO
        //  XXX   = 2 y+ x+- samma som c, y- inte samma, x+y+ och x-y+ inte samma (en mindre per icke samma)
        //  ¤X¤
        if (gv(lt) == c && gv(rt) == c && gv(dn) == c && gv(up) != c)
        {
            if (gv(dr) != c)
                count++;
            if (gv(dl) != c)
                count++;
            continue;
        }
        //  ¤X¤
        //  XXX   = 2 y- x+- samma som c, y+ inte samma, x-y- och x-y+ inte samma (en mindre per icke samma)
        //  OOO
        if (gv(lt) == c && gv(up) == c && gv(rt) == c && gv(dn) != c)
        {
            if (gv(ur) != c)
                count++;
            if (gv(ul) != c)
                count++;
            continue;
        }
        //  %X%
        //  XXX   = 4 x+ och y+ och x- och y- samma som c, x+y+ x-y+ x+y- x-y- inte samma (en mindre per icke samma)
        //  %X%
        if (gv(lt) == c && gv(rt) == c && gv(up) == c && gv(dn) == c)
        {
            if (gv(dr) != c)
                count++;
            if (gv(ur) != c)
                count++;
            if (gv(dl) != c)
                count++;
            if (gv(ul) != c)
                count++;
        }
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