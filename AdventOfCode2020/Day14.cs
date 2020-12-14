using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day14 : AbstractDay
    {
        private protected override object CachedResult1 => 14722016054794;
        private protected override object CachedResult2 => 3618217244644;
        internal override string DayName => "Docking Data";

        private protected override object Task1()
        {
            var input = ReadInputAsLines();
            var mem = new Dictionary<int, long>();
            var mask = "";
            foreach (var i in input)
            {
                var tkns = i.Tokenize("[]= ");
                if (tkns[0] == "mask")
                {
                    mask = tkns[1];
                } else
                {
                    var valInput = long.Parse(tkns[2]);
                    var valNew = 0L;
                    var bitSelect = 1L << 35;
                    foreach (var c in mask)
                    {
                        if (c == '1')
                        {
                            valNew += bitSelect;
                        } else if (c == 'X')
                        {
                            valNew += valInput & bitSelect;
                        }
                        bitSelect >>= 1;
                    }
                    var index = int.Parse(tkns[1]);
                    mem[index] = valNew;
                }
            }
            return mem.Sum(kvp => kvp.Value);
        }

        private protected override object Task2()
        {
            var input = ReadInputAsLines();
            var mem = new Dictionary<long, long>();
            var mask = "";
            foreach (var i in input)
            {
                var tkns = i.Tokenize("[]= ");
                if (tkns[0] == "mask")
                {
                    mask = tkns[1];
                }
                else
                {
                    var valInput = long.Parse(tkns[2]);
                    var origIdx = long.Parse(tkns[1]);
                    var newAddresses = new List<long>() { 0 };
                    var bitSelect = 1L << 35;
                    foreach (var c in mask)
                    {
                        var l = newAddresses.Count;
                        for (var a = 0; a < l; a++)
                        {
                            if (c == '1')
                            {
                                newAddresses[a] += bitSelect;
                            }
                            else if (c == 'X')
                            {
                                newAddresses.Add(newAddresses[a]);
                                newAddresses[a] += bitSelect;
                            }
                            else
                            {
                                newAddresses[a] += origIdx & bitSelect;
                            }
                        }
                        bitSelect >>= 1;
                    }
                    foreach (var a in newAddresses)
                    {
                        mem[a] = valInput;
                    }
                }
            }
            return mem.Sum(kvp => kvp.Value);
        }
    }
}
