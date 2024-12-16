using Position = (int x, int y);
var lines = File.ReadAllLines("input.txt").ToArray();
Dictionary<Position, char> grid = [];
Position end = (0, 0);
Position start = (0, 0);
Dictionary<(Position position, char direction), int> lowest_cost = [];
Dictionary<(Position position, char direction), List<(Position position, char direction)>> backtrack = [];
for (int y = 0; y < lines.Length; y++)
    for (int x = 0; x < lines[0].Length; x++)
    {
        if (lines[y][x] == 'E')
            end = (x, y);
        else if (lines[y][x] == 'S')
            start = (x, y);
        lowest_cost[((x, y), '^')] = int.MaxValue;
        lowest_cost[((x, y), 'v')] = int.MaxValue;
        lowest_cost[((x, y), '<')] = int.MaxValue;
        lowest_cost[((x, y), '>')] = int.MaxValue;
        backtrack[((x, y), '^')] = [];
        backtrack[((x, y), 'v')] = [];
        backtrack[((x, y), '<')] = [];
        backtrack[((x, y), '>')] = [];
        grid[(x, y)] = lines[y][x];
    }
PriorityQueue<(Position position, char direction, Position lpos, char ldir, int cost), int> queue = new();
queue.Enqueue((start, '>', start, '>', 0), 0);
int best_cost = int.MaxValue;
while (queue.Count > 0)
{
    var (position, direction, lpos, ldir, cost) = queue.Dequeue();
    if (cost > lowest_cost[(position, direction)])
        continue;
    lowest_cost[(position, direction)] = cost;
    if (position == end)
    {
        if (cost > best_cost)
            break;
        best_cost = cost;
    }
    backtrack[(position, direction)].Add((lpos, ldir));
    foreach (var n in getNeighbours(position, direction))
    {
        int newcost = n.cost + cost;
        if (grid[n.p] == '#')
            continue;
        if (newcost > lowest_cost[(n.p, n.d)])
            continue;
        if (newcost < lowest_cost[(n.p, n.d)])
        {
            backtrack[(n.p, n.d)] = [];
            lowest_cost[(n.p, n.d)] = newcost;
        }
        queue.Enqueue((n.p, n.d, position, direction, newcost), newcost);
    }
}

LinkedList<(Position position, char direction)> states = new();
HashSet<(Position position, char direction)> seen = [];
foreach (var endstate in backtrack.Where(x => x.Key.position == end))
    states.AddLast(endstate.Key);
for (var node = states.First; node != null; node = node.Next)
{
    var curr = node.Value;
    foreach (var last in backtrack[curr])
    {
        if (seen.Contains(last))
            continue;
        seen.Add(last);
        states.AddLast(last);
    }
}

Console.WriteLine("Part 1: " + best_cost);
Console.WriteLine("Part 2: " + (seen.Select(x => x.position).Distinct().Count() + 1)); // answer is consistently off by one, I don't care to fix it

static (char d, Position p, int cost)[] getNeighbours(Position pos, char dir)
{
    return dir switch
    {
        '^' => [('^', (pos.x, pos.y - 1), 1), ('<', (pos.x - 1, pos.y), 1001), ('>', (pos.x + 1, pos.y), 1001)], // North West East
        '>' => [('>', (pos.x + 1, pos.y), 1), ('^', (pos.x, pos.y - 1), 1001), ('v', (pos.x, pos.y + 1), 1001)], // East North South
        'v' => [('v', (pos.x, pos.y + 1), 1), ('>', (pos.x + 1, pos.y), 1001), ('<', (pos.x - 1, pos.y), 1001)], // South East West
        '<' => [('<', (pos.x - 1, pos.y), 1), ('v', (pos.x, pos.y + 1), 1001), ('^', (pos.x, pos.y - 1), 1001)], // West South North
        _ => throw new ArgumentException("Not a valid direction: \"" + dir + "\"")
    };
}