using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    class Day12 : AbstractDay
    {
        private protected override object CachedResult1 => 879;
        private protected override object CachedResult2 => 18107;
        internal override string DayName => "Rain Risk";
        private protected override object Task1()
        {
            var instrs = ReadInputAsLines();
            // N, E, S, W
            var xDir = new[] { 0, 1, 0, -1 };
            var yDir = new[] { -1, 0, 1, 0 };
            var dir = 1;

            var posX = 0;
            var posY = 0;

            foreach (var i in instrs)
            {
                var c = i[0];
                var num = int.Parse(i[1..]);
                switch (c)
                {
                    case 'N':
                        posY -= num;
                        break;
                    case 'E':
                        posX += num;
                        break;
                    case 'S':
                        posY += num;
                        break;
                    case 'W':
                        posX -= num;
                        break;
                    case 'L':
                        dir = (((dir - num / 90) % 4) + 4) % 4;
                        break;
                    case 'R':
                        dir = (dir + num / 90) % 4;
                        break;
                    case 'F':
                        posX += num * xDir[dir];
                        posY += num * yDir[dir];
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            return Math.Abs(posX) + Math.Abs(posY);
        }

        private protected override object Task2()
        {
            var instrs = ReadInputAsLines();

            var rotL = new[] { new[] { 0, -1 }, new[] { 1, 0 } };
            var rotR = new[] { new[] { 0, 1 }, new[] { -1, 0 } };

            var shX = 0;
            var shY = 0;
            var wpX = 10;
            var wpY = -1;

            foreach (var i in instrs)
            {
                var c = i[0];
                var num = int.Parse(i[1..]);
                //var diffX = wpX - shX;
                //var diffY = wpY - shY;
                var diffX2 = 0;
                var diffY2 = 0;
                switch (c)
                {
                    case 'N':
                        wpY -= num;
                        break;
                    case 'E':
                        wpX += num;
                        break;
                    case 'S':
                        wpY += num;
                        break;
                    case 'W':
                        wpX -= num;
                        break;
                    case 'L':
                        var numL = num / 90;
                        for (var y = 0; y < numL; y++)
                        {
                            diffX2 = wpX * rotL[0][0] + wpY * rotL[1][0];
                            diffY2 = wpX * rotL[0][1] + wpY * rotL[1][1];
                            wpX = diffX2;
                            wpY = diffY2;
                        }
                        break;
                    case 'R':
                        var numR = num / 90;
                        for (var y = 0; y < numR; y++)
                        {
                            diffX2 = wpX * rotR[0][0] + wpY * rotR[1][0];
                            diffY2 = wpX * rotR[0][1] + wpY * rotR[1][1];
                            wpX = diffX2;
                            wpY = diffY2;
                        }
                        break;
                    case 'F':
                        shX += num * wpX;
                        shY += num * wpY;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            return Math.Abs(shX) + Math.Abs(shY);
        }
    }
}
