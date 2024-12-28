var lines = File.ReadAllLines("input.txt").Select(x => x.Split("-"));
(long sumpart1, long sumpart2) = (0, 0);
Dictionary<string, HashSet<string>> connections = [];
foreach (var line in lines)
{
    connect_computers(line[0], line[1]);
}

HashSet<(string, string, string)> seen = [];
foreach (var computer in connections)
{
    var x = computer.Key;
    foreach (var y in computer.Value)
    {
        foreach (var z in connections[y])
        {
            if (x != z && connections[z].Contains(x))
            {
                string[] unordered = [x, y, z];
                var ordered = unordered.Order().ToArray();
                seen.Add((ordered[0], ordered[1], ordered[2]));
            }
        }
    }
}

Console.WriteLine("Part 1: " + seen.Count(x => x.Item1.StartsWith('t') || x.Item2.StartsWith('t') || x.Item3.StartsWith('t')));

HashSet<string> clusters = [];
foreach (var node in connections)
{
    search(node.Key, [node.Key]);
}
Console.WriteLine("Part 2: " + clusters.MaxBy(x => x.Length));


void search(string node, HashSet<string> required)
{
    var key = string.Join(",", required.Order());
    if (clusters.Contains(key))
        return;
    clusters.Add(key);
    foreach (var neighbour in connections[node])
    {
        if (required.Contains(neighbour))
            continue;
        if (!required.IsSubsetOf(connections[neighbour]))
            continue;
        var newreq = required.ToHashSet();
        newreq.Add(neighbour);
        search(neighbour, newreq);
    }
}

void connect_computers(string x, string y)
{
    if (connections.TryGetValue(x, out var list))
        list.Add(y);
    else
        connections[x] = [y];
    if (connections.TryGetValue(y, out list))
        list.Add(x);
    else
        connections[y] = [x];
}
