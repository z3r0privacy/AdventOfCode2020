using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day15 : AbstractDay
    {
        private protected override object CachedResult1 => 447;
        private protected override object CachedResult2 => 11721679;
        internal override string DayName => "Rambunctious Recitation";

        private protected override object Task1()
        {
            var numbers = ReadInput().Split(',').Select(str => int.Parse(str)).ToList();
            while (numbers.Count < 2020)
            {
                var lastNr = numbers.Last();
                var nextNr = 0;
                for (var i = 1; i < numbers.Count; i++)
                {
                    if (numbers[numbers.Count-1-i] == lastNr)
                    {
                        nextNr = i;
                        break;
                    }
                }
                numbers.Add(nextNr);
            }
            return numbers.Last();
        }

        private protected override object Task2()
        {
            var numbers = ReadInput().Split(',').Select(str => int.Parse(str)).ToList();
            var lastSeen = new Dictionary<int, int>();
            for (var i = 0; i < numbers.Count-1; i++)
            {
                lastSeen[numbers[i]] = i+1;
            }
            var flow = numbers.Last();
            while (numbers.Count < 30000000)
            {
                var nextNr = 0;
                if (lastSeen.ContainsKey(flow))
                {
                    nextNr = numbers.Count - lastSeen[flow];
                }
                lastSeen[flow] = numbers.Count;
                numbers.Add(flow);
                flow = nextNr;
            }
            return flow;
        }
    }
}
