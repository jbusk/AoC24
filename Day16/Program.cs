using Position = (int x, int y);
var lines = File.ReadAllLines("input.txt").ToArray();
Dictionary<Position, char> grid = [];
Position end = (0, 0);
Position start = (0, 0);
for (int y = 0; y < lines.Length; y++)
    for (int x = 0; x < lines[0].Length; x++)
    {
        if (lines[y][x] == 'E')
            end = (x, y);
        else if (lines[y][x] == 'S')
            start = (x, y);
        grid[(x, y)] = lines[y][x];
    }
PriorityQueue<(Position, char direction, int cost), int> queue = new();
queue.Enqueue((start, '>', 0), 0);
HashSet<(char direction, Position)> visited = [];
List<int> costs = [];
while (queue.Count > 0)
{
    var (position, direction, cost) = queue.Dequeue();
    if (visited.Contains((direction, position)))
        continue;
    visited.Add((direction, position));
    if (position == end)
    {
        costs.Add(cost);
    }
    foreach (var n in getNeighbours(position, direction))
    {
        if (grid[n.p] == '#')
            continue;
        if (visited.Contains((n.d, n.p)))
            continue;
        queue.Enqueue((n.p, n.d, n.cost + cost), n.cost + cost);
    }
}

Console.WriteLine("Part 1: " + costs.Min());

static (char d, Position p, int cost)[] getNeighbours(Position pos, char dir)
{
    return dir switch
    {
        '^' => [('^', (pos.x, pos.y - 1), 1), ('<', (pos.x - 1, pos.y), 1001), ('>', (pos.x + 1, pos.y), 1001)], // North West East
        '>' => [('>', (pos.x + 1, pos.y), 1), ('^', (pos.x, pos.y - 1), 1001), ('v', (pos.x, pos.y + 1), 1001)], // East North South
        'v' => [('v', (pos.x, pos.y + 1), 1), ('>', (pos.x + 1, pos.y), 1001), ('<', (pos.x - 1, pos.y), 1001)], // South East West
        _   => [('<', (pos.x - 1, pos.y), 1), ('v', (pos.x, pos.y + 1), 1001), ('^', (pos.x, pos.y - 1), 1001)] // West South North
    };
}

