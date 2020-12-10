using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day09 : AbstractDay
    {
        private protected override object CachedResult1 => 1038347917;
        private protected override object CachedResult2 => 137394018;
        internal override string DayName => "Encoding Error";
        private protected override object Task1()
        {
            var nums = ReadInputAsLines<Int64>(Int64.TryParse);
            bool canCreateNum(int i)
            {
                for (var j = i - 25; j < i; j++)
                {
                    for (var y = j + 1; y < i; y++)
                    {
                        if (nums[j] + nums[y] == nums[i])
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            for (var i = 25; i < nums.Length; i++)
            {
                if (!canCreateNum(i))
                {
                    return nums[i];
                }
            }
            throw new Exception();
        }

        private protected override object Task2()
        {
            var goal = 1038347917L;
            var nums = ReadInputAsLines<Int64>(Int64.TryParse);
            var start = 0;
            while (true)
            {
                var sum = 0L;
                var curr = start;
                while (sum < goal)
                {
                    sum += nums[curr];
                    curr++;
                }
                if (sum == goal && (curr-1) > start)
                {
                    var len = curr - start;
                    var seq = nums.Skip(start).Take(len);
                    return seq.Min() + seq.Max();
                }
                start++;
            }
        }
    }
}
