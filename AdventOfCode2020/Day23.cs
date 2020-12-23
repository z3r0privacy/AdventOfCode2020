using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day23 : AbstractDay
    {
        private protected override object CachedResult1 => 54327968;
        private protected override object CachedResult2 => 157410423276;
        internal override string DayName => "Crab Cups";


        class Node<T>
        {
            public T Value { get; set; }
            public Node<T> Prev { get; set; }
            public Node<T> Next { get; set; }
        }

        private string PlayGame(IEnumerable<int> input, int rounds, Func<Node<int>, string> resultFunc)
        {
            var low = input.Min();
            var high = input.Max();

            var links = new Node<int>[input.Count() + 1];

            var start = new Node<int> { Value = input.First() };
            links[start.Value] = start;
            var before = start;
            foreach (var i in input.Skip(1))
            {
                var n = new Node<int> { Value = i, Prev = before };
                links[i] = n;
                before.Next = n;
                before = n;
            }
            before.Next = start;
            start.Prev = before;

            var current = start;
            for (var i = 0; i < rounds; i++)
            {
                var picked = current.Next;
                current.Next = picked.Next.Next.Next;
                current.Next.Prev = current;

                picked.Prev = null;
                picked.Next.Next.Next = null;

                var destVal = current.Value;
                Node<int> dest = null;
                while (dest == null)
                {
                    destVal--;
                    if (destVal < low)
                    {
                        destVal = high;
                    }
                    if (destVal == picked.Value || destVal == picked.Next.Value || destVal == picked.Next.Next.Value)
                    {
                        continue;
                    }
                    dest = links[destVal];
                }

                dest.Next.Prev = picked.Next.Next;
                picked.Next.Next.Next = dest.Next;
                dest.Next = picked;
                picked.Prev = dest;

                current = current.Next;
            }

            return resultFunc(links[1].Next);
        }


        private protected override object Task1()
        {
            var input = ReadInputAsCharArray().Select(c => int.Parse(c.ToString()));

            return PlayGame(input, 100, n1 =>
            {
                var str = "";
                while (n1.Value != 1)
                {
                    str += n1.Value;
                    n1 = n1.Next;
                }
                return str;
            });
        }

        private protected override object Task2()
        {
            var input = ReadInputAsCharArray().Select(c => int.Parse(c.ToString())).ToList();
            input.AddRange(Enumerable.Range(10, 1000000 - 9));

            return PlayGame(input, 10000000, n1 =>
            {
                return ((long)n1.Value * (long)n1.Next.Value).ToString();
            });
        }
    }
}
