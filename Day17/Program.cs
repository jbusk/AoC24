﻿using System.Text.RegularExpressions;
var lines = File.ReadAllLines("input.txt");
var regex = new Regex(@"(-?\d+)");
int regA = int.Parse(regex.Matches(lines[0])[0].Value);
int regB = int.Parse(regex.Matches(lines[1])[0].Value);
int regC = int.Parse(regex.Matches(lines[2])[0].Value);
List<int> program = regex.Matches(lines[4]).Select(x => int.Parse(x.Value)).ToList();
List<int> output = [];
int pc = 0;
while (true)
{
    if (pc >= program.Count)
        break;
    switch (program[pc])
    {
        case 0:
            inst_adv(program[pc + 1]);
            break;
        case 1:
            inst_bxl(program[pc + 1]);
            break;
        case 2:
            inst_bst(program[pc + 1]);
            break;
        case 3:
            inst_jnz(program[pc + 1]);
            break;
        case 4:
            inst_bxc(program[pc + 1]);
            break;
        case 5:
            inst_out(program[pc + 1]);
            break;
        case 6:
            inst_bdv(program[pc + 1]);
            break;
        case 7:
            inst_cdv(program[pc + 1]);
            break;
        default:
            break;
    }
}

Console.WriteLine("Part 1: " + string.Join(',', output));

int combo_operand(int value)
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

int twopow(int value)
{
    return (int)(Math.Pow(2, value));
}

void inst_adv(int operand) // op 0
{
    var denom = twopow(combo_operand(operand));
    regA /= denom;
    pc += 2;
}

void inst_bxl(int operand) // op 1
{
    regB ^= operand;
    pc += 2;
}

void inst_bst(int operand) // op 2
{
    var op = combo_operand(operand);
    regB = op % 8;
    pc += 2;
}

void inst_jnz(int operand) // op 3
{
    if (regA == 0)
    {
        pc += 2;
        return;
    }
    pc = operand;
}

void inst_bxc(int operand) // op 4
{
    regB ^= regC;
    pc += 2;
}

void inst_out(int operand) // op 5
{
    var op = combo_operand(operand);
    output.Add(op % 8);
    pc += 2;
}

void inst_bdv(int operand) // op 6
{
    var denom = twopow(combo_operand(operand));
    regB = regA / denom;
    pc += 2;
}

void inst_cdv(int operand) // op 7
{
    var denom = twopow(combo_operand(operand));
    regC = regA / denom;
    pc += 2;
}