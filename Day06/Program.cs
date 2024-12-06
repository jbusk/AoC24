using System.Collections.Concurrent;
using System.Diagnostics;
var lines = File.ReadAllLines("input.txt");
Dictionary<(int x, int y), char> grid = [];
for (int x = 0; x < lines.Length; x++)
{
    for (int y = 0; y < lines[x].Length; y++)
    {
        grid[(x,y)] = lines[x][y];
    }
}
char[] guard = ['^', '<', '>', 'v'];
var origin = grid.Where(x => guard.Contains(x.Value)).FirstOrDefault();
(int x, int y)[] neighbours = [(-1, -1), (-1, 1), (1, 1), (1, -1)];
Stopwatch sw = Stopwatch.StartNew();
//part 1
while (true)
{
    var curr = grid.Where(x => guard.Contains(x.Value)).FirstOrDefault();
    if (curr.Value == '\0')
        break;
    var cpos = curr.Key;
    var cval = curr.Value;
    (int x, int y) npos = curr.Value switch
    {
        '^' => ((curr.Key.x - 1), (curr.Key.y)),
        'v' => ((curr.Key.x + 1), (curr.Key.y)),
        '<' => ((curr.Key.x), (curr.Key.y - 1)),
        _ => ((curr.Key.x), (curr.Key.y + 1)), // '>'        
    };
    if (grid.TryGetValue(npos, out char nval))
    {
        if (nval == '#')
        {
            grid[cpos] = curr.Value switch
            {
                '^' => '>',
                '>' => 'v',
                'v' => '<',
                _ => '^'
            };
        }
        else
        {
            foreach (var neighbour in neighbours)
            {
                var neighpos = (cpos.x + neighbour.x, cpos.y + neighbour.y);
                if (grid.TryGetValue(neighpos, out var neighval))
                {
                    if (neighval == '.')
                        grid[neighpos] = 'O';
                }
            }
            grid[npos] = cval;
            grid[cpos] = 'X';
        }
    }
    else
    {
        grid[cpos] = 'X';
    }
}
Console.WriteLine("Part 1: " + grid.Where(x => x.Value == 'X').Sum(x => 1));
Console.WriteLine("Part 1 after " + sw.Elapsed);
// part 2
grid[origin.Key] = origin.Value;
var possibles = grid.Where(x => x.Value != '.');
ConcurrentBag<int> part2bag = new();
Parallel.ForEach(possibles, pos =>
{
    var gridcopy = grid.ToDictionary(entry => entry.Key, entry => entry.Value);
    if (!(pos.Key == origin.Key) && pos.Value != '#')
    {
        Dictionary<(int x, int y), int> stepgrid = [];
        HashSet<((int x, int y), char)> steps = [];
        gridcopy[pos.Key] = '#';
        while (true)
        {
            var curr = gridcopy.Where(x => guard.Contains(x.Value)).FirstOrDefault();
            if (curr.Value == '\0')
                break;
            (var currpos, var currval) = (curr.Key, curr.Value);
            (int x, int y) nextpos = curr.Value switch
            {
                '^' => ((curr.Key.x - 1), (curr.Key.y)),
                'v' => ((curr.Key.x + 1), (curr.Key.y)),
                '<' => ((curr.Key.x), (curr.Key.y - 1)),
                _ => ((curr.Key.x), (curr.Key.y + 1)), // '>'        
            };
            if (gridcopy.TryGetValue(nextpos, out char nextval))
            {
                if (nextval == '#')
                {
                    gridcopy[currpos] = curr.Value switch
                    {
                        '^' => '>',
                        '>' => 'v',
                        'v' => '<',
                        _ => '^' // '<'
                    };
                }
                else
                {
                    if (steps.Contains((currpos, currval)))
                    {
                        part2bag.Add(1);
                        break;
                    }
                    else
                        steps.Add((currpos, currval));
                    gridcopy[nextpos] = currval;
                    gridcopy[currpos] = '.';
                }
            }
            else
            {
                break;
            }
        }
    }
});

Console.WriteLine("Part 2: " + part2bag.Sum());
sw.Stop();
Console.WriteLine("Total time: " + sw.Elapsed);
