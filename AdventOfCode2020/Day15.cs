using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day15 : AbstractDay
    {
        //private protected override object CachedResult1 => 447;
        //private protected override object CachedResult2 => 11721679;
        internal override string DayName => "Rambunctious Recitation";

        private List<int> _numbers;
        private protected override void Init()
        {
            _numbers = ReadInput().Split(',').Select(str => int.Parse(str)).ToList();
        }

        private int MemoryGame(int goal)
        {
            var localNumbers = _numbers.ToList();
            var lastSeen = new int[goal];  // ca 120MB, needs more RAM, but faster / use dictionary for a bit slower but smaller solution
            // new Dictionary<int, int>();
            for (var i = 0; i < localNumbers.Count - 1; i++)
            {
                lastSeen[localNumbers[i]] = i + 1;
            }
            var flow = localNumbers.Last();
            while (localNumbers.Count < goal)
            {
                var nextNr = 0;
//                if (lastSeen.ContainsKey(flow))
                if (lastSeen[flow] != 0)
                {
                    nextNr = localNumbers.Count - lastSeen[flow];
                }
                lastSeen[flow] = localNumbers.Count;
                localNumbers.Add(flow);
                flow = nextNr;
            }
            return flow;
        }

        private protected override object Task1()
        {
            return MemoryGame(2020);
        }

        private protected override object Task2()
        {
            return MemoryGame(30000000);
        }
    }
}
