using Day05;
using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
(int sumpart1, int sumpart2) = (0, 0);
bool process_rules = true;
Dictionary<int, List<int>> rules = [];
List<int[]> updates = [];
foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        process_rules = false;
        continue;
    }
    if (process_rules)
    {
        var l = line.Split("|").Select(int.Parse);
        (int before, int after) = (l.First(), l.Last());
        addRule(rules, before, after);
    }
    else
    {
        var pages = line.Split(",").Select(int.Parse).ToList();
        if (line_is_valid(rules, pages))
        {
            var half = pages.ElementAt(pages.Count / 2);
            sumpart1 += half;
        }
        else
        {
            var list = pages.Permutate().AsParallel().First(x => line_is_valid(rules, x)).ToList();
           // var list = pages.Permutate().First(x => line_is_valid(rules, x)).ToList();
            var half = list.ElementAt(list.Count / 2);
            sumpart2 += half;
        }
    }
}
sw.Stop();
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine(sw.Elapsed);
static void addRule(Dictionary<int, List<int>> rules, int before, int after)
{
    if (rules.TryGetValue(before, out var dval))
    {
        dval.Add(after);
        rules[before] = dval;
    }
    else
        rules[before] = [after];

}

static bool line_is_valid(Dictionary<int, List<int>> before_afters, IEnumerable<int> pages)
{
    List<int> previous = [];
    foreach (var page in pages)
    {
        before_afters.TryGetValue(page, out var befores);
        if (befores != null && previous.Any(befores.Contains))
            return false;
        previous.Add(page);
    }
    return true;
}


