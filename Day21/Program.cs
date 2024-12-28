using Position = (int x, int y);
var codes = File.ReadAllLines("input.txt");
(long sumpart1, long sumpart2) = (0, 0);
(char d, Position p)[] directions = [('v', (0, 1)), ('>', (1, 0)), ('^', (0, -1)), ('<', (-1, 0))];
var numpad = new char[,] { { '7', '8', '9' },
                           { '4', '5', '6' },
                           { '1', '2', '3' },
                           { '#', '0', 'A' }};
var dirpad = new char[,] { { '#', '^', 'A' },
                           { '<', 'v', '>' }};
Dictionary<(char from, char to), List<string>> shortest_sequences = [];
Dictionary<(string code, int level), long> sequence_cache = [];

cache_all_shortest(numpad);
cache_all_shortest(dirpad);
foreach (var code in codes)
{
    var numeric = int.Parse(code[..^1]);
    var seq1 = get_shortest_sequence_length(code, 0, 3);
    sumpart1 += numeric * seq1;
}
sequence_cache.Clear();
foreach (var code in codes)
{
    var numeric = int.Parse(code[..^1]);
    var seq2 = get_shortest_sequence_length(code, 0, 26);
    sumpart2 += numeric * seq2;
}

Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);

long get_shortest_sequence_length(string code, int layer, int stop_at)
{
    if (sequence_cache.TryGetValue((code, layer), out var cached_value))
        return cached_value;
    if (layer == stop_at)
    {
        sequence_cache[(code, layer)] = code.LongCount();
        return code.LongCount();
    }

    long best = 0;
    char prev = 'A';
    foreach (var curr in code)
    {
        var keypad = layer switch
        {
            0 => numpad,
            _ => dirpad
        };
        var paths = shortest_sequences;
        var shortest_paths = paths[(prev, curr)];
        long current_best = long.MaxValue;
        foreach (var path in shortest_paths)
        {
            var length = get_shortest_sequence_length(path, layer + 1, stop_at);
            if (current_best > length)
                current_best = length;
        }
        best += current_best;
        prev = curr;
    }
    sequence_cache[(code, layer)] = best;
    return best;
}

void cache_all_shortest(char[,] keypad)
{
    foreach (var from in keypad)
    {
        if (from == '#')
            continue;
        foreach (var to in keypad)
        {
            if (to == '#')
                continue;
            shortest_sequences[(from, to)] = [];
            if (from == to)
            {
                shortest_sequences[(from, to)].Add("A");
                continue;
            }
            cache_shortest_sequence(from, to, keypad);
        }
    }
}

void cache_shortest_sequence(char from, char to, char[,] keypad)
{
    Position from_position = find(from, keypad);
    Position to_position = find(to, keypad);
    int shortest_path_length = manhattan(from_position, to_position);

    Queue<(Position, string seq)> queue = [];
    queue.Enqueue((from_position, ""));
    while (queue.Count > 0)
    {
        var (pos, sequence) = queue.Dequeue();

        if (pos == to_position && sequence.Length == shortest_path_length)
        {
            shortest_sequences[(from, to)].Add(sequence + "A");
            continue;
        }
        if (sequence.Length >= shortest_path_length)
            continue;

        foreach (var (direction_char, direction_pos) in directions)
        {
            Position nextpos = (pos.x + direction_pos.x, pos.y + direction_pos.y);
            if (nextpos.x >= 0 && nextpos.x < keypad.GetLength(1) &&
                nextpos.y >= 0 && nextpos.y < keypad.GetLength(0) &&
                keypad[nextpos.y, nextpos.x] != '#')
            {
                queue.Enqueue((nextpos, sequence + direction_char));
            }
        }
    }
}

Position find(char c, char[,] keypad)
{
    for (int y = 0; y < keypad.GetLength(0); y++)
    {
        for (int x = 0; x < keypad.GetLength(1); x++)
        {
            if (keypad[y, x] == c)
                return (x, y);
        }
    }
    throw new ArgumentException(c + " is not a valid character for this keypad");
}

int manhattan(Position pos1, Position pos2) => Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y);
