using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class JSONData
    {
        public string owner_id { get; set; }
        public string @event { get; set; }
        public Dictionary<string, Member> members { get; set; }
        public long download_time { get; set; }
        public string owner => members[owner_id].name;
    }
    class Member
    {
        public int local_score { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int stars { get; set; }
        public string last_star_ts { get; set; }
        public int global_score { get; set; }
        public Dictionary<string, Dictionary<string, PartCompletion>> completion_day_level { get; set; }
    }
    class PartCompletion
    {
        public string get_star_ts { get; set; }
    }


    class Leaderboard
    {
        private static async Task DownloadLeaderboard(string id)
        {
            //Console.WriteLine("Downloading leaderboard " + id);
            var url = $"https://adventofcode.com/2020/leaderboard/private/view/{id}.json";
            
            using var wc = new WebClient();
            var cookie = File.ReadAllText(Path.Combine("NoGit", "cookie.txt"));
            wc.Headers.Add(HttpRequestHeader.Cookie, $"session={cookie}");
            var json = await wc.DownloadStringTaskAsync(url);
            json = json.Substring(0, json.Length - 1) + ",\"download_time\":" + DateTime.Now.Ticks + "}";
            File.WriteAllText(Path.Combine("NoGit", id + ".json"), json);
        }

        private static string UnixTimeStampToString(string timeStamp, int dayNr)
        {
            if (timeStamp == null) return "";
            var ts = int.Parse(timeStamp);
            var localEndDateTime = new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc).AddSeconds(ts).ToLocalTime();
            var localStartDateTime = new DateTime(2020, 12, dayNr, 6, 0, 0, DateTimeKind.Local);
            var duration = localEndDateTime - localStartDateTime;
            var hours = (int)duration.TotalHours;
            return $"{hours:D2}:{duration:mm\\:ss}";
        }

        private static string CreateParticipantsString(JSONData data)
        {
            var maxLen = 48;
            var memberLines = new List<string>();
            var title = "Participants: ";
            var formatString = "{0," + title.Length + "}";
            var currentString = string.Format(formatString, title);
            var notFirst = false;
            foreach (var m in data.members.OrderBy(x => x.Value.name))
            {
                if (currentString.Length + m.Value.name.Length + 3 > maxLen)
                {
                    memberLines.Add(currentString + ",");
                    currentString = string.Format(formatString, "");
                    currentString += m.Value.name;
                } else
                {
                    if (notFirst)
                    {
                        currentString += ", ";
                    }
                    currentString += m.Value.name;
                    notFirst = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(currentString)) memberLines.Add(currentString);
            return memberLines.Aggregate((s1, s2) => s1 + "\n" + s2);
        }

        public static async Task<string> PrintLeaderboard(string id)
        {
            var sb = new StringBuilder();

            var file = Path.Combine("NoGit", id + ".json");
            if (!File.Exists(file))
            {
                await DownloadLeaderboard(id);
            }
            var board = JsonConvert.DeserializeObject<JSONData>(File.ReadAllText(file));

            if (new TimeSpan(DateTime.Now.Ticks - board.download_time).TotalSeconds > 900)
            {
                await DownloadLeaderboard(id);
                board = JsonConvert.DeserializeObject<JSONData>(File.ReadAllText(file));
            }


            var maxDay = board.members.Where(kvp => kvp.Value.completion_day_level.Count > 0).Max(kvp => kvp.Value.completion_day_level.Max(kvp2 => int.Parse(kvp2.Key)));

            var formatStringH = "{0,4}  {1,-17}{2,-14}{3,-14}";
            var formatString = "{0,3}.  {1,-17}{2,-14}{3,-14}";

            sb.AppendLine("\n\n");
            sb.AppendLine("################################################");
            sb.AppendLine("################## Leaderboard #################");
            sb.AppendLine(string.Format("################# {0,-13} ################",
                string.Format("{0," + ((13 + board.owner.Length) / 2).ToString() + "}", board.owner)));
            sb.AppendLine(string.Format("################# {0,-13} ################",
                string.Format("{0," + ((13 + board.owner_id.Length) / 2).ToString() + "}", board.owner_id)));
            sb.AppendLine("################################################");


            sb.AppendLine(CreateParticipantsString(board));


            sb.AppendLine();

            for (var i = maxDay; i >= 1; i--)
            {
                sb.AppendLine($"==================== Day {i:D2} ====================");
                sb.AppendLine(string.Format(formatStringH, "Rank", "User", "First star", "Second Star"));

                var participatingMembers = board.members.Where(m => m.Value.completion_day_level.ContainsKey(i.ToString()))
                    .OrderBy(m => m.Value.completion_day_level[i.ToString()].ContainsKey("2") ? 0 : 1)
                    .ThenBy(m => int.Parse(m.Value.completion_day_level[i.ToString()].GetIfPresent("2")?.get_star_ts ?? "0"))
                    .ThenBy(m => int.Parse(m.Value.completion_day_level[i.ToString()]["1"].get_star_ts))
                    .Select(m => (m.Value.name,
                        firstStar: UnixTimeStampToString(m.Value.completion_day_level[i.ToString()]["1"].get_star_ts, i),
                        secondStar: UnixTimeStampToString(m.Value.completion_day_level[i.ToString()].GetIfPresent("2")?.get_star_ts, i)))
                    .ToList();

                for (var p = 0; p < participatingMembers.Count(); p++)
                {
                    sb.AppendLine(string.Format(formatString, p + 1, participatingMembers[p].name, participatingMembers[p].firstStar, participatingMembers[p].secondStar));
                }

                sb.AppendLine();

            }
            return sb.ToString();
        }
    }
}
