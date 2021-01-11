using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    class Day25 : AbstractDay
    {
        private protected override object Task1()
        {
            var pkCard = 1526110;
            var pkDoor = 20175123;
            var loopSizeCard = -1;
            var loopSizeDoor = -1;
            var sn = 7;
            var mod = 20201227;

            var currVal = 1L;
            var loop = 1;
            while (loopSizeCard == -1 && loopSizeDoor == -1)
            {
                currVal *= sn;
                currVal %= mod;
                if (currVal == pkCard)
                {
                    loopSizeCard = loop;
                }
                if (currVal == pkDoor)
                {
                    loopSizeDoor = loop;
                }
                loop++;
            }

            var pk = loopSizeCard == -1 ? pkCard : pkDoor;
            var gen = loopSizeCard == -1 ? loopSizeDoor : loopSizeCard;

            var privKey = 1L;
            for (var i = 0; i < gen; i++)
            {
                privKey *= pk;
                privKey %= mod;
            }
            return privKey;
        }

        private protected override object Task2()
        {
            return "Congratz, everything solved :)";
        }
    }
}
