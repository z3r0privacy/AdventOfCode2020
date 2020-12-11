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
                foreach (var m2 in mods)
                {
                    if (m1 == 0 && m2 == 0) continue;

                    try
                    {
                        if (layout[y + m1][x + m2] == find)
                        {
                            count++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return count;
        }

        private protected override object Task1()
        {
            var layout = ReadInputAsLines().Select(l => l.ToCharArray()).ToArray();

            var hasChanges = true;
            while (hasChanges)
            {
                hasChanges = false;
                var newLayout = new char[layout.Length][];
                for (var r = 0; r < layout.Length; r++)
                {
                    newLayout[r] = new char[layout[r].Length];
                    for (var c = 0; c < newLayout[r].Length; c++)
                    {
                        if (layout[r][c] == FREE && CountSurrounding(layout, TAKEN, c, r) == 0)
                        {
                            newLayout[r][c] = TAKEN;
                            hasChanges = true;
                        } else if (layout[r][c] == TAKEN && CountSurrounding(layout, TAKEN, c, r) >= 4)
                        {
                            newLayout[r][c] = FREE;
                            hasChanges = true;
                        } else
                        {
                            newLayout[r][c] = layout[r][c];
                        }
                    }
                }
                layout = newLayout;
            }

            return layout.Sum(r => r.Count(c => c == TAKEN));
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
                    try
                    {
                        if (layout[y2][x2] == FLOOR) continue;
                        if (layout[y2][x2] == find) return true;
                        return false;
                    } catch (IndexOutOfRangeException)
                    {
                        return false;
                    } catch(Exception)
                    {
                        throw;
                    }
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
            var layout = ReadInputAsLines().Select(l => l.ToCharArray()).ToArray();

            var hasChanges = true;
            while (hasChanges)
            {
                hasChanges = false;
                var newLayout = new char[layout.Length][];
                for (var r = 0; r < layout.Length; r++)
                {
                    newLayout[r] = new char[layout[r].Length];
                    for (var c = 0; c < newLayout[r].Length; c++)
                    {
                        if (layout[r][c] == FREE && CountVisible(layout, TAKEN, c, r) == 0)
                        {
                            newLayout[r][c] = TAKEN;
                            hasChanges = true;
                        }
                        else if (layout[r][c] == TAKEN && CountVisible(layout, TAKEN, c, r) >= 5)
                        {
                            newLayout[r][c] = FREE;
                            hasChanges = true;
                        }
                        else
                        {
                            newLayout[r][c] = layout[r][c];
                        }
                    }
                }
                layout = newLayout;
            }

            return layout.Sum(r => r.Count(c => c == TAKEN));
        }
    }
}
