using System.Text;
var file = File.ReadAllText("input.txt").Split("\r\n\r\n");
var textgrid = file[0].Split("\r\n");
var moves = file[1].Replace("\r\n", "");
(int x, int y) robot1 = (0, 0);
(int x, int y) robot2 = (0, 0);
Dictionary<(int x, int y), char> grid1 = [];
Dictionary<(int x, int y), char> grid2 = [];
string[][] replacements = [["#", "##"], ["O", "[]"], [".", ".."], ["@", "@."]];
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
    foreach (var item in replacements)
        sb.Replace(item[0], item[1]);
    string line2 = sb.ToString();
    for (int gx = 0; gx < line2.Length; gx++)
    {
        if (line2[gx] == '@')
            robot2 = (gx, gy);
        grid2[(gx, gy)] = line2[gx];
    }
    gy++;
}

moveRobot1();
Console.WriteLine("Part 1: " + score(grid1));
moveRobot2();
Console.WriteLine("Part 2: " + score(grid2));

void moveRobot1()
{
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
        if (canMove)
        {
            foreach (var pair in positions)
            {
                grid1[pair.Item2] = pair.Item1;
            }
            grid1[robot1] = '.';
            robot1 = (robot1.x + dx, robot1.y + dy);
        }
    }
}

void moveRobot2()
{
    foreach (var move in moves)
    {
        (int dx, int dy) = move switch
        {
            '^' => (0, -1),
            '>' => (1, 0),
            'v' => (0, 1),
            _ => (-1, 0) // '<'
        };
        LinkedList<(int x, int y)> targets = [];
        targets.AddLast(robot2);
        bool canMove = true;

        for (var node = targets.First; node != null; node = node.Next)
        {
            var curr = node.Value;
            (int x, int y) next = (curr.x + dx, curr.y + dy);
            if (targets.Contains(next))
                continue;
            var nextval = grid2[next];
            if (nextval == '#')
            {
                canMove = false;
                break;
            }
            if (nextval == '[')
            {
                targets.AddLast(next);
                targets.AddLast((next.x + 1, next.y));
            }
            if (nextval == ']')
            {
                targets.AddLast(next);
                targets.AddLast((next.x - 1, next.y));
            }
        }
        if (canMove)
        {
            var copy = grid2.ToDictionary();
            grid2[robot2] = '.';
            grid2[(robot2.x + dx, robot2.y + dy)] = '@';
            foreach (var pos in targets.Skip(1))
            {
                grid2[pos] = '.';
            }
            foreach (var pos in targets.Skip(1))
            {
                var next = (pos.x + dx, pos.y + dy);
                grid2[next] = copy[pos];
            }
            robot2 = (robot2.x + dx, robot2.y + dy);
        }
    }
}

static int score(Dictionary<(int x, int y), char> grid)
{
    int score = 0;
    foreach (var item in grid.Where(x => x.Value == 'O' || x.Value == '['))
    {
        score += 100 * item.Key.y + item.Key.x;
    }
    return score;
}