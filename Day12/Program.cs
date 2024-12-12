using Position = (int x, int y);
(int sumpart1, int sumpart2) = (0, 0);
var lines = File.ReadAllLines("sample-1.txt");
HashSet<Position> visited = [];
List<HashSet<Position>> regions = [];
int max = lines.Length; // The grids axis are equal
for (int x = 0; x < max; x++)
    for (int y = 0; max > y; y++)
    {
        if (visited.Contains((x, y)))
            continue;
        HashSet<Position> region = [(x,y)];
        LinkedList<Position> queue = [];
        var currval = lines[x][y];
        queue.AddLast((x, y));
        for (var node = queue.First; node != null; node = node.Next)
        {
            var currpos = node.Value;
            foreach (var pos in getNeighbours((x,y)))
            {
                (int nx, int ny) = pos;
                if (!inRange(pos, max))
                    continue;
                if (lines[nx][ny] != currval)
                    continue;
                region.Add(pos);
                queue.AddLast(pos);
            }
            queue.Remove(node);
        }
        visited.UnionWith(region);
        regions.Add(region);
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