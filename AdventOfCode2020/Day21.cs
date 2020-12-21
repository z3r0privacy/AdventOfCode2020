using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day21 : AbstractDay
    {
        private protected override object CachedResult1 => 2614;
        private protected override object CachedResult2 => "mxmxvkd,sqjhc,fvjkl";
        internal override string DayName => "Allergen Assessment";

        private List<(string ingr, string allerg)> _matches;
        private protected override void Init()
        {
            _matches = new List<(string ingr, string allerg)>();
        }
        private protected override object Task1()
        {
            var foods = ReadInputAsLines().Select(l => new string(l.TakeWhile(c => c != '(').ToArray())).Select(l => l.Tokenize(" ")).ToList();
            var allergs = ReadInputAsLines().Select(l => new string(l.Skip(l.IndexOf("contains")+8).ToArray())).Select(l => l.Tokenize(" ,)")).ToList();

            while (allergs.Any(l => l.Any(i => !string.IsNullOrWhiteSpace(i))))
            {
                for (var i = 0; i < foods.Count; i++)
                {
                    var remAllergs = new List<string>();
                    foreach (var allerg in allergs[i])
                    {
                        var pList = foods[i].ToList();
                        for (var j = i+1; j < foods.Count && pList.Count>1; j++)
                        {
                            if (allergs[j].Contains(allerg))
                            {
                                for (var x = pList.Count-1; x >= 0; x--)
                                {
                                    if (!foods[j].Contains(pList[x]))
                                    {
                                        pList.RemoveAt(x);
                                    }
                                }
                            }
                        }
                        if (pList.Count == 1)
                        {
                            _matches.Add((pList[0], allerg));
                            remAllergs.Add(allerg);
                            foreach (var inglist in foods)
                            {
                                inglist.Remove(pList[0]);
                            }
                        }
                    }
                    foreach (var allers in allergs)
                    {
                        remAllergs.ForEach(rem => allers.Remove(rem));
                    }
                }
            }

            return foods.Sum(l => l.Count());
        }

        private protected override object Task2()
        {
            return _matches.OrderBy(m => m.allerg).Select(m => m.ingr).Aggregate((i1, i2) => i1 + "," + i2);
        }
    }
}
