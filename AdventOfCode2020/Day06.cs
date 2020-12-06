using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day06 : AbstractDay
    {
        private protected override object CachedResult1 => 6768;
        private protected override object CachedResult2 => 3489;

        private protected override object Task1()
        {
            var lines = ReadInputAsLines();
            var sum = 0;
            var str = "";
            foreach (var l in lines)
            {
                if (string.IsNullOrWhiteSpace(l))
                {
                    sum += str.Distinct().Count();
                    str = "";
                } else
                {
                    str += l;
                }
            }
            sum += str.Distinct().Count();
            return sum;
        }

        private protected override object Task2()
        {
            var lines = ReadInputAsLines();
            var current = new List<string>();
            var sum = 0;
            foreach (var l in lines)
            {
                if (string.IsNullOrWhiteSpace(l))
                {
                    sum += Enumerable.Range('a', 26).Count(c => current.All(e => e.Contains((char)c)));
                    current.Clear();
                } else
                {
                    current.Add(l);
                }
            }
            sum += Enumerable.Range('a', 26).Count(c => current.All(e => e.Contains((char)c)));
            return sum;
        }
    }
}
