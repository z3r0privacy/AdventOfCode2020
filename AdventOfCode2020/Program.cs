using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
#if DEBUG
        private const int TAKE_SEPERATE = 1;
#else
        private const int TAKE_SEPERATE = 0;
#endif
        static void Main(string[] args)
        {
            Console.Write("..:: AdventOfCode 2020 ::..\n");

#if (DEBUG == false)
            var leaderboards = new[] { Leaderboard.PrintLeaderboard("142643"), Leaderboard.PrintLeaderboard("606817") };
#endif

            var days = typeof(Program).Assembly.GetTypes()
                .Where(t => t.Name.StartsWith("Day", StringComparison.InvariantCultureIgnoreCase)).OrderBy(d => d.Name)
                .Select(d => d.GetConstructor(Type.EmptyTypes).Invoke(null) as AbstractDay)
                .ToList();

#if (DEBUG == false)
            var runLongParts = false;
            if (days.Any(d => d.IsLongRunning1 || d.IsLongRunning2))
            {
                var ans = "";
                do
                {
                    Console.Write("Some parts may run a long time (more than 10s). Run them? [y/N]: ");
                    var input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        ans = "n";
                    } else
                    {
                        ans = char.ToLower(input[0]).ToString();
                    }
                } while (ans != "y" && ans != "n");
                if (ans == "y")
                {
                    runLongParts = true;
                }
            }
#endif

            var start = DateTime.Now;

            days.Take(days.Count - TAKE_SEPERATE).AsParallel().ForAll(d =>
              {
                  d.ExecDay(
#if DEBUG
                    true
#else 
                      false, !runLongParts
#endif
                      );
              });

            foreach (var d in days.Skip(days.Count - TAKE_SEPERATE))
            {
                d.ExecDay();
            }

            var duration = DateTime.Now - start;
            var hours = (int)duration.TotalHours;
            var timeUsed = $"{hours:D2}:{duration:mm\\:ss\\.fff}";

            foreach (var d in days)
            {
                Console.Write(d);
            }
            Console.WriteLine($"\n\n*** Time to compute in total: {timeUsed} ***");

#if (DEBUG == false)
            leaderboards.ForEach(t => Console.WriteLine(t.Result));
#else
            Console.WriteLine("\nLeaderboard is only displayed in Release mode to prevent distractions during solving");
#endif
        }
    }
}
