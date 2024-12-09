using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var line = File.ReadAllText("sample.txt");
(long sumpart1, long sumpart2) = (0, 0);

bool file = true;
long filecounter = 0;
List<(long value, bool empty)> disklist = [];
List<(long value, bool empty, int length, bool moved)> disklist2 = [];
for (int i = 0; i < line.Length; i++)
{
    int sectorlength = line[i] & 15;
    if (file)
    {
        disklist2.Add((filecounter, false, sectorlength, false));
        for (int x = 0; x < sectorlength; x++)
        {
            disklist.Add((filecounter, false));
        }
        filecounter++;
        file = false;
    }
    else
    {
        disklist2.Add((0, true, sectorlength, false));
        for (int x = 0; x < sectorlength; x++)
        {
            disklist.Add((0, true));
        }
        file = true;
    }
}

var disk1 = disklist.ToArray();

visualise(disk1);

int backwards = disk1.Length - 1;
// part 1
for (int i = 0; i < disk1.Length; i++)
{
    if (!disk1[i].empty)
        continue;
    while (backwards > i)
        if (disk1[backwards].empty)
            backwards--;
        else
            break;
    (disk1[i], disk1[backwards]) = (disk1[backwards], disk1[i]);
}

visualise(disk1);

for (int i = 0; i < disk1.Length; ++i)
{
    sumpart1 += i * disk1[i].value;
}


List<(long value, bool empty)> list2 = [];
for (int i = 0; i < disklist2.Count; ++i)
{
    var sector = disklist2[i];
    var length = sector.length;
    if (!sector.empty)
    {
        for (int j = 0; j < sector.length; ++j)
            list2.Add((sector.value, false));
    }
    else
    {
        while (length > 0)
        {
            // find an un-moved file of length <= this free space's length from the end of disklist2
            var filelist = disklist2.Where(x => !x.moved && !x.empty && x.length <= sector.length);
            if (filelist.Count() != 0)
            {
                var fil = filelist.LastOrDefault();
                for (int j = 0; j < sector.length; j++)
                {
                    list2.Add((fil.value, false));
                }
                length -= fil.length;
                fil.moved = true;
            }
            else
            {
                for (int j = 0; j < length; j++)
                {
                    list2.Add((0, true));
                    length--;
                }
            }
        }
    }

}
var disk2 = list2.ToArray();
visualise(disk2);
for (int i = 0; i < disk2.Length; ++i)
{
    sumpart2 += i * disk2[i].value;
}

Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine("Time: " + sw.Elapsed);

static void visualise((long value, bool empty)[] disk)
{
    for (int i = 0; i < disk.Length; i++)
    {
        if (disk[i].empty)
            Console.Write(".");
        else
            Console.Write(disk[i].value);
    }
    Console.WriteLine();
}

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