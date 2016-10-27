using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baseball
{
    public class Database
    {
        public IList<Game> Games { get; } = new List<Game>();
        public IDictionary<string, Park> Parks { get; } = new Dictionary<string, Park>();
        public IDictionary<string, Team> Teams { get; } = new Dictionary<string, Team>();


        public Database()
        {
        }

        public void Execute(DatabaseCommand command)
        {
            command.Execute(this);
        }

        /// <summary>
        /// Loads all the game logs from a directory.
        /// </summary>
        /// <param name="path"></param>
        public void LoadGameLogs(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path))
            {
                LoadGameLog(file);
            }
        }


        /// <summary>
        /// Parses CSV for GameLog files found here: http://www.retrosheet.org/gamelogs/index.html
        /// A description of the fields is here: http://www.retrosheet.org/gamelogs/glfields.txt
        /// </summary>
        /// <param name="file"></param>
        public void LoadGameLog(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            {
                using (var reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            var parts = line.Split(',');

                            var game = new Game();

                            // 0: Date 
                            game.Date = parts[0].RemoveQuotes().ToDate();

                            // 3-4: Visiting team, league
                            var team = parts[3].RemoveQuotes();
                            var league = parts[4].RemoveQuotes();

                            Team visitor = null;
                            if (!Teams.TryGetValue(team, out visitor))
                            {
                                visitor = new Team { Name = team, League = league.ToLeague() };
                            }
                            game.Visitor = visitor;

                            // 6-7: Homee team, league
                            team = parts[6].RemoveQuotes();
                            league = parts[7].RemoveQuotes();
                            Team home = null;
                            if (!Teams.TryGetValue(team, out home))
                            {
                                home = new Team { Name = team, League = league.ToLeague() };
                                Teams.Add(home.Name, home);
                            }
                            game.Home = home;

                            // 9-10: Score 
                            game.VisitorScore = int.Parse(parts[9]);
                            game.HomeScore = int.Parse(parts[10]);

                            // 16-17: Park id, attendance
                            Park park = null;
                            var parkId = parts[16].RemoveQuotes();
                            if (!Parks.TryGetValue(parkId, out park))
                            {
                                park = new Park { Id = parkId };
                                Parks.Add(parkId, park);
                            }
                            game.Park = park;

                            game.Attendance = int.Parse(parts[17]);

                            Games.Add(game);
                        }
                        catch
                        {
                            // ignore any parsing errors.
                        }
                    }
                }
            }
        }
    }

    internal static class Extensions
    {
        internal static string RemoveQuotes(this string str)
        {
            if (str.Length > 0)
            {
                if (str[0] == '"')
                {
                    str = str.Substring(1, str.Length - 2);
                }
            }
            return str;
        }

        internal static DateTime ToDate(this string str)
        {
            int year = int.Parse(str.Substring(0, 4));
            int month = int.Parse(str.Substring(4, 2));
            int day = int.Parse(str.Substring(6, 2));
            return new DateTime(year, month, day);
        }

        internal static League ToLeague(this string str)
        {
            League league = League.Unknown;
            Enum.TryParse<League>(str, out league);
            return league;
        }
    }
}
