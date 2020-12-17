using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day17 : AbstractDay
    {
        private protected override object CachedResult1 => 257;
        private protected override object CachedResult2 => 2532;
        internal override string DayName => "Conway Cubes";

        private const char ACTIVE = '#';
        private const char INACTIVE = '.';

        private int CountSurroundingActive3D(Dictionary<(int x, int y, int z), char> dimension, int x, int y, int z)
        {
            var count = 0;

            var mutations = new[] { -1, 0, 1 };
            foreach (var mz in mutations)
            {
                foreach (var my in mutations)
                {
                    foreach (var mx in mutations)
                    {
                        if (mz == 0 && my == 0 && mx == 0)
                        {
                            continue;
                        }
                        var _z = z + mz;
                        var _y = y + my;
                        var _x = x + mx;

                        if (dimension.TryGetValue((_x, _y, _z), out var val) && val == ACTIVE)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private int CountSurroundingActive4D(Dictionary<(int x, int y, int z, int w), char> dimension, int x, int y, int z, int w)
        {
            var count = 0;

            var mutations = new[] { -1, 0, 1 };
            foreach (var mz in mutations)
            {
                foreach (var my in mutations)
                {
                    foreach (var mx in mutations)
                    {
                        foreach (var mw in mutations)
                        {
                            if (mz == 0 && my == 0 && mx == 0 && mw == 0)
                            {
                                continue;
                            }
                            var _z = z + mz;
                            var _y = y + my;
                            var _x = x + mx;
                            var _w = w + mw;

                            if (dimension.TryGetValue((_x, _y, _z, _w), out var val) && val == ACTIVE)
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
            var input = ReadInputAsLines().Select(l => l.ToCharArray()).ToList();

            var dict = new Dictionary<(int x, int y, int z), char>();
            
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    dict[(x, y, 0)] = input[y][x];
                }
            }

            for (var i = 0; i < 6; i++)
            {
                var helpDict = new Dictionary<(int x, int y, int z), char>();
                for (var iz = dict.Keys.Min(k => k.z) - 1; iz <= dict.Keys.Max(k => k.z)+1; iz++)
                {
                    for (var iy = dict.Keys.Min(k => k.y)- 1; iy <= dict.Keys.Max(k => k.y)+1; iy++)
                    {
                        for (var ix = dict.Keys.Min(k => k.x) - 1; ix <= dict.Keys.Max(k => k.x)+1; ix++)
                        {
                            var pos = (x:ix, y:iy, z:iz);
                            var state = dict.GetValueOrDefault(pos, INACTIVE);

                            var countSur = CountSurroundingActive3D(dict, pos.x, pos.y, pos.z);
                            if (state == ACTIVE && (countSur == 2 || countSur == 3))
                            {
                                helpDict[pos] = ACTIVE;
                            }
                            else if (state == INACTIVE && countSur == 3)
                            {
                                helpDict[pos] = ACTIVE;
                            }
                        }
                    }
                }
                dict = helpDict;
            }
            return dict.Values.Count(v => v == ACTIVE);
        }

        private protected override object Task2()
        {
            var input = ReadInputAsLines().Select(l => l.ToCharArray()).ToList();

            var dict = new Dictionary<(int x, int y, int z, int w), char>();

            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    dict[(x, y, 0, 0)] = input[y][x];
                }
            }

            for (var i = 0; i < 6; i++)
            {
                //drawDimensions(dict);
                var helpDict = new Dictionary<(int x, int y, int z, int w), char>();
                for (var iz = dict.Keys.Min(k => k.z) - 1; iz <= dict.Keys.Max(k => k.z) + 1; iz++)
                {
                    for (var iy = dict.Keys.Min(k => k.y) - 1; iy <= dict.Keys.Max(k => k.y) + 1; iy++)
                    {
                        for (var ix = dict.Keys.Min(k => k.x) - 1; ix <= dict.Keys.Max(k => k.x) + 1; ix++)
                        {
                            for (var iw = dict.Keys.Min(k => k.w) - 1; iw <= dict.Keys.Max(k => k.w) + 1; iw++)
                            {
                                var pos = (x: ix, y: iy, z: iz, w:iw);
                                var state = dict.GetValueOrDefault(pos, INACTIVE);

                                var countSur = CountSurroundingActive4D(dict, pos.x, pos.y, pos.z, pos.w);
                                if (state == ACTIVE && (countSur == 2 || countSur == 3))
                                {
                                    helpDict[pos] = ACTIVE;
                                }
                                else if (state == INACTIVE && countSur == 3)
                                {
                                    helpDict[pos] = ACTIVE;
                                }
                            }
                        }
                    }
                }
                dict = helpDict;
            }
            return dict.Values.Count(v => v == ACTIVE);
        }
    }
}
