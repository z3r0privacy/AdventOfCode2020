using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day18 : AbstractDay
    {

        private protected override object CachedResult1 => 209335026987;
        private protected override object CachedResult2 => 33331817392479;
        internal override string DayName => "Operation Order";

        private long CalcSubExpression(List<string> tokens, ref int index)
        {
            var sum = 0L;
            Func<long,long,long> op = null;
            for (; index < tokens.Count; index++)
            {
                switch (tokens[index])
                {
                    case "+":
                        op = (i1, i2) => i1 + i2;
                        break;
                    case "*":
                        op = (i1, i2) => i1 * i2;
                        break;
                    case ")":
                        return sum;
                    case "(":
                        index++;
                        var sNum = CalcSubExpression(tokens, ref index);
                        if (op != null)
                        {
                            sum = op(sum, sNum);
                            op = null;
                        } else
                        {
                            sum = sNum;
                        }
                        break;
                    default:
                        var num = long.Parse(tokens[index]);
                        if (op != null)
                        {
                            sum = op(sum, num);
                            op = null;
                        }
                        else
                        {
                            sum = num;
                        }
                        break;
                }
            }
            return sum;
        }

        private protected override object Task1()
        {
            var input = ReadInputAsLines();

            var sum = 0L;
            foreach (var m in input)
            {
                var tokens = m.Replace("(", "( ").Replace(")", " )").Tokenize(" ");
                var pos = 0;
                var res = CalcSubExpression(tokens, ref pos);
                sum += res;
            }
            return sum;
        }

        private long CalcSubExpression2(List<string> tokens, ref int index)
        {
            var nums = new List<long>();
            var ops = new List<char>();
            var finished = false;
            for (; index < tokens.Count && !finished; index++)
            {
                switch (tokens[index])
                {
                    case "+":
                        ops.Add('+');
                        break;
                    case "*":
                        ops.Add('*');
                        break;
                    case ")":
                        finished = true;
                        index--;
                        break;
                    case "(":
                        index++;
                        var sNum = CalcSubExpression2(tokens, ref index);
                        nums.Add(sNum);
                        break;
                    default:
                        var num = long.Parse(tokens[index]);
                        nums.Add(num);
                        break;
                }
            }

            var remainings = new List<long>();
            var accu = nums.Last();
            for (var i = ops.Count-1; i >= 0; i--)
            {
                if (ops[i] == '+')
                {
                    accu += nums[i];
                } else
                {
                    remainings.Add(accu);
                    accu = nums[i];
                }
            }
            remainings.Add(accu);
            return remainings.Aggregate((l1, l2) => l1 * l2);
        }

        private protected override object Task2()
        {
            var input = ReadInputAsLines();

            var sum = 0L;
            foreach (var m in input)
            {
                var tokens = m.Replace("(", "( ").Replace(")", " )").Tokenize(" ");
                var pos = 0;
                var res = CalcSubExpression2(tokens, ref pos);
                sum += res;
            }
            return sum;
        }
    }
}
