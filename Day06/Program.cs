using System.Diagnostics;
var lines = File.ReadAllLines("input.txt");
Dictionary<(int x, int y), char> grid = [];
for (int x = 0; x < lines.Length; x++)
    for (int y = 0; y < lines[x].Length; y++)
        grid[(x, y)] = lines[x][y];

char[] guard = ['^', '<', '>', 'v'];
var origin = grid.Where(x => guard.Contains(x.Value)).FirstOrDefault();
Stopwatch sw = Stopwatch.StartNew();
//part 1
(var cpos, var cval) = (origin.Key, origin.Value);
GuardLoops(grid, cpos, cval);
Console.WriteLine("Part 1: " + grid.Where(x => x.Value == 'X').Sum(x => 1));
Console.WriteLine("Part 1 time: " + sw.Elapsed);
sw.Restart();

// part 2
grid[origin.Key] = origin.Value;
var possibles = grid.Where(x => x.Value != '.');
int sumpart2 = 0;
Parallel.ForEach(possibles, pos =>
{
    var gridcopy = grid.ToDictionary(entry => entry.Key, entry => entry.Value);
    gridcopy[pos.Key] = '#';
    (var cpos, var cval) = (origin.Key, origin.Value);
    if (GuardLoops(gridcopy, cpos, cval))
        Interlocked.Increment(ref sumpart2);
});
sw.Stop();
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine("Part 2 time: " + sw.Elapsed);

static bool GuardLoops(Dictionary<(int x, int y), char> grid, (int x, int y) cpos, char cval)
{
    HashSet<((int x, int y), char)> steps = [];
    while (true)
    {
        (int x, int y) npos = cval switch
        {
            '^' => ((cpos.x - 1), (cpos.y)),
            '>' => ((cpos.x), (cpos.y + 1)),
            'v' => ((cpos.x + 1), (cpos.y)),
            _ => ((cpos.x), (cpos.y - 1)), // '<'        
        };
        if (grid.TryGetValue(npos, out char nval))
        {
            if (nval == '#')
            {
                cval = cval switch
                {
                    '^' => '>',
                    '>' => 'v',
                    'v' => '<',
                    _ => '^' // '<'
                };
            }
            else
            {
                if (steps.Contains((cpos, cval)))
                    return true;
                else
                    steps.Add((cpos, cval));
                grid[cpos] = 'X';
                cpos = npos;
            }
        }
        else
        {
            grid[cpos] = 'X';
            return false;
        }
    }
}