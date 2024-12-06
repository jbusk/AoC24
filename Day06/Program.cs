var lines = File.ReadAllLines("sample.txt");
(int sumpart1, int sumpart2) = (0, 0);
//Dictionary<(int x, int y), char> grid = [];
HashSet<((int x, int y), char)> grid = [];

int x = 0;
foreach (var line in lines)
{
    int y = 0;
    foreach (var ch in line)
    {
        grid.Add(((x, y), ch));
        y++;
    }
    x++;
}
var foo = grid.Where(x => x.Item1 == (1000, 1000)).FirstOrDefault().Item2;
Console.WriteLine( "["+foo +"]" + (foo == '\0'));
char[] guard = ['^', '<', '>', 'v'];

while (true)
{
    var curr = grid.Where(x => guard.Contains(x.Item2)).FirstOrDefault();
    if (curr.Item2 == '\0')
        break;
    (int x, int y) next = curr.Item2 switch
    {
        '^' => ((curr.Item1.x - 1), (curr.Item1.y)),
        'v' => ((curr.Item1.x + 1), (curr.Item1.y)),
        '<' => ((curr.Item1.x), (curr.Item1.y - 1)),
        _ => ((curr.Item1.x), (curr.Item1.y + 1)), // '>'        
    };
    //Console.WriteLine($"Guard is at {curr.Item1.x},{curr.Item1.y} and is pointed {curr.Item2} next is {next.x},{next.y}");
    var nextpos = grid.Where(x => x.Item1 == next).FirstOrDefault();
    if (nextpos.Item2 == '#')
        curr.Item2 = curr.Item2 switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            _ => '^'
        };
    else if (nextpos.Item2 == '\0')
        curr.Item2 = 'X';
    else
    {
        nextpos.Item2 = curr.Item2;
        curr.Item2 = 'X';
    }
}
sumpart1 = grid.Where(x => x.Item2 == 'X').Sum(x => 1);
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
