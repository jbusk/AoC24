using System.Diagnostics;
using Position = (int x, int y);
Stopwatch sw = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
(int sumpart1, int sumpart2, int max) = (0, 0, lines.Length); 
Dictionary<Position, int> grid = [];
List<Position> heads = [];
for (int x = 0; x < max; x++)
    for (int y = 0; y < max; y++)
    {
        var val = lines[x][y] & 15;
        grid[(x, y)] = val;
        if (val == 0)
            heads.Add((x, y));
    }

foreach (var head in heads)
{
    HashSet<Position> found = [];
    sumpart2 += ascend(grid, head, 0, max, found);
    sumpart1 += found.Count;
}

Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine("Time: " + sw.Elapsed);

IEnumerable<(int x, int y)> neighbours(Position pos, int max)
{
    Position[] rel_pos = [(0, 1), (1, 0), (0, -1), (-1, 0)];
    foreach (var rel in rel_pos)
    {
        var new_pos = (pos.x + rel.x, pos.y + rel.y);
        if (inRange(new_pos, max))
            yield return new_pos;
    }
    yield break;
}

bool inRange(Position pos, int max) => (pos.x < max && pos.x >= 0 && pos.y < max && pos.y >= 0);

int ascend(Dictionary<Position, int> grid, Position pos, int sought, int max, HashSet<Position> found)
{
    var curr = grid[pos];
    if (curr == sought)
    {
        if (sought == 9)
        {
            found.Add(pos);
            return 1;
        }
        int retval = 0;
        foreach (var neighbour in neighbours(pos, max))
            retval += ascend(grid, neighbour, sought + 1, max, found);
        return retval;
    }
    else
        return 0;
}