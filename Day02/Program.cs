var lines = File.ReadAllLines("input.txt").Select(x => x.Split(" ").Select(int.Parse)).ToList();
int sumpart1 = 0;
int sumpart2 = 0;

foreach (var line in lines)
{
    if (is_safe(line))
    {
        sumpart1++;
        sumpart2++;
        continue;
    }
    for (int i = 0; i < line.Count(); i++)
    {
        var list = line.ToList();
        list.RemoveAt(i);
        if (is_safe(list))
        {
            sumpart2++;
            break;
        }
    }

}
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);

static bool is_safe(IEnumerable<int> line)
{
    var tuples = line.Zip(line.Skip(1), (a, b) => Tuple.Create(a, b));
    return tuples.All(pair => Math.Abs(pair.Item1 - pair.Item2) <= 3)
        && (tuples.All(pair => pair.Item1 < pair.Item2)
        || tuples.All(pair => pair.Item1 > pair.Item2)
        );
}
