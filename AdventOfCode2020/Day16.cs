using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day16 : AbstractDay
    {
        private protected override object CachedResult1 => 27911;
        private protected override object CachedResult2 => 737176602479;
        internal override string DayName => "Ticket Translation";

        private int[] _myTicket;
        private List<int[]> _foreignTickets;
        private List<(string name, Predicate<int> pred)> _rules;

        private protected override void Init()
        {
            _foreignTickets = new List<int[]>();
            _rules = new List<(string name, Predicate<int> pred)>();

            var lines = ReadInputAsLines();
            var i = 0;
            for (; !string.IsNullOrWhiteSpace(lines[i]); i++)
            {
                var rule = lines[i].Split(":");
                var predTkns = rule[1].Tokenize("- ");
                _rules.Add((rule[0],
                    num => (num >= int.Parse(predTkns[0]) && num <= int.Parse(predTkns[1]))
                    || (num >= int.Parse(predTkns[3]) && num <= int.Parse(predTkns[4]))));
            }

            i += 2;
            _myTicket = lines[i].Split(",").Select(str => int.Parse(str)).ToArray();

            i += 3;
            for (; i < lines.Length; i++)
            {
                _foreignTickets.Add(lines[i].Split(",").Select(str => int.Parse(str)).ToArray());
            }
        }

        private protected override object Task1()
        {
            var sum = 0;
            foreach (var ticket in _foreignTickets)
            {
                foreach (var value in ticket)
                {
                    if (_rules.All(r => !r.pred(value)))
                    {
                        sum += value;
                    }
                }
            }
            return sum;
        }

        private protected override object Task2()
        {
            var possibleLocs = new List<string>[_myTicket.Length];
            for (var i = 0; i < possibleLocs.Length; i++) possibleLocs[i] = new List<string>();
            var validTickets = new List<int[]>();
            
            foreach (var ticket in _foreignTickets)
            {
                if (ticket.All(value => _rules.Any(r => r.pred(value))))
                {
                    validTickets.Add(ticket);
                }
            }
            validTickets.Add(_myTicket);

            foreach ((var name, var rule) in _rules)
            {
                for (var i = 0; i < possibleLocs.Length; i++)
                {
                    if (validTickets.All(t => rule(t[i])))
                    {
                        possibleLocs[i].Add(name);
                    }
                }
            }
            var fixedNames = new List<string>();
            while (possibleLocs.Any(l => l.Count > 1))
            {
                fixedNames = possibleLocs.Where(l => l.Count == 1).Select(l => l[0]).ToList();
                for (var i = 0; i < possibleLocs.Length; i++)
                {
                    if (possibleLocs[i].Count > 1)
                    {
                        fixedNames.ForEach(n => possibleLocs[i].Remove(n));
                    }
                }
            }
            var prod = 1L;
            for (var i = 0; i < possibleLocs.Length; i++)
            {
                if (possibleLocs[i][0].StartsWith("departure"))
                {
                    prod *= _myTicket[i];
                }
            }
            return prod;
        }
    }
}
