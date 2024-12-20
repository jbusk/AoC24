using Position = (int x, int y);
var lines = File.ReadAllLines("input.txt");
(int sumpart1, int sumpart2, int max) = (0, 0, lines.Length);
HashSet<Position> obstacles = [];
(Position start, Position end) = ((0, 0), (0, 0));
for (int y = 0; y < max; y++)
    for (int x = 0; x < max; x++)
    {
        if (lines[y][x] == '#')
            obstacles.Add((x, y));
        else if (lines[y][x] == 'S')
            start = ((x, y));
        else if (lines[y][x] == 'E')
            end = ((x, y));
    }
int initial_distance = navigate(obstacles, start, end, out var walkable);
int aim_for = initial_distance - 100;
walkable[end] = initial_distance;

Parallel.ForEach(walkable, cheat_start =>
{
    foreach (var cheat_end in walkable.Where(c_end => manhattan(c_end.Key, cheat_start.Key) <= 20 && c_end.Value > cheat_start.Value))
    {
        int mdist = manhattan(cheat_end.Key, cheat_start.Key);
        int cost = (initial_distance - cheat_end.Value) + mdist + cheat_start.Value;
        if (cost <= aim_for)
        {
            if (mdist == 2)
                Interlocked.Increment(ref sumpart1);
            Interlocked.Increment(ref sumpart2);
        }
    }
});
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);

int navigate(HashSet<Position> obstacles, Position start, Position end, out Dictionary<Position, int> cost)
{
    cost = [];
    LinkedList<(Position position, int cost)> list = [];
    HashSet<Position> seen = [];
    list.AddLast((start, 0));
    for (var node = list.First; node != null; node = node.Next)
    {
        foreach (var n in neighbours(node.Value.position))
        {
            if (seen.Contains(n))
                continue;
            if (obstacles.Contains(n))
                continue;
            if (n == end)
                return node.Value.cost + 1;
            seen.Add(n);
            cost[n] = node.Value.cost + 1;
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

int manhattan(Position pos1, Position pos2) => Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y);