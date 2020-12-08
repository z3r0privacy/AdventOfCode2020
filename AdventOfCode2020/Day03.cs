using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day03 : AbstractDay
    {
        internal override string DayName => "Toboggan Trajectory";
        private protected override object CachedResult1 => 191;
        private protected override object CachedResult2 => 1478615040;


        private protected override object Task1()
        {
            var map = ReadInputAsLines().Select(s => s.ToCharArray()).ToList();
            int x = 0, y = 0, cnt = 0;
            while (y < map.Count)
            {
                if (map[y][x] == '#')
                {
                    cnt++;
                }
                y++;
                x = (x + 3) % map[0].Count();
            }
            return cnt;
        }

        private protected override object Task2()
        {
            var map = ReadInputAsLines().Select(s => s.ToCharArray()).ToList();

            var dirs = new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            var count = new List<int>();

            foreach ((var mx, var my) in dirs)
            {
                int x = 0, y = 0, cnt = 0;
                while (y < map.Count)
                {
                    if (map[y][x] == '#')
                    {
                        cnt++;
                    }
                    y+=my;
                    x = (x + mx) % map[0].Count();
                }
                count.Add(cnt);
            }
            return count.Aggregate((i1, i2) => i1 * i2);
        }
    }
}
