using System.Diagnostics;
using System.Text;
Stopwatch sw = Stopwatch.StartNew();
var line = File.ReadAllText("sample.txt");
(long sumpart1, long sumpart2) = (0, 0);

bool file = true;
long filecounter = 0;
long diskcounter = 0;
HashSet<(long pos, Sector)> disk = [];
for (int i = 0; i < line.Length; i++)
{
    int pos = line[i] & 15;
    if (file)
    {
        for (int x = 0; x < pos; x++)
        {
            disk.Add((diskcounter, new Sector(filecounter)));
            diskcounter++;
        }
        filecounter++;
        file = false;
    }
    else
    {
        for (int x = 0; x < pos; x++)
        {
            disk.Add((diskcounter, new Sector(0,true)));
            diskcounter++;
        }
        file = true;
    }
}

foreach (var sector in disk)
{
    Console.Write(sector.Item2);
}
Console.WriteLine();
Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine("Time: " + sw.Elapsed);

class Sector
{
    public Sector(long value, bool empty = false)
    {
        Value = value;
        Empty = empty;
    }
    public long Value { get; set; }
    public bool Empty { get; set; }

    public override string ToString()
    {
        if (Empty)
            return ".";
        return Value.ToString();
    }
}