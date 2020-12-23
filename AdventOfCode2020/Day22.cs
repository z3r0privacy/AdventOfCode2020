using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day22 : AbstractDay
    {

        // Got help from reddit: No implementation or details, but reading the rules more exactly
        // (subgames only play with X cards, NOT all remaining)

        private protected override object CachedResult1 => 32598;
        private protected override object CachedResult2 => 35836;
        internal override string DayName => "Crab Combat";
        internal override bool IsLongRunning2 => true;
        private protected override object Task1()
        {
            var input = ReadInputAsLines();
            var player1 = new Queue<int>(input.Skip(1).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => int.Parse(l)));
            var player2 = new Queue<int>(input.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(2).Select(l => int.Parse(l)));

            while (player1.Count > 0 && player2.Count > 0)
            {
                var card1 = player1.Dequeue();
                var card2 = player2.Dequeue();
                if (card1 > card2)
                {
                    player1.Enqueue(card1);
                    player1.Enqueue(card2);
                } else
                {
                    player2.Enqueue(card2);
                    player2.Enqueue(card1);
                }
            }

            var winningDeck = player1.Count == 0 ? player2 : player1;
            var sum = 0L;
            while (winningDeck.Count > 0)
            {
                sum += winningDeck.Count * winningDeck.Dequeue();
            }
            return sum;
        }

        private int PlayGame(Queue<int> p1, Queue<int> p2)
        {
            var input = (new Queue<int>(p1), new Queue<int>(p2));
            var states = new List<(Queue<int> p1, Queue<int> p2)>();
            while (p1.Count > 0 && p2.Count > 0)
            {
                foreach ((var sp1, var sp2) in states)
                {
                    if (sp1.SequenceEqual(p1) && sp2.SequenceEqual(p2))
                    {
                        return 1;
                    }
                }
                states.Add((new Queue<int>(p1), new Queue<int>(p2)));
                var c1 = p1.Dequeue();
                var c2 = p2.Dequeue();
                int winner;
                if (p1.Count >= c1 && p2.Count >= c2)
                {
                    winner = PlayGame(new Queue<int>(p1.Take(c1)), new Queue<int>(p2.Take(c2)));
                } else
                {
                    winner = c1 > c2 ? 1 : 2;
                }
                if (winner == 1)
                {
                    p1.Enqueue(c1);
                    p1.Enqueue(c2);
                } else
                {
                    p2.Enqueue(c2);
                    p2.Enqueue(c1);
                }
            }
            var win = p1.Count == 0 ? 2 : 1;
            return win;
        }

        private protected override object Task2()
        {
            var input = ReadInputAsLines();
            var player1 = new Queue<int>(input.Skip(1).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => int.Parse(l)));
            var player2 = new Queue<int>(input.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(2).Select(l => int.Parse(l)));

            var w = PlayGame(player1, player2);

            var winningDeck = w == 1 ? player1 : player2;
            var sum = 0L;
            while (winningDeck.Count > 0)
            {
                sum += winningDeck.Count * winningDeck.Dequeue();
            }
            return sum;
        }
    }
}
