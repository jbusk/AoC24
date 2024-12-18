using Position = (int x, int y);
int max = 70; //6; 
int stop_at = 1024; // 12
var lines = File.ReadAllLines("input.txt").Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
HashSet<Position> grid = [];
int count = 0;
foreach (var line in lines)
{
    if (count == stop_at)
        break;
    grid.Add((line[0], line[1]));
    count++;
}

Console.WriteLine("Part 1: " + navigate(grid));

string part2 = "";
for (int i = 1024; i < lines.Length; i++)
    grid.Add((lines[i][0], lines[i][1]));
foreach (var line in lines.Reverse())
{
    grid.Remove((line[0], line[1]));
    int ret = navigate(grid);
    if (ret != int.MaxValue)
    {
        part2 = $"{line[0]},{line[1]}";
        break;
    }
}
Console.WriteLine($"Part 2: " + part2);

int navigate(HashSet<Position> grid)
{
    Position start = (0, 0);
    Position end = (max, max);
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

bool inRange(Position pos) => (pos.x <= max && pos.x >= 0 && pos.y <= max && pos.y >= 0);
