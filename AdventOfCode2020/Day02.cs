using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day02 : AbstractDay
    {
        internal override string DayName => "Password Philosophy";
        private protected override object CachedResult1 => 460;
        private protected override object CachedResult2 => 251;

        private protected override object Task1()
        {
            return ReadInputAsLines()
                .Count(l => 
                { 
                    var ts = l.Tokenize(" :-"); 
                    var cnt = ts[3].Count(c => c == ts[2][0]); 
                    return cnt >= int.Parse(ts[0]) && cnt <= int.Parse(ts[1]); 
                });
        }

        private protected override object Task2()
        {
            return ReadInputAsLines()
                .Count(l => 
                { 
                    var ts = l.Tokenize(" :-");
                    return ts[3][int.Parse(ts[0]) - 1] == ts[2][0] ^ ts[3][int.Parse(ts[1]) - 1] == ts[2][0]; 
                });
        }
    }
}
