using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day08 : AbstractDay
    {
        internal override string DayName => "Handheld Halting";
        private protected override object CachedResult1 => 2003;
        private protected override object CachedResult2 => 1984;

        private (int accValue, bool endedCorrectly) RunAccumulator(List<string> instructions)
        {
            var eip = 0;
            var acc = 0;
            var history = new List<int>();

            while (true)
            {
                if (eip == instructions.Count)
                {
                    return (acc, true);
                } else if (eip < 0 || eip > instructions.Count)
                {
                    return (acc, false);
                }
                if (history.Contains(eip))
                {
                    return (acc, false);
                }
                history.Add(eip);
                var split = instructions[eip].Split(" ");
                var instr = split[0];
                var num = int.Parse(split[1]);
                switch (instr)
                {
                    case "acc":
                        acc += num;
                        eip++;
                        break;
                    case "nop":
                        eip++;
                        break;
                    case "jmp":
                        eip += num;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private protected override object Task1()
        {
            var instructions = ReadInputAsLines().ToList();
            return RunAccumulator(instructions).accValue;
        }

        private protected override object Task2()
        {
            var instructions = ReadInputAsLines();
            for (var i = 0; i < instructions.Length; i++)
            {
                var bku = instructions[i];
                var opcode = instructions[i][..3];
                if (opcode == "jmp")
                {
                    instructions[i] = "nop" + instructions[i][3..];
                } else if (opcode == "nop")
                {
                    instructions[i] = "jmp" + instructions[i][3..];
                } else
                {
                    continue;
                }

                (var acc, var finished) = RunAccumulator(instructions.ToList());
                if (finished)
                {
                    return acc;
                }

                instructions[i] = bku;
            }
            throw new Exception();
        }
    }
}
