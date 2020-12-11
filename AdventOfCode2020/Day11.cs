using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day11 : AbstractDay
    {
        private protected override object CachedResult1 => 2277;
        private protected override object CachedResult2 => 2066;
        internal override string DayName => "Seating System";

        private const char FLOOR = '.';
        private const char FREE = 'L';
        private const char TAKEN = '#';

        private int CountSurrounding(char[][] layout, char find, int x, int y)
        {
            var count = 0;
            var mods = new[] { -1, 0, 1 };
            foreach (var m1 in mods)
            {
                var y2 = m1 + y;
                if (y2 >= 0 && y2 < layout.Length)
                {
                    foreach (var m2 in mods)
                    {
                        if (m1 == 0 && m2 == 0) continue;

                        var x2 = m2 + x;
                        if (x2 >= 0 && x2 < layout[y2].Length)
                        {
                            if (layout[y + m1][x + m2] == find)
                            {
                                count++;
                            }
                        }
                    }
                }
            }
            return count;
        }

        private protected override object Task1()
        {
            var layout1 = ReadInputAsLines().Select(l => l.ToCharArray()).ToArray();

            var layout2 = new char[layout1.Length][];
            for(var i = 0; i < layout2.Length; i++)
            {
                layout2[i] = new char[layout1[i].Length];
            }

            var current = layout1;
            var next = layout2;

            var hasChanges = true;
            while (hasChanges)
            {
                hasChanges = false;
                for (var r = 0; r < layout1.Length; r++)
                {
                    for (var c = 0; c < layout1[r].Length; c++)
                    {
                        if (current[r][c] == FREE && CountSurrounding(current, TAKEN, c, r) == 0)
                        {
                            next[r][c] = TAKEN;
                            hasChanges = true;
                        } else if (current[r][c] == TAKEN && CountSurrounding(current, TAKEN, c, r) >= 4)
                        {
                            next[r][c] = FREE;
                            hasChanges = true;
                        } else
                        {
                            next[r][c] = current[r][c];
                        }
                    }
                }
                current = next;
                next = current == layout1 ? layout2 : layout1;
            }

            return layout1.Sum(r => r.Count(c => c == TAKEN));
        }


        private int CountVisible(char[][] layout, char find, int x, int y)
        {
            bool SearchDirection(int mx, int my)
            {
                var x2 = x; var y2 = y;
                while (true)
                {
                    x2 += mx;
                    y2 += my;
                    if (y2 >= 0 && y2 < layout.Length)
                    {
                        if (x2 >= 0 && x2 < layout[y2].Length)
                        {
                            if (layout[y2][x2] == FLOOR) continue;
                            if (layout[y2][x2] == find) return true;
                            return false;
                        }
                    }
                    return false;
                }
            }

            var count = 0;
            var mods = new[] { -1, 0, 1 };
            foreach (var m1 in mods)
            {
                foreach (var m2 in mods)
                {
                    if (m1 == 0 && m2 == 0) continue;

                    if (SearchDirection(m1, m2))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private protected override object Task2()
        {
            var layout1 = ReadInputAsLines().Select(l => l.ToCharArray()).ToArray();

            var layout2 = new char[layout1.Length][];
            for (var i = 0; i < layout2.Length; i++)
            {
                layout2[i] = new char[layout1[i].Length];
            }

            var current = layout1;
            var next = layout2;
            var hasChanges = true;
            while (hasChanges)
            {
                hasChanges = false;
                for (var r = 0; r < layout1.Length; r++)
                {
                    for (var c = 0; c < layout1[r].Length; c++)
                    {
                        if (current[r][c] == FREE && CountVisible(current, TAKEN, c, r) == 0)
                        {
                            next[r][c] = TAKEN;
                            hasChanges = true;
                        }
                        else if (current[r][c] == TAKEN && CountVisible(current, TAKEN, c, r) >= 5)
                        {
                            next[r][c] = FREE;
                            hasChanges = true;
                        }
                        else
                        {
                            next[r][c] = current[r][c];
                        }
                    }
                }
                current = next;
                next = current == layout1 ? layout2 : layout1;
            }

            return layout1.Sum(r => r.Count(c => c == TAKEN));
        }
    }
}
