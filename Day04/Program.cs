var lines = File.ReadAllLines("input.txt");
(int sumpart1, int sumpart2) = (0, 0);
int i = 0;
Dictionary<(int x, int y), char> vals = new();
foreach (var line in lines)
{
    for (int j = 0; j < line.Length; j++)
    {
        vals.Add((i, j), line[j]);
    }
    i++;
}

foreach (var pos in vals.Where(v => v.Value == 'X'))
{
    if (Left(pos.Key, vals))
        sumpart1++;
    if (Right(pos.Key, vals))
        sumpart1++;
    if (Up(pos.Key, vals))
        sumpart1++;
    if (Down(pos.Key, vals))
        sumpart1++;
    if (UpLeft(pos.Key, vals))
        sumpart1++;
    if (UpRight(pos.Key, vals))
        sumpart1++;
    if (DownLeft(pos.Key, vals))
        sumpart1++;
    if (DownRight(pos.Key, vals))
        sumpart1++;
}

foreach (var pos in vals.Where(v => v.Value == 'A'))
{
    if (Grave(pos.Key, vals) && Acute(pos.Key, vals))
        sumpart2++;
}

Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);

bool Grave((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // M   
    //  \   
    //   S
    if (vals.TryGetValue((x - 1, y - 1), out char ul) && ul == 'M')
        if (vals.TryGetValue((x + 1, y + 1), out char dr) && dr == 'S')
            return true;
    // S   
    //  \   
    //   M
    if (vals.TryGetValue((x - 1, y - 1), out ul) && ul == 'S')
        if (vals.TryGetValue((x + 1, y + 1), out char dr) && dr == 'M')
            return true;
    return false;
}

bool Acute((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    //   M
    //  /
    // S
    if (vals.TryGetValue((x - 1, y + 1), out char ul) && ul == 'M')
        if (vals.TryGetValue((x + 1, y - 1), out char dr) && dr == 'S')
            return true;
    //   S
    //  /
    // M
    if (vals.TryGetValue((x - 1, y + 1), out ul) && ul == 'S')
        if (vals.TryGetValue((x + 1, y - 1), out char dr) && dr == 'M')
            return true;
    return false;
}
bool Left((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // Check for M
    if (vals.TryGetValue((x - 1, y), out char m) && m == 'M')
        // Check for A
        if (vals.TryGetValue((x - 2, y), out char a) && a == 'A')
            // Check for S
            return (vals.TryGetValue((x - 3, y), out char s) && s == 'S');
    return false;
}

bool Right((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // Check for M
    if (vals.TryGetValue((x + 1, y), out char m) && m == 'M')
        // Check for A
        if (vals.TryGetValue((x + 2, y), out char a) && a == 'A')
            // Check for S
            return (vals.TryGetValue((x + 3, y), out char s) && s == 'S');
    return false;
}

bool Up((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // Check for M
    if (vals.TryGetValue((x, y - 1), out char m) && m == 'M')
        // Check for A
        if (vals.TryGetValue((x, y - 2), out char a) && a == 'A')
            // Check for S
            return (vals.TryGetValue((x, y - 3), out char s) && s == 'S');
    return false;
}

bool Down((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // Check for M
    if (vals.TryGetValue((x, y + 1), out char m) && m == 'M')
        // Check for A
        if (vals.TryGetValue((x, y + 2), out char a) && a == 'A')
            // Check for S
            return (vals.TryGetValue((x, y + 3), out char s) && s == 'S');
    return false;
}

bool UpLeft((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // Check for M
    if (vals.TryGetValue((x - 1, y - 1), out char m) && m == 'M')
        // Check for A
        if (vals.TryGetValue((x - 2, y - 2), out char a) && a == 'A')
            // Check for S
            return (vals.TryGetValue((x - 3, y - 3), out char s) && s == 'S');
    return false;
}

bool DownLeft((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // Check for M
    if (vals.TryGetValue((x + 1, y + 1), out char m) && m == 'M')
        // Check for A
        if (vals.TryGetValue((x + 2, y + 2), out char a) && a == 'A')
            // Check for S
            return (vals.TryGetValue((x + 3, y + 3), out char s) && s == 'S');
    return false;
}

bool UpRight((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // Check for M
    if (vals.TryGetValue((x - 1, y + 1), out char m) && m == 'M')
        // Check for A
        if (vals.TryGetValue((x - 2, y + 2), out char a) && a == 'A')
            // Check for S
            return (vals.TryGetValue((x - 3, y + 3), out char s) && s == 'S');
    return false;
}

bool DownRight((int x, int y) key, Dictionary<(int x, int y), char> vals)
{
    (int x, int y) = key;
    // Check for M
    if (vals.TryGetValue((x + 1, y - 1), out char m) && m == 'M')
        // Check for A
        if (vals.TryGetValue((x + 2, y - 2), out char a) && a == 'A')
            // Check for S
            return (vals.TryGetValue((x + 3, y - 3), out char s) && s == 'S');
    return false;
}
