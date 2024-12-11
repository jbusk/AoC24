using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var line = File.ReadAllText("input.txt").Split(' ');
Dictionary<string, long> dict = [];
foreach (var item in line)
    dict[item] = 1;

dict = blink_times(dict, 25);
Console.WriteLine("Part 1: " + dict.Sum(x => x.Value));
Console.WriteLine(sw.Elapsed);
dict = blink_times(dict, 50);
Console.WriteLine("Part 2: " + dict.Sum(x => x.Value));
Console.WriteLine(sw.Elapsed);

static Dictionary<string, long> blink(Dictionary<string, long> dict)
{
    Dictionary<string, long> ret = [];
    foreach (var item in dict)
    {
        if (item.Key == "0")
        {
            add(ret, "1", item.Value);
        }
        else if (item.Key.Length % 2 == 0)
        {
            add(ret, long.Parse(item.Key[(item.Key.Length / 2)..]).ToString(), item.Value);
            add(ret, long.Parse(item.Key[..(item.Key.Length / 2)]).ToString(), item.Value);
        }
        else
        {
            add(ret, (long.Parse(item.Key) * 2024).ToString(), item.Value);
        }
    }
    return ret;
    static void add(Dictionary<string, long> d, string k, long v)
    {
        if (d.TryGetValue(k, out long value))
            d[k] = value + v;
        else
            d[k] = v;
    }
}

static Dictionary<string, long> blink_times(Dictionary<string, long> dict, int times)
{
    for (int i = 0; i < times; i++)
    {
        var output = blink(dict);
        dict = output;
    }
    return dict;
}
