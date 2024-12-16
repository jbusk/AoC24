using Position = (int x, int y);
var lines = File.ReadAllLines("sample.txt").ToArray();
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
PriorityQueue<(Position, char direction, List<Position> back, int cost), int> queue = new();
queue.Enqueue((start, '>', [], 0), 0);
HashSet<(char direction, Position)> visited = [];
List<int> costs = [];
List<(int cost, List<Position>)> backlist = [];
while (queue.Count > 0)
{
    var (position, direction, back, cost) = queue.Dequeue();
    if (visited.Contains((direction, position)))
        continue;
    visited.Add((direction, position));

    if (position == end)
    {
        costs.Add(cost);
        backlist.Add((cost, back));
    }
    foreach (var n in getNeighbours(position, direction))
    {
        if (grid[n.p] == '#')
            continue;
        if (visited.Contains((n.d, n.p)))
            continue;
        var nback = back.ToList();
        nback.Add(position);
        queue.Enqueue((n.p, n.d, nback, n.cost + cost), n.cost + cost);
    }
}

HashSet<Position> track = [];
foreach (var list in backlist.Where(x => x.cost == costs.Min()))
{
    track.UnionWith(list.Item2);
}
Console.WriteLine(track.Count);
Console.WriteLine("Part 1: (11048 expected sample 2): " + costs.Min());

static (char d, Position p, int cost)[] getNeighbours(Position pos, char dir)
{
    return dir switch
    {
        '^' => [('^', (pos.x, pos.y - 1), 1), ('<', (pos.x - 1, pos.y), 1001), ('>', (pos.x + 1, pos.y), 1001)], // North West East
        '>' => [('>', (pos.x + 1, pos.y), 1), ('^', (pos.x, pos.y - 1), 1001), ('v', (pos.x, pos.y + 1), 1001)], // East North South
        'v' => [('v', (pos.x, pos.y + 1), 1), ('>', (pos.x + 1, pos.y), 1001), ('<', (pos.x - 1, pos.y), 1001)], // South East West
        _ => [('<', (pos.x - 1, pos.y), 1), ('v', (pos.x, pos.y + 1), 1001), ('^', (pos.x, pos.y - 1), 1001)] // West South North
    };
}

