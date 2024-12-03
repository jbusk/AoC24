using System.Text.RegularExpressions;
var lines = File.ReadAllLines("input.txt");
(int sumpart1, int sumpart2) = (0, 0);
var pattern = @"(do\(\))|(mul\(\d+,\d+\))|(don't\(\))";
bool is_active = true;
foreach (var line in lines)
{
    var matches = Regex.Matches(line, pattern).Cast<Match>().Select(m => m.Value);
    foreach(var match in matches)
    {
        switch (match)
        {
            case "don't()":
                is_active = false;
                break;
            case "do()":
                is_active = true;
                break;
            default:
                sumpart1 += mult(match);
                if (is_active)
                    sumpart2 += mult(match);
                break;
        }
    }
}
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
int mult(string input)
{
    input = input.Remove(0, 4);
    input = input.Remove(input.Length - 1, 1);
    var a = input.Split(",").Select(int.Parse).ToArray();
    return a[0] * a[1];
}