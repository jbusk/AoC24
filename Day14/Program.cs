using System.Text.RegularExpressions;
var lines = File.ReadAllLines("input.txt");
(int x, int y) tsize = (101, 103);
Dictionary<(int x, int y), List<(int x, int y)>> grid = [];
(int sumpart1, int sumpart2) = (0, 100);
var regex = new Regex(@"(-?\d+)");
foreach (var line in lines) 
{
    var m = regex.Matches(line);
    placeRobot((int.Parse(m[0].Value), int.Parse(m[1].Value)), (int.Parse(m[2].Value), int.Parse(m[3].Value)), grid);
}
// part 1
for (int i = 0; i < 100; i++)
    grid = moveRobots(grid);

var q1 = grid.Where(r => r.Key.x < tsize.x / 2 && r.Key.y < tsize.y / 2);
var q2 = grid.Where(r => r.Key.x > tsize.x / 2 && r.Key.y < tsize.y / 2);
var q3 = grid.Where(r => r.Key.x < tsize.x / 2 && r.Key.y > tsize.y / 2);
var q4 = grid.Where(r => r.Key.x > tsize.x / 2 && r.Key.y > tsize.y / 2);
sumpart1 = q1.Sum(x => x.Value.Count) * q2.Sum(x => x.Value.Count) * q3.Sum(x => x.Value.Count) * q4.Sum(x => x.Value.Count);

//Part 2
while (grid.Any(g => g.Value.Count > 1))
{
    grid = moveRobots(grid);
    sumpart2++;
}
visualise(grid);
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);

Dictionary<(int x, int y), List<(int x, int y)>> moveRobots(Dictionary<(int x, int y), List<(int x, int y)>> grid)
{
    Dictionary<(int x, int y), List<(int x, int y)>> ret = [];
    foreach (var pos in grid)
        foreach (var robot in pos.Value)
            placeRobot(((pos.Key.x + robot.x + tsize.x) % tsize.x, (pos.Key.y + robot.y + tsize.y) % tsize.y), robot, ret);
    return ret;
}

void visualise(Dictionary<(int x, int y), List<(int x, int y)>> grid)
{
    for (int y = 0; y < tsize.y; y++)
    {
        for (int x = 0; x < tsize.x; x++)
            if (grid.TryGetValue((x, y), out var robots))
                Console.Write(robots.Count);
            else
                Console.Write(".");
        Console.WriteLine();
    }
}

void placeRobot((int x, int y) pos, (int x, int y) val, Dictionary<(int x, int y), List<(int x, int y)>> grid)
{
    if (grid.TryGetValue(pos, out var gridValue))
        gridValue.Add(val);
    else
        grid[pos] = [val];
}
