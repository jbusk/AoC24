Dictionary<string, sbyte> gates = [];
Queue<(string in1, string in2, string result, string operation)> queue = [];
using (StreamReader filestream = new StreamReader("input.txt"))
{
    bool initialgates = true;
    while (!filestream.EndOfStream)
    {
        var line = filestream.ReadLine();
        if (line == "")
        {
            initialgates = false;
            line = filestream.ReadLine();
        }
        if (initialgates)
        {
            var split = line!.Split(": ");
            gates[split[0]] = sbyte.Parse(split[1]);
        }
        else
        {
            var split = line!.Split(' ');
            queue.Enqueue((split[0], split[2], split[4], split[1]));
        }
    }
}

while (queue.Count > 0)
{
    var (in1, in2, result, operation) = queue.Dequeue();
    if (gates.TryGetValue(in1, out var in1val) && gates.TryGetValue(in2, out var in2val))
    {
        gates[result] = getOperation(operation)(in1val, in2val);
    }
    else
    {
        queue.Enqueue((in1, in2, result, operation));
    }
}

var part1 = string.Join("", gates.Where(x => x.Key.StartsWith('z')).OrderByDescending(x => x.Key).Select(x => x.Value));
Console.WriteLine("Part 1: " + Convert.ToInt64(part1, 2));

static Func<sbyte, sbyte, sbyte> getOperation(string operand) => operand switch
{
    "XOR" => static (x, y) => (sbyte)(x ^ y), // xor
    "OR" => static (x, y) => (sbyte)(x | y),  // or
    "AND" => static (x, y) => (sbyte)(x & y),  // and
    _ => throw new ArgumentException("Unknown operation")
};