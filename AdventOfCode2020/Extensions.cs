using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2020
{
    public static class Extensions
    {
        public static string Rev(this string s)
        {
            return new string(s.Reverse().ToArray());
        }
        public static List<string> Tokenize(this string s, string splitChars)
        {
            var tokens = new List<string>();
            var current = "";
            foreach (var c in s)
            {
                if (splitChars.Contains(c))
                {
                    if (!string.IsNullOrEmpty(current))
                    {
                        tokens.Add(current);
                    }
                    current = "";
                } else
                {
                    current += c;
                }
            }
            if (!string.IsNullOrEmpty(current)) tokens.Add(current);

            return tokens;
        }

        public static T GetIfPresent<K,T>(this Dictionary<K,T> dic, K key, T valueElse = default)
        {
            if (dic == null) throw new ArgumentNullException(nameof(dic));
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (dic.TryGetValue(key, out T val))
            {
                return val;
            }
            return valueElse;
        }

        public static void ForEach<T>(this IEnumerable<T> col, Action<T> action)
        {
            if (col == null) throw new ArgumentNullException(nameof(col));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var el in col)
            {
                action(el);
            }
        }

        public static IEnumerable<(T,T)> CreatePairs<T>(this IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            var i = 0;
            foreach (var el1 in list)
            {
                i++;
                foreach (var el2 in list.Skip(i))
                {
                    yield return (el1, el2);
                }
            }
        }

        public static BigInteger ToBigInteger(this long l)
        {
            return new BigInteger(l);
        }
    }
}
