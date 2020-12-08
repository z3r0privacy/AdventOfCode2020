using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day07 : AbstractDay
    {
        internal override string DayName => "Handy Haversacks";
        private protected override object CachedResult1 => 101;
        private protected override object CachedResult2 => 108636;

        private Dictionary<string, List<(string, int)>> _dict;
        private protected override void Init()
        {
            var input = ReadInputAsLines();
            _dict = new Dictionary<string, List<(string, int)>>();

            foreach (var l in input)
            {
                var words = l.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var col = words[0] + " " + words[1];
                _dict.Add(col, new List<(string, int)>());
                if (!l.EndsWith("no other bags."))
                {
                    var pos = 1;
                    while (pos * 4 < words.Length)
                    {
                        var name = words[pos * 4 + 1] + " " + words[pos * 4 + 2];
                        var num = int.Parse(words[pos * 4]);
                        _dict[col].Add((name, num));
                        pos++;
                    }
                }
            }
        }

        private bool CanBagContain(string bag, string color)
        {
            foreach ((var col, var _) in _dict[bag])
            {
                if (col == color) return true;
                if (CanBagContain(col, color)) return true;
            }
            return false;
        }

        private protected override object Task1()
        {
            return _dict.Keys.Count(bag => CanBagContain(bag, "shiny gold"));
        }

        private int CountSubBags(string bag)
        {
            return _dict[bag].Sum(e => e.Item2 + e.Item2 * CountSubBags(e.Item1));
        }
        private protected override object Task2()
        {
            return CountSubBags("shiny gold");
        }
    }
}
