using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var line = File.ReadAllText("input.txt");
(long sumpart1, long sumpart2) = (0, 0);

bool file = true;
int filecounter = 0;
List<(int value, bool empty)> disklist = [];
List<(int value, bool empty, int length)> disklist2 = [];
for (int i = 0; i < line.Length; i++)
{
    int sectorlength = line[i] & 15;
    if (file)
    {
        disklist2.Add((filecounter, false, sectorlength));
        for (int x = 0; x < sectorlength; x++)
        {
            disklist.Add((filecounter, false));
        }
        filecounter++;
        file = false;
    }
    else
    {
        disklist2.Add((0, true, sectorlength));
        for (int x = 0; x < sectorlength; x++)
        {
            disklist.Add((0, true));
        }
        file = true;
    }
}

var disk1 = disklist.ToArray();

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


for (int i = 0; i < disk1.Length; ++i)
{
    sumpart1 += i * disk1[i].value;
}

// part 2
List<(long value, bool empty, int length)> p2list = [];

while (true)
{
    var curr = disklist2.First();
    if (!curr.empty)
    {
        p2list.Add(curr);
        disklist2.Remove(curr);
    }
    else // curr is empty
    {
        int length = curr.length;
        while (length > 0)
        {
            var last_matches = disklist2.Where(x => !x.empty && x.length <= length);
            if (last_matches.Count() == 0)
                break;
            var last_match = last_matches.Last();
            length -= last_match.length;
            p2list.Add(last_match);
            var last_idx = disklist2.IndexOf(last_match);
            disklist2[last_idx] = (0, true, last_match.length);
            disklist2.Remove(last_match);
            
        }
        if (length > 0)
        {
            p2list.Add((0, true, length));
        }
        disklist2.Remove(curr);
    }
    if (disklist2.Count == 0)
        break;
}



List<(long value, bool empty)> disk2 = [];
foreach (var sektor in p2list)
{
    for (int i = 0; i < sektor.length; i++)
    {
        disk2.Add((sektor.value, sektor.empty));
    }
}

for (int i = 0; i < disk2.Count; ++i)
{
    sumpart2 += i * disk2[i].value;
}

Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine("Time: " + sw.Elapsed);

