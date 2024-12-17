using System.Text.RegularExpressions;
var lines = File.ReadAllLines("input.txt");
var regex = new Regex(@"(-?\d+)");
long r_a = long.Parse(regex.Matches(lines[0])[0].Value);
long r_b = long.Parse(regex.Matches(lines[1])[0].Value);
long r_c = long.Parse(regex.Matches(lines[2])[0].Value);
List<long> program = regex.Matches(lines[4]).Select(x => long.Parse(x.Value)).ToList();

Console.WriteLine("Part 1: " + string.Join(',', runProgram(program, r_a, r_b, r_c)));

var part2 = findA(program, 0, false);
Console.WriteLine("Part 2: " + part2.answer);

/* 
B = A % 8
B = B ^ 3
C = A >> B
B = B ^ 5
A = A >> 3
B = B ^ C
PRINT B % 8
IF A != 0 LOOP
 */
(List<long> list, long answer, bool failed) findA(List<long> target, long answer, bool failed)
{
    if (target.Count == 0)
        return ([], answer, false);
    for (int i = 0; i < 8; i++)
    {
        long a = (answer << 3) | i;
        long initial_a = a;
        long b = a % 8;
        b ^= 3;
        long c = a >> (int)b;
        b ^= 5;
        a >>= 3;
        b ^= c;
        b %= 8;
        if (b == target.Last())
        {
            var next = findA(target.SkipLast(1).ToList(), initial_a, false);
            if (next.failed)
                continue;
            return next;
        }
    }
    return ([], 0, true);
}

static List<long> runProgram(List<long> program, long regA, long regB, long regC)
{
    List<long> output = [];
    long pc = 0;
    while (true)
    { // combo-op:
      // 0-3: literal
      // 4: A 5: B 6: C
        if (pc >= program.Count)
            break;
        long operand = program[(int)pc + 1];
        switch (program[(int)pc])
        {
            case 0: // A =  A >> op 
                regA = regA >> (int)c_op(operand);
                break;
            case 1: // B =  B ^ op
                regB ^= operand;
                break;
            case 2: // B = op % 8 
                regB = c_op(operand) % 8;
                break;
            case 3: // !A jump
                if (regA != 0)
                {
                    pc = operand;
                    continue;
                }
                break;
            case 4: // B = B ^ C 
                regB ^= regC;
                break;
            case 5: // op % 8 -> out
                output.Add(c_op(operand) % 8);
                break;
            case 6: // B = A >> op 
                regB = regA / (int)c_op(operand);
                break;
            case 7: // C = A >> op
                regC = regA >> (int)c_op(operand);
                break;
            default:
                break;
        }
        pc += 2;
    }
    return output;
    long c_op(long value)
    {
        return value switch
        {
            < 4 => value,
            4 => regA,
            5 => regB,
            6 => regC,
            _ => throw new ArgumentException("Invalid value for operand")
        };
    }
}
