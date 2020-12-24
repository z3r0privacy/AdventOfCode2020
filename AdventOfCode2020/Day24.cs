using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day24 : AbstractDay
    {
        private protected override object CachedResult1 => 282;
        private protected override object CachedResult2 => 3445;
        internal override string DayName => "Lobby Layout";

        private Dictionary<(int x, int y), bool> _blacks;

        private protected override object Task1()
        {
            var input = ReadInputAsLines();
            _blacks = new Dictionary<(int x, int y), bool>();

            foreach (var l in input)
            {
                var x = 0;
                var y = 0;
                for (var i = 0; i < l.Length; i++)
                {
                    var str = l[i].ToString();
                    if (str == "s" || str == "n")
                    {
                        i++;
                        str += l[i];
                    }
                    switch (str)
                    {
                        case "ne":
                            x += 1;
                            y += 1;
                            break;
                        case "e":
                            x += 2;
                            break;
                        case "se":
                            x += 1;
                            y -= 1;
                            break;
                        case "sw":
                            x -= 1;
                            y -= 1;
                            break;
                        case "w":
                            x -= 2;
                            break;
                        case "nw":
                            x -= 1;
                            y += 1;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                if (_blacks.ContainsKey((x,y))) {
                    _blacks.Remove((x, y));
                } else
                {
                    _blacks[(x, y)] = true;
                }
            }
            return _blacks.Count(kvp => kvp.Value);
        }

        private int CountSurroundingBlacks(int x, int y, Dictionary<(int x, int y), bool> blacks)
        {
            var tx = new[] { 1, 2, 1, -1, -2, -1 };
            var ty = new[] { 1, 0, -1, -1, 0, 1 };

            var count = 0;
            for (var i = 0; i < tx.Length; i++)
            {
                if (blacks.ContainsKey((x+tx[i], y+ty[i])))
                {
                    count++;
                }
            }
            return count;
        }
        private protected override object Task2()
        {
            for (var i = 0; i < 100; i++)
            {
                var newBlacks = new Dictionary<(int x, int y), bool>();
                for (var y = _blacks.Keys.Min(v => v.y)-1; y <= _blacks.Keys.Max(v => v.y) + 1; y++)
                {
                    var x = _blacks.Keys.Min(v => v.x) - 2;
                    var unevenX = 0;
                    if (!((y&1) == (x&1)))
                    {
                        x--;
                        unevenX = 1;
                    }
                    for (; x <= _blacks.Keys.Max(v => v.x) + 2+unevenX; x += 2)
                    {
                        var cnt = CountSurroundingBlacks(x, y, _blacks);
                        if (_blacks.ContainsKey((x,y)))
                        {
                            if (cnt == 1 || cnt == 2)
                            {
                                newBlacks[(x, y)] = true;
                            }
                        } else
                        {
                            if (cnt == 2)
                            {
                                newBlacks[(x, y)] = true;
                            }
                        }
                    }
                }
                _blacks = newBlacks;
            }
            return _blacks.Count;
        }
    }
}
