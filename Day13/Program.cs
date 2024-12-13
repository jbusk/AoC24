using System.Diagnostics;
Stopwatch sw = Stopwatch.StartNew();
var slots = File.ReadAllText("input.txt").Split("\r\n\r\n");
(long sumpart1, long sumpart2) = (0, 0);
foreach (var slot in slots)
{
    var split = slot.Split('\n');
    var axy = split[0][12..].Split(", Y+").Select(int.Parse).ToArray();
    (int x, int y) a = (axy[0], axy[1]);
    var bxy = split[1][12..].Split(", Y+").Select(int.Parse).ToArray();
    (int x, int y) b = (bxy[0], bxy[1]);
    var gxy = split[2][9..].Split(", Y=").Select(int.Parse).ToArray();
    (int x, int y) g = (gxy[0], gxy[1]);
    sumpart1 += solve(a, b, g);
    sumpart2 += solve(a, b, g, true);
}

Console.WriteLine("Part 1: " + sumpart1);
Console.WriteLine("Part 2: " + sumpart2);
Console.WriteLine("Time: " + sw.Elapsed);

static long solve((int x, int y) aval, (int x, int y) bval, (long x, long y) gval, bool part2 = false)
{
    long cost = 0;
    var (ax, ay) = aval;
    var (bx, by) = bval;
    var (gx, gy) = gval;
    if (part2)
    {
        gx += 10000000000000;
        gy += 10000000000000;
    }
    //      (gx * by) - (gy * bx)
    // a =  ---------------------
    //      (ax * by) - (ay * bx)
    // if ax * by == ay * bx, there is no solution (divide by zero) and we can return 0 immediately
    if (ax * by == ay * bx)
        return cost;
    var a = ((gx * by) - (gy * bx)) / ((ax * by) - (ay * bx));
    //     gx - ax*a
    // b = ---------
    //        bx
    var b = (gx - (ax * a)) / bx;
    // sanity check, press A a times and B b times and see if they equal the goal values
    if (a * ax + b * bx == gx && a * ay + b * by == gy)
        cost = 3 * a + b;
    return cost;
}

