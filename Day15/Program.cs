using System.Text;

var file = File.ReadAllText("input.txt").Split("\r\n\r\n");
var textgrid = file[0].Split("\r\n");
var moves = file[1].Replace("\r\n", "");
(int x, int y) robot1 = (0, 0);
(int x, int y) robot2 = (0, 0);
Dictionary<(int x, int y), char> grid1 = [];
Dictionary<(int x, int y), char> grid2 = [];
int gy = 0;
foreach (var line in textgrid)
{
    for (var gx = 0; gx < line.Length; gx++)
    {
        if (line[gx] == '@')
            robot1 = (gx, gy);
        grid1[(gx, gy)] = line[gx];
    }
    StringBuilder sb = new StringBuilder(line);
    sb.Replace("#", "##");
    sb.Replace("O", "[]");
    sb.Replace(".", "..");
    sb.Replace("@", "@.");
    string newline = sb.ToString();
    for (int gx = 0; gx < newline.Length; gx++)
    {
        if (newline[gx] == '@')
            robot2 = (gx, gy);
        grid2[(gx, gy)] = newline[gx];
    }
    gy++;
}

// visualise();

foreach (var move in moves)
{
    (int dx, int dy) = move switch
    {
        '^' => (0, -1),
        '>' => (1, 0),
        'v' => (0, 1),
        _ => (-1, 0) // '<'
    };
    List<(char, (int x, int y))> positions = [('.', robot1)];
    var curr = robot1;
    bool canMove = true;
    while (true)
    {
        var currval = grid1[curr];
        (int x, int y) next = (curr.x + dx, curr.y + dy);
        var nextval = grid1[next];
        if (nextval == '#')
        {
            canMove = false;
            break;
        }
        else if (nextval == 'O')
        {
            positions.Add((currval, next));
        }
        else // '.'
        {
            positions.Add((currval, next));
            break;
        }
        curr = next;
    }
    if (!canMove)
        continue;

    foreach (var pair in positions)
    {
        grid1[pair.Item2] = pair.Item1;
    }
    grid1[robot1] = '.';
    robot1 = (robot1.x + dx, robot1.y + dy);

    //Console.WriteLine(move);
    // visualise();
}

Console.WriteLine("Part 1: " + scorePositions());

int scorePositions()
{
    int score = 0;
    foreach (var item in grid1.Where(x => x.Value == 'O'))
    {
        score += 100 * item.Key.y + item.Key.x;
    }
    return score;
}

void visualise()
{
    for (int y = 0; y < textgrid.Length; y++)
    {
        for (int x = 0; x < textgrid[0].Length; x++)
        {
            Console.Write(grid1[(x, y)]);
        }
        Console.WriteLine();
    }
}