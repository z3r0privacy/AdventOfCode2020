using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day01 : AbstractDay
    {
        internal override string DayName => "Report Repair";
        private protected override object CachedResult1 => 913824;
        private protected override object CachedResult2 => 240889536;

        private protected override object Task1()
        {
            var nums =  ReadInputAsLines<int>(int.TryParse);
            for (var i = 0; i < nums.Length; i++)
            {
                for (var j = i+1; j < nums.Length; j++)
                {
                    if (nums[i]+nums[j] == 2020)
                    {
                        return nums[i] * nums[j];
                    }
                }
            }
            throw new Exception();
        }

        private protected override object Task2()
        {
            var nums = ReadInputAsLines<int>(int.TryParse);
            for (var i = 0; i < nums.Length; i++)
            {
                for (var j = i + 1; j < nums.Length; j++)
                {
                    for (var y = j +1; y < nums.Length; y++)
                    {
                        if (nums[i] + nums[j] + nums[y] == 2020)
                        {
                            return nums[i] * nums[j] * nums[y];
                        }
                    }
                }
            }
            throw new Exception();
        }
    }
}
