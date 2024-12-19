(int sumpart1, long sumpart2) = (0, 0);
var lines = File.ReadAllLines("input.txt");
var patterns = lines[0].Split(", ").ToHashSet();
Dictionary<string, long> cache = [];
int maxlength = patterns.MaxBy(x => x.Length)!.Length;
for (int i = 2; i < lines.Length; i++)
{
    var line = lines[i];
    long arr = arrangements(line);
    if (arr > 0)
    {
        sumpart1++;
        sumpart2 += arr;
    }
}

Console.WriteLine(sumpart1);
Console.WriteLine(sumpart2);

long arrangements(string design)
{
    if (string.IsNullOrWhiteSpace(design)) return 1;
    if (cache.TryGetValue(design, out var result)) return result;
    long count = 0;
    for (int i = 0; i <= Math.Min(design.Length, maxlength + 1); i++)
    {
        var first = design[0..i];
        var last = design[i..];
        if (patterns.Contains(first))
            count += arrangements(last);
    }
    cache[design] = count;
    return count;
}