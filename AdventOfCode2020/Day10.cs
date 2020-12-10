using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day10 : AbstractDay
    {
        private protected override object CachedResult1 => 1700;
        private protected override object CachedResult2 => 12401793332096;
        internal override string DayName => "Adapter Array";
        private protected override object Task1()
        {
            var nums = ReadInputAsLines<int>(int.TryParse).OrderBy(i => i);
            var q = new Queue<int>(nums);
            var currOut = 0;
            var num1 = 0;
            var num3 = 0;
            while (q.Count > 0)
            {
                var next = q.Dequeue();
                var diff = next - currOut;
                if (diff > 3 || diff < 1)
                {
                    throw new Exception();
                }
                if (diff == 1) num1++;
                if (diff == 3) num3++;
                currOut = next;
            }
            num3++;
            return num1 * num3;
        }

        private long FindWays(int curr, List<int> remaining, int goal)
        {
            if (curr == goal) return 1;
            if (curr > goal) return 0;

            var count = 0L;
            foreach (var r in new List<int>(remaining))
            {
                var diff = r - curr;
                if (diff > 0 && diff <= 3)
                {
                    remaining.Remove(r);
                    count += FindWays(r, remaining, goal);
                    remaining.Add(r);
                }
            }
            return count;
        }
        private protected override object Task2()
        {
            var nums = ReadInputAsLines<int>(int.TryParse).OrderBy(i => i).ToList();

            var possibilities = new List<long>();
            var start = 0;
            for (var i = 1; i < nums.Count; i++)
            {
                if (nums[i] - nums[i - 1] == 3)
                {
                    possibilities.Add(FindWays(start, nums.ToList(), nums[i - 1])); ;
                    start = nums[i];
                }
            }
            possibilities.Add(FindWays(start, nums.ToList(), nums.Last()));

            return possibilities.Aggregate((l1,l2) => l1*l2);
        }
    }
}
