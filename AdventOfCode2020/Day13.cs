using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day13 : AbstractDay
    {
        private protected override object CachedResult1 => 1835;
        private protected override object CachedResult2 => 247086664214628;
        internal override string DayName => "Shuttle Search";

        private protected override object Task1()
        {
            var input = ReadInputAsLines().Select(l => l.Split(",").Where(str => str != "x").Select(i => int.Parse(i)).ToList()).ToList();
            var lowestId = -1;
            var lowestWait = int.MaxValue;
            foreach (var i in input[1])
            {
                var mult = input[0][0] / i;
                var wait = (mult + 1) * i - input[0][0];
                if (wait < lowestWait)
                {
                    lowestWait = wait;
                    lowestId = i;
                }
            }
            return lowestId * lowestWait;
        }

        private protected override object Task2()
        {
            var input = ReadInputAsLines()[1].Split(",").Select(str => str == "x" ? 0 : int.Parse(str)).ToList();

            var t = (long)input[0];
            var jmp = t;

            for (var i = 1; i < input.Count(); i++)
            {
                if (input[i] == 0)
                {
                    continue;
                }
                var fst = 0L;
                while (true)
                {
                    t += jmp;
                    if ((t+i)%input[i] == 0)
                    {
                        if (fst == 0)
                        {
                            fst = t;
                        } else
                        {
                            jmp = t - fst;
                            t = fst;
                            break;
                        }
                    }
                }
            }
            return t;
        }
    }
}
