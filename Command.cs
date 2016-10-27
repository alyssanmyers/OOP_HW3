using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baseball
{
    abstract public class DatabaseCommand
    {
        public abstract void Execute(Database db);

        public static DatabaseCommand FromCommand(Command cmd)
        {
            switch (cmd.Verb.ToLower())
            {
                case "insert":
                    switch (cmd.Collection)
                    {
                        case "park": return new InsertParkCommand { ParkId = cmd.Args["ParkId"] };
                        case "team": return new InsertTeamCommand { TeamId = cmd.Args["TeamId"], League = cmd.Args["League"] };
                        case "game": return new InsertGameCommand
                        {
                            Date = DateTime.Parse(cmd.Args["Date"]),
                            HomeTeamId = cmd.Args["HomeTeamId"],
                            VisitorTeamId = cmd.Args["VisitorTeamId"],
                            HomeScore = int.Parse(cmd.Args["HomeScore"]),
                            VisitorScore = int.Parse(cmd.Args["VisitorScore"]),
                            ParkId = cmd.Args["ParkId"],
                            Attendance = int.Parse(cmd.Args["Attendance"]),
                        };
                    }
                    break;
                case "update":
                    switch (cmd.Collection)
                    {
                        case "park": return new UpdateParkCommand { ParkId = cmd.Args["ParkId"] };
                        case "team":
                            return new UpdateTeamCommand
                            {
                                TeamId = cmd.Args["TeamId"],
                                League = cmd.Args.ContainsKey("League") ? cmd.Args["League"] : null
                            };
                        case "game":
                            return new UpdateGameCommand
                            {
                                Date = DateTime.Parse(cmd.Args["Date"]),
                                HomeTeamId = cmd.Args["HomeTeamId"],
                                VisitorTeamId = cmd.Args["VisitorTeamId"],
                                HomeScore = cmd.Args.ContainsKey("HomeScore") ? int.Parse(cmd.Args["HomeScore"]) : new int?(),
                                VisitorScore = cmd.Args.ContainsKey("VisitorScore") ? int.Parse(cmd.Args["VisitorScore"]) : new int?(),
                                ParkId = cmd.Args["ParkId"],
                                Attendance = cmd.Args.ContainsKey("Attendance") ? int.Parse(cmd.Args["Attendance"]) : new int(),
                            };
                    }
                    break;
                case "delete":
                    throw new NotImplementedException();
                default:
                    break;
            }
            return null;

        }
    }


    public class InsertParkCommand : DatabaseCommand
    {
        public string ParkId { get; set; }

        public override void Execute(Database db)
        {
            if (!db.Parks.ContainsKey(ParkId))
            {
                db.Parks.Add(ParkId, new Park { Id = ParkId });
            }
        }
    }

    public class InsertTeamCommand : DatabaseCommand
    {
        public string TeamId { get; set; }
        public string League { get; set; }

        public override void Execute(Database db)
        {
            if (!db.Teams.ContainsKey(TeamId))
            {
                db.Teams.Add(TeamId, new Team { Name = TeamId, League = League.ToLeague() });
            }
        }
    }

    public class InsertGameCommand : DatabaseCommand
    {
        public DateTime Date { get; set; }
        public string HomeTeamId { get; set; }
        public string VisitorTeamId { get; set; }
        public int HomeScore { get; set; }
        public int VisitorScore { get; set; }
        public string ParkId { get; set; }
        public int Attendance { get; set; }

        public override void Execute(Database db)
        {
            // TODO: Implement me!
        }
    }



    public class UpdateParkCommand : DatabaseCommand
    {
        public string ParkId { get; set; }

        public override void Execute(Database db)
        {
            // There's nothing to update.
        }
    }

    public class UpdateTeamCommand : DatabaseCommand
    {
        public string TeamId { get; set; }
        public string League { get; set; }

        public override void Execute(Database db)
        {
            if (db.Teams.ContainsKey(TeamId) && !string.IsNullOrEmpty(League))
            {
                db.Teams[TeamId].League = League.ToLeague();
            }
        }
    }

    public class UpdateGameCommand : DatabaseCommand
    {
        public DateTime Date { get; set; }
        public string HomeTeamId { get; set; }
        public string VisitorTeamId { get; set; }
        public int? HomeScore { get; set; }
        public int? VisitorScore { get; set; }
        public string ParkId { get; set; }
        public int? Attendance { get; set; }

        public override void Execute(Database db)
        {
            // Game must match date, home and visitor team exactly
            var game = db.Games.Where(g => g.Date == Date.Date && g.Home.Name == HomeTeamId && g.Visitor.Name == VisitorTeamId).Single();

            //TODO: Implement me!
        }
    }



    /// <summary>
    /// Begins a transaction
    /// </summary>
    public class BeginCommand : DatabaseCommand
    {
        public override void Execute(Database db)
        {
            // TODO: Implement me!
        }
    }

    /// <summary>
    /// Commits a transaction
    /// </summary>
    public class CommitCommand : DatabaseCommand
    {
        public override void Execute(Database db)
        {
            // TODO: Implement me!
        }
    }

    public class RollBack : DatabaseCommand
    {
        public override void Execute(Database db)
        {
            // TODO: Implement me!
        }
    }

}
