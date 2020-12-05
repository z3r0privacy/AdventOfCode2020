using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day05 : AbstractDay
    {
        private protected override object CachedResult1 => 995;
        private protected override object CachedResult2 => 569;

        private List<int> _seatIds;
        private protected override void Init()
        {
            _seatIds = new List<int>();
        }

        private int BinarySearch(int from, int to, char H, string text)
        {
            var num = 0;
            var p = text.Length - 1;
            foreach (var c in text)
            {
                if (c == H)
                {
                    num += 1 << p;
                }
                p--;
            }
            return num;
        }

        private protected override object Task1()
        {
            var seats = ReadInputAsLines();
            var highest = 0;

            foreach (var seat in seats)
            {
                var row = BinarySearch(0, 127, 'B', seat[..7]);
                var col = BinarySearch(0, 7, 'R', seat[^3..]);
                var id = row * 8 + col;
                _seatIds.Add(id);
                if (id > highest)
                {
                    highest = id;
                }
            }
            return highest;
        }

        private protected override object Task2()
        {
            _seatIds = _seatIds.OrderBy(i => i).ToList();
            for (var i = 1; i < _seatIds.Count; i++)
            {
                if (_seatIds[i] - _seatIds[i-1] > 1)
                {
                    return _seatIds[i] - 1;
                }
            }
            throw new Exception("Answer not found");
        }
    }
}
