using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    class Day04 : AbstractDay
    {
        private protected override object CachedResult1 => 206;
        private protected override object CachedResult2 => 123;

        private protected override object Task1()
        {
            var lines = ReadInputAsLines();
            var numValids = 0;
            var currentPass = 0;
            var fields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            
            foreach (var l in lines)
            {
                if (string.IsNullOrWhiteSpace(l))
                {
                    if (currentPass >= 7)
                    {
                        numValids++;
                    }
                    currentPass = 0;
                } else
                {
                    foreach (var word in fields)
                    {
                        if (l.Contains($"{word}:"))
                        {
                            currentPass++;
                        }
                    }
                }
            }
            if (currentPass >= 7) {
                numValids++;
            }

            return numValids;
        }

        private protected override object Task2()
        {
            var lines = ReadInputAsLines();
            var numValids = 0;
            var currentPass = 0;
            var fields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            var validiers = new Predicate<string>[]
            {
                str => int.TryParse(str, out var n) && n >= 1920 && n <= 2002,
                str => int.TryParse(str, out var n) && n >= 2010 && n <= 2020,
                str => int.TryParse(str, out var n) && n >= 2020 && n <= 2030,
                str => (str[^2..]=="cm" && int.TryParse(str[0..^2], out var n) && n >= 150 && n <= 193)
                      || (str[^2..]=="in" && int.TryParse(str[0..^2], out var n1) && n1 >= 59 && n1 <= 76),
                str => Regex.IsMatch(str, "#[0-9a-f]{6}"),
                str => new []{"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(str),
                str => str.Length == 9 && int.TryParse(str, out var _)
            };

            foreach (var l in lines)
            {
                if (string.IsNullOrWhiteSpace(l))
                {
                    if (currentPass >= 7)
                    {
                        numValids++;
                    }
                    currentPass = 0;
                }
                else
                {
                    var tkns = l.Tokenize(": ");
                    for (var i = 0; i < fields.Length; i++)
                    {
                        var idx = tkns.IndexOf(fields[i]);
                        if (idx != -1)
                        {
                            if (validiers[i](tkns[idx + 1])) {
                                currentPass++;
                            }
                        }
                    }
                }
            }
            if (currentPass >= 7) {
                numValids++;
            }

            return numValids;
        }
    }
}
