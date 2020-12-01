using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class ResultData
    {
        public TimeSpan Task1Time { get; set; }
        public TimeSpan Task2Time { get; set; }
        public TimeSpan TotalTime => Task1Time + Task2Time;
        public string ResultT1 { get; set; }
        public string ResultT2 { get; set; }
        public int Day { get; set; }

        private string Duration(TimeSpan t)
        {
            var hours = (int)t.TotalHours;
            return $"{hours:D2}:{t:mm\\:ss\\.fff}";
        }

        public override string ToString()
        {
            var format =@"
################################
Day {0} (Total time: {1})
Task1 (Time: {2}): {3}
Task2 (Time: {4}): {5}
";

            return string.Format(format, Day, Duration(TotalTime), Duration(Task1Time), ResultT1, Duration(Task2Time), ResultT2);
        }
    }

    abstract class AbstractDay
    {
        public delegate bool TryParse<T>(string input, out T num);

        internal int Day { get; set; }

        private protected string _file;
        private string content = null;
        private string[] contentLines = null;

        private protected virtual  object CachedResult1 { get; }
        private protected virtual object CachedResult2 { get; }

        internal virtual bool IsLongRunning1 => false;
        internal virtual bool IsLongRunning2 => false;

        public AbstractDay()
        {
            var file = GetType().Name;
            _file = Path.Combine(Environment.CurrentDirectory, "Inputs", file + ".txt");
            if (int.TryParse(file.Substring(3), out var day))
            {
                Day = day;
            }
            else
            {
                Day = -1;
            }
        }

        public AbstractDay(string file) : this()
        {
            _file = file;
        }

        private protected virtual void Init() { }

        private protected virtual void PrepareTask1() { }
        private protected virtual void PrepareTask2() { }

        private string Int_Task1(bool useCached, bool skipLongRunners)
        {
            if (useCached || (IsLongRunning1 && skipLongRunners))
            {
                if (CachedResult1 != null) {
                    return "[Cached] " + CachedResult1.ToString();
                }
            }
            try
            {
                return Task1().ToString();
            }
            catch (NotImplementedException)
            {
                return "not yet implemented";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string Int_Task2(bool useCached, bool skipLongRunners)
        {
            if (useCached || (IsLongRunning2 && skipLongRunners))
            {
                if (CachedResult2 != null)
                {
                    return "[Cached] " + CachedResult2.ToString();
                }
            }
            try
            {
                return Task2().ToString();
            } catch (NotImplementedException)
            {
                return "not yet implemented";
            } catch(Exception e)
            {
                return e.Message;
            }
            
        }

        private protected abstract object Task1();
        private protected abstract object Task2();

        private protected void EnableSecondContentFile()
        {
            _file += "2";
            content = null;
        }
        
        private protected string ReadInput ()
        {
            if (content == null)
            {
                content = File.ReadAllText(_file);
            }
            return content;
        }

        private protected T ReadInputAsType<T>(TryParse<T> parse)
        {
            var content = ReadInput();
            if (parse(content, out var res))
            {
                return res;
            }
            throw new ArgumentException($"Could not parse {content} as {typeof(T).Name}");
        }

        private protected string[] ReadInputSplittedBy(string delimiter)
        {
            return ReadInput().Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
        }

        private protected T[] ReadInputSplittedBy<T>(string delimiter, TryParse<T> parser)
        {
            return ReadInputSplittedBy(delimiter).Select(s =>
            {
                if (parser(s, out var res))
                {
                    return res;
                }
                throw new ArgumentException($"Could not parse {content} as {typeof(T).Name}");
            }).ToArray();
        }

        private protected string[] ReadInputAsLines()
        {
            if (contentLines == null)
            {
                contentLines = File.ReadAllLines(_file);
            }

            // probably not necessary in >90% of the cases, but ensures compatibility for the rest
            // this little overhead is ok in order to avoid side effects
            // and is still faster than reading the file again
            var input = new string[contentLines.Length];
            Array.Copy(contentLines, input, contentLines.Length);
            return input;
        }

        private protected T[] ReadInputAsLines<T>(TryParse<T> parser)
        {
            return ReadInputAsLines().Select(s =>
            {
                if (parser(s, out var t))
                {
                    return t;
                }
                return default;
            }).ToArray();
        }

        private protected char[] ReadInputAsCharArray()
        {
            return ReadInput().ToCharArray();
        }

        internal ResultData ExecDay(bool useCached = false, bool skipLongRunners = false)
        {
            var start = DateTime.Now;
            if (!(useCached && CachedResult1 != null && CachedResult2 != null))
            {
                Init();
            }
            if (!(useCached && CachedResult1 != null))
            {
                PrepareTask1();
            }
            var r1 = Int_Task1(useCached, skipLongRunners);
            var afterT1 = DateTime.Now;
            if (!(useCached && CachedResult2 != null))
            {
                PrepareTask2();
            }
            var r2 = Int_Task2(useCached, skipLongRunners);
            var end = DateTime.Now;
            return new ResultData
            {
                Day = Day,
                ResultT1 = r1,
                ResultT2 = r2,
                Task1Time = afterT1 - start,
                Task2Time = end - afterT1,
            };
        }
    }
}
