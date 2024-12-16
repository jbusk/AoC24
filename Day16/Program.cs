using Position = (int x, int y);
var lines = File.ReadAllLines("sample.txt").ToArray();
Dictionary<Position, Node> grid = [];
Position end = (0, 0);
Position start = (0, 0);
for (int y = 0; y < lines.Length; y++)
    for (int x = 0; x < lines[0].Length; x++)
    {
        if (lines[y][x] == 'E')
            end = (x, y);
        else if (lines[y][x] == 'S')
            start = (x, y);
        grid[(x, y)] = new Node(lines[y][x]);
    }

// start at E in the grid
Console.WriteLine("Part 1: " + findPath(start, end));

int findPath(Position start, Position end)
{
    Direction dir = Direction.E;

    return 0;
}

IEnumerable<Position> getNeighbours(Position pos)
{
    Position[] relpositions = [(0, 1), (0, -1), (1, 0), (-1, 0)];
    foreach (var rel in relpositions)
    {
        yield return (pos.x + rel.x, pos.y + rel.y);
    }
    yield break;
}

enum Direction
{
    N,
    E,
    S,
    W
}

class Node
{

    public Node(char c)
    {
        if (c == '.')
        {
            Visited = false;
            Path = true;
            Start = false;
            End = false;
        }
        else if (c == 'S')
        {
            Path = true;
            Visited = false;
            Start = true;
            End = false;
        }
        else if (c == 'E')
        {
            Path = true;
            Visited = false;
            Start = false;
            End = true;
        }
        else
        {
            Visited = true;
            Path = false;
            Start = false;
            End = false;
        }
    }
    public bool Start { get; set; }
    public bool End { get; set; }
    public bool Visited { get; set; }
    public bool Path { get; set; }
    public int CostN { get; set; }
    public int CostE { get; set; }
    public int CostS { get; set; }
    public int CostW { get; set; }
}