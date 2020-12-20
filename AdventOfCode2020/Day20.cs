using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day20 : AbstractDay
    {
        private protected override object CachedResult1 => 8425574315321;
        private protected override object CachedResult2 => 1841;
        internal override string DayName => "Jurassic Jigsaw";

        class Tile
        {
            public char[][] Data;
            public long ID;

            public int HasMatchingBorder(Tile t)
            {
                var ot = new string(t.Data[0]).Rev();
                var rt = new string(t.Data.Select(r => r[9]).ToArray()).Rev();
                var dt = new string(t.Data[9].Reverse().ToArray()).Rev();
                var lt = new string(t.Data.Select(r => r[0]).Reverse().ToArray()).Rev();

                var om = new string(Data[0]);
                var rm = new string(Data.Select(r => r[9]).ToArray());
                var dm = new string(Data[9].Reverse().ToArray());
                var lm = new string(Data.Select(r => r[0]).Reverse().ToArray());

                if (om == ot) return 0;
                if (om == rt) return 1;
                if (om == dt) return 2;
                if (om == lt) return 3;

                if (rm == ot) return 4;
                if (rm == rt) return 5;
                if (rm == dt) return 6;
                if (rm == lt) return 7;

                if (dm == ot) return 8;
                if (dm == rt) return 9;
                if (dm == dt) return 10;
                if (dm == lt) return 11;

                if (lm == ot) return 12;
                if (lm == rt) return 13;
                if (lm == dt) return 14;
                if (lm == lt) return 15;

                return -1;
            }

            public static char[][] RotateRightOnce(char[][] original) //, char[][] target)
            {
                var target = new char[original.Length][];
                for (var i = 0; i < target.Length; i++)
                {
                    target[i] = new char[original[i].Length];
                }
                for (var i = 0; i < original.Length; i++)
                {
                    for (var j = 0; j < original[i].Length; j++)
                    {
                        target[j][original[i].Length-1 - i] = original[i][j];
                    }
                }
                return target;
            }
            public Tile RotateRight(int times)
            {
                var orig = Data;
                for (var i = 0; i < times; i++)
                {
                    var target = RotateRightOnce(orig);
                    orig = target;
                }
                return new Tile { Data = orig, ID = ID };
            }

            public static char[][] FlipArray(bool hor, char[][] orig) //, char[][] target)
            {
                var target = new char[orig.Length][];
                for (var i = 0; i < target.Length; i++)
                {
                    target[i] = new char[orig[i].Length];
                }
                for (var y = 0; y < orig.Length; y++)
                {
                    for (var x = 0; x < orig[y].Length; x++)
                    {
                        var tx = hor ? orig[y].Length-1 - x : x;
                        var ty = hor ? y : orig.Length -1 - y;
                        target[ty][tx] = orig[y][x];
                    }
                }
                return target;
            }

            public Tile Flip(bool hor)
            {
                var target = FlipArray(hor, Data);

                return new Tile { Data = target, ID = ID };
            }
        }

        private List<Tile> _tiles;
        private Dictionary<(int x, int y), Tile> _image;

        private protected override void Init()
        {
            var input = ReadInputAsLines();
            var tiles = new List<Tile>();
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i].StartsWith("Tile"))
                {
                    var t = new Tile();
                    t.Data = input.Skip(i + 1).Take(10).Select(s => s.ToArray()).ToArray();
                    t.ID = long.Parse(input[i][5..9]);
                    i += 11;
                    tiles.Add(t);
                }
            }
            _tiles = tiles.ToList();
        }

        private void PrintMap(Dictionary<(int x, int y), Tile> map)
        {
            var minX = map.Keys.Min(k => k.x);
            var minY = map.Keys.Min(k => k.y);
            var maxX = map.Keys.Max(k => k.x);
            var maxY = map.Keys.Max(k => k.y);

            for (var y = 0; y < (maxY - minY) * 10 + 10; y++)
            {
                if (y % 10 == 0 && y != minY * 10) Console.WriteLine();

                for (var x = 0; x < (maxX - minX) * 10 + 10; x++)
                {
                    if (x % 10 == 0 && x != minX * 10) Console.Write("  ");
                    var tx = x / 10;
                    tx += minX;
                    var ty = y / 10;
                    ty += minY;
                    var ax = ((x % 10) + 10) % 10;
                    var ay = ((y % 10) + 10) % 10;
                    if (map.ContainsKey((tx, ty)))
                    {
                        Console.Write(map[(tx, ty)].Data[ay][ax]);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private (int newX, int newY, Tile newT) TransformTile((int x, int y) refPos, Tile currT, int side)
        {
            return side switch
            {
                0 => (refPos.x, refPos.y - 1, currT.RotateRight(2)),
                1 => (refPos.x, refPos.y - 1, currT.RotateRight(1)),
                2 => (refPos.x, refPos.y - 1, currT),
                3 => (refPos.x, refPos.y - 1, currT.RotateRight(3)),
                4 => (refPos.x + 1, refPos.y, currT.RotateRight(3)),
                5 => (refPos.x + 1, refPos.y, currT.RotateRight(2)),
                6 => (refPos.x + 1, refPos.y, currT.RotateRight(1)),
                7 => (refPos.x + 1, refPos.y, currT),
                8 => (refPos.x, refPos.y + 1, currT),
                9 => (refPos.x, refPos.y + 1, currT.RotateRight(3)),
                10 => (refPos.x, refPos.y + 1, currT.RotateRight(2)),
                11 => (refPos.x, refPos.y + 1, currT.RotateRight(1)),
                12 => (refPos.x - 1, refPos.y, currT.RotateRight(1)),
                13 => (refPos.x - 1, refPos.y, currT),
                14 => (refPos.x - 1, refPos.y, currT.RotateRight(3)),
                15 => (refPos.x - 1, refPos.y, currT.RotateRight(2)),
                16 => (refPos.x, refPos.y - 1, currT.Flip(false)),
                17 => (refPos.x + 1, refPos.y, currT.Flip(true)),
                18 => (refPos.x, refPos.y + 1, currT.Flip(false)),
                19 => (refPos.x - 1, refPos.y, currT.Flip(true)),
                _ => throw new InvalidOperationException(),
            };
        }

        private protected override object Task1()
        {
            var map = new Dictionary<(int x, int y), Tile>();
            var myList = _tiles.ToList();
            map[(0, 0)] = myList[1];
            myList.RemoveAt(1);

            while (myList.Count > 0)
            {
                Tile currT = null;
                (int x, int y) refPos = (0,0);
                int side = -1;
                var origIdx = -1;
                for (var i = 0; i < myList.Count && currT == null; i++)
                {
                    foreach (var kvp in map)
                    {
                        var testT = myList[i];
                        var res = kvp.Value.HasMatchingBorder(testT);
                        if (res == -1)
                        {
                            testT = testT.Flip(false);
                            res = kvp.Value.HasMatchingBorder(testT);
                        }
                        if (res == -1)
                        {
                            testT = testT.Flip(false).Flip(true);
                            res = kvp.Value.HasMatchingBorder(testT);
                        }
                        if (res != -1)
                        {
                            currT = testT;
                            refPos = kvp.Key;
                            side = res;
                            origIdx = i;
                            break;
                        }
                    }
                }
                if (currT == null)
                {
                    throw new InvalidOperationException();
                }
                (int newX, int newY, Tile newT) = TransformTile(refPos, currT, side);
                if (map.ContainsKey((newX, newY)))
                {
                    throw new InvalidOperationException();
                }
                map[(newX, newY)] = newT;
                myList.RemoveAt(origIdx);
            }

            _image = map;

            var minX = map.Keys.Min(k => k.x);
            var minY = map.Keys.Min(k => k.y);
            var maxX = map.Keys.Max(k => k.x);
            var maxY = map.Keys.Max(k => k.y);
            return map[(minX, minY)].ID * map[(maxX, minY)].ID * map[(minX, maxY)].ID * map[(maxX, maxY)].ID;
        }


        private int CountMonsters(char[][] data)
        {
            var offX = new[] { 1, 4, 5, 6, 7, 10, 11, 12, 13, 16, 17, 18, 18, 19 };
            var offY = new[] { 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, -1, 0 };
            var count = 0;

            for (var y = 0; y < data.Length; y++)
            {
                for (var x = 0; x < data[y].Length; x++)
                {
                    if (data[y][x] == '#')
                    {
                        if (Enumerable.Range(0,14).All(i =>
                        {
                            var _y = y + offY[i];
                            var _x = x + offX[i];
                            return _y >= 0 && _y < data.Length && _x >= 0 && _x < data[_y].Length && data[_y][_x] == '#';
                        }))
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private protected override object Task2()
        {
            var minX = _image.Keys.Min(k => k.x);
            var minY = _image.Keys.Min(k => k.y);
            var maxX = _image.Keys.Max(k => k.x);
            var maxY = _image.Keys.Max(k => k.y);

            var height = (maxY - minY+1) * 8;
            var width = (maxX - minX+1) * 8;
            var currX = 0;
            var currY = 0;

            var image = new char[height][];
            for (var i = 0; i< height; i++)
            {
                image[i] = new char[width];
            }

            for (var y = 0; y < (maxY - minY) * 10 + 10; y++)
            {
                var ty = y / 10;
                ty += minY;
                var ay = ((y % 10) + 10) % 10;
                if (ay != 0 && ay != 9)
                {
                    for (var x = 0; x < (maxX - minX) * 10 + 10; x++)
                    {
                        var tx = x / 10;
                        tx += minX;
                        var ax = ((x % 10) + 10) % 10;

                        if (ax != 0 && ax != 9)
                        {
                            image[currY][currX] = _image[(tx, ty)].Data[ay][ax];
                            currX++;
                        }
                    }
                    currY++;
                    currX = 0;
                }
            }

            var res = -1;
            for (var i = 0; i < 4; i++)
            {
                var cnt = CountMonsters(image);
                if (cnt > 0)
                {
                    res = cnt;
                    break;
                }
                image = Tile.FlipArray(false, image);
                cnt = CountMonsters(image);
                if (cnt > 0)
                {
                    res = cnt;
                    break;
                }
                image = Tile.FlipArray(true, Tile.FlipArray(false, image));
                cnt = CountMonsters(image);
                if (cnt > 0)
                {
                    res = cnt;
                    break;
                }

                image = Tile.FlipArray(true, image);
                image = Tile.RotateRightOnce(image);
            }

            if (res == -1)
            {
                throw new InvalidOperationException();
            }

            return image.Sum(l => l.Count(c => c == '#')) - res * 15;
        }
    }
}
