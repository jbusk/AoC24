var lines = File.ReadAllLines("input.txt").Select(long.Parse);
(long sumpart1, long sumpart2) = (0, 0);
Dictionary<(int, int, int, int), int> allSequences = [];
foreach (var line in lines)
{
    sumpart1 += generate_secrets(line, 2000, out var sequences);
    foreach (var sequence in sequences)
    {
        if (allSequences.TryGetValue(sequence.Key, out var value))
        {
            allSequences[sequence.Key] = sequence.Value + value;
        }
        else
        {
            allSequences[sequence.Key] = sequence.Value;
        }
    }
}

Console.WriteLine("Part 1: " + sumpart1);
var kvp = allSequences.MaxBy(x => x.Value);
Console.WriteLine("Part 2: " + kvp.Value);
long generate_secrets(long secret, int iterations, out Dictionary<(int, int, int, int), int> sequences)
{
    sequences = [];
    (int a, int b, int c, int d) = (0, 0, 0, 0);
    long prev_secret = 0;
    int prev = 0;
    int current_price = 0;
    for (int i = 0; i < iterations + 1; i++)
    {
        prev_secret = secret;
        secret = next(secret);
        current_price = lastdigit(secret);
        var diff = current_price - prev;
        (a, b, c, d) = (b, c, d, diff);
        if (i > 3)
        {
            sequences.TryAdd((a, b, c, d), current_price);
        }
        prev = current_price;
    }
    return prev_secret;
}

long next(long secret)
{
    secret = prune(mix(secret, secret << 6));
    secret = prune(mix(secret, secret >> 5));
    secret = prune(mix(secret, secret << 11));
    return secret;
}

long mix(long number, long value) => number ^ value;

long prune(long secret) => secret % 16777216;

int lastdigit(long number) => (int)(number % 10);