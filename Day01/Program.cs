var lines = File.ReadAllLines("input.txt").Select(x => x.Split("   ")).Select(x => (int.Parse(x[0]), int.Parse(x[1])));
var group = lines.Select(x => x.Item2).GroupBy(i => i)
    .Select(g => new KeyValuePair<int, int>(g.Key, g.Count())).ToDictionary();
var sumpart1 = lines.Select(x => x.Item1).Order().Zip(lines.Select(x => x.Item2).Order()).Select(a => Math.Abs(a.First - a.Second)).Sum();
var sumpart2 = lines.Select(x => { group.TryGetValue(x.Item1, out int mult);  return x.Item1 * mult; }).Sum();
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
