var input = File.ReadAllText("input.txt").Split("\r\n\r\n").Select(x => x.Split("\r\n"));
List<int[]> keys = [];
List<int[]> locks = [];
foreach (var thing in input)
{
    if (thing[0][0] == '#')
        locks.Add(handleThing(thing[1..]));
    else
        keys.Add(handleThing(thing[..^1]));
}
(int sumpart1, int sumpart2) = (0, 0);

foreach (var hole in locks)
    sumpart1 += keys.Count(x => (x[0] + hole[0] <= 5) &&
                                (x[1] + hole[1] <= 5) &&
                                (x[2] + hole[2] <= 5) &&
                                (x[3] + hole[3] <= 5) &&
                                (x[4] + hole[4] <= 5));

Console.WriteLine(sumpart1);

int[] handleThing(string[] thing)
{
    int[] retval = new int[thing[0].Length];
    for (int i = 0; i < 6; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            if (thing[i][j] == '#')
                retval[j]++;
        }
    }
    return retval;
}
