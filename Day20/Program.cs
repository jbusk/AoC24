using Position = (int x, int y);
var lines = File.ReadAllLines("input.txt");
(int sumpart1, int sumpart2, int max) = (0, 0, lines.Length);

HashSet<Position> grid = [];
(Position start, Position end) = ((0, 0), (0, 0));
for (int y = 0; y < max; y++)
    for (int x = 0; x < max; x++)
    {
        if (lines[y][x] == '#')
            grid.Add((x, y));
        else if (lines[y][x] == 'S')
            start = ((x, y));
        else if (lines[y][x] == 'E')
            end = ((x, y));

    }

// 1. run the course to get the initial distance, save positions of all touched walls
int initial_distance = navigate(grid, start, end);
Console.WriteLine("Initial distance: " + initial_distance);
int aim_for = initial_distance - 100;
// 2. for each touched wall, remove it and run the course again, seeing how much time was saved
foreach (Position position in grid)
{
    var ngrid = grid.ToHashSet();
    ngrid.Remove(position);
    var ndist = navigate(ngrid, start, end);
    if (ndist <= aim_for)
        sumpart1++;
}
Console.WriteLine("Part 1: " + sumpart1);

int navigate(HashSet<Position> grid, Position start, Position end)
{
    LinkedList<(Position position, int cost)> list = [];
    HashSet<Position> seen = [];
    list.AddLast((start, 0));
    for (var node = list.First; node != null; node = node.Next)
    {
        foreach (var n in neighbours(node.Value.position))
        {
            if (seen.Contains(n))
                continue;
            if (grid.Contains(n))
                continue;
            if (n == end)
                return node.Value.cost + 1;
            seen.Add(n);
            list.AddLast((n, node.Value.cost + 1));
        }
    }
    return int.MaxValue;
}

IEnumerable<Position> neighbours(Position pos)
{
    Position[] rel_pos = [(0, 1), (1, 0), (0, -1), (-1, 0)];
    foreach (var (x, y) in rel_pos)
    {
        var new_pos = (pos.x + x, pos.y + y);
        if (inRange(new_pos))
            yield return new_pos;
    }
    yield break;
}

bool inRange(Position pos) => (pos.x < max && pos.x >= 0 && pos.y < max && pos.y >= 0);
