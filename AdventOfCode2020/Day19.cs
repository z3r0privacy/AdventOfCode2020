using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    class Day19 : AbstractDay
    {
        private protected override object CachedResult1 => 279;
        private protected override object CachedResult2 => 384;
        internal override string DayName => "Monster Messages";

        private int depth = 0;
        
        private string GetRegex(int rule, Dictionary<int, string[]> source)
        {
            depth++;
            var str = "(";
            foreach (var el in source[rule])
            {
                if (int.TryParse(el, out var num))
                {
                    if (depth <= 15)
                    {
                        str += GetRegex(num, source);
                    }
                } else if (el == "|")
                {
                    str += "|";
                } else
                {
                    str += el.Replace("\"", "");
                }
            }
            str += ")";
            depth--;
            return str;
        }
        private protected override object Task1()
        {
            var rules = ReadInputAsLines().TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Tokenize(": ")).ToDictionary(l => int.Parse(l[0]), l => l.ToArray()[1..].ToArray());
            var inputs = ReadInputAsLines().Skip(rules.Count + 1);
            var rgx = "^" + GetRegex(0, rules) + "$";
            var regex = new Regex(rgx);
            return inputs.Count(s => regex.IsMatch(s));
        }

        private protected override object Task2()
        {
            /*
             * Changed Input
             * 8: 42 | 42 8
             * 11: 42 31 | 42 11 31
            */

            var rules = ReadInputAsLines().TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Tokenize(": ")).ToDictionary(l => int.Parse(l[0]), l => l.ToArray()[1..].ToArray());
            var inputs = ReadInputAsLines().Skip(rules.Count + 1);

            rules[8] = new[] { "42", "|", "42", "8" };
            rules[11] = new[] { "42", "31", "|", "42", "11", "31" };

            var rgx = "^" + GetRegex(0, rules) + "$";
            var regex = new Regex(rgx);
            return inputs.Count(s => regex.IsMatch(s));
        }
    }
}
