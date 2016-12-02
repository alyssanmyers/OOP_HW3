using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baseball
{

    public class Command
    {
        /// <summary>
        /// Insert, Update, Delete,
        /// </summary>
        public string Verb { get; set; }
        /// <summary>
        /// Park, Team, Game
        /// </summary>
        public string Collection { get; set; }

        /// <summary>
        /// Data associated with the command
        /// </summary>
        public Dictionary<string, string> Args { get; set; } = new Dictionary<string, string>();
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new Database();

            db.Parks["45778"] = new Park { Id= "45778" };
            db.Parks["90438"] = new Park { Id = "90438" };
            db.Teams["8007"] = new Team { Name = "8007", League = League.NA };
            db.Teams["9234"] = new Team { Name = "9234", League = League.NA };
            var tempGame = new Game { Date = DateTime.Now, Park = db.Parks["45778"], Attendance = 500,
            Visitor = db.Teams["8007"], VisitorScore = 2, Home = db.Teams["9234"], HomeScore = 5 };
            db.Games.Add(tempGame);
            
            /*********************/
            /* adding commands  */
            /*********************/

            var cmd = ParseArgs(args);
            if (cmd.Verb == "select")
            {
                var dbFlt = Filter.FromCommand(cmd);
                db.Execute(dbFlt);
            }
            else
            {
                var dbCmd = DatabaseCommand.FromCommand(cmd);
                db.Commands.Add(dbCmd);
                dbCmd.Execute(db);
            }

            /* mine */

            args = new string[] { "insert", "park", "ParkId", "98765" };
            cmd = ParseArgs(args);
            db.Commands.Add(DatabaseCommand.FromCommand(cmd));

            args = new string[] { "insert", "team", "TeamId", "1000", "League", "NA" };
            cmd = ParseArgs(args);
            db.Commands.Add(DatabaseCommand.FromCommand(cmd));

            args = new string[] { "commit", "commit" };
            cmd = ParseArgs(args);
            db.Commands.Add(DatabaseCommand.FromCommand(cmd));
            db.Commands.Last().Execute(db);

            /**********************************/
            /* printing out database elements */
            /**********************************/

            foreach (var c in db.Parks)
            {
                Console.WriteLine(c.Key + ": " + c.Value);
            }
            foreach (var c in db.Teams)
            {
                Console.WriteLine(c.Key + ": " + c.Value.Name + " " + c.Value.League);
            }
            foreach (var c in db.Games)
            {
                Console.WriteLine(c.Date + " | " + c.Home.Name + " vs " + c.Visitor.Name + "\n" 
                + c.HomeScore + ":" + c.VisitorScore + " with "
                + c.Attendance + " fans at " + c.Park.Id);
            }
        }

        private static Command ParseArgs(string[] args)
        {
            var cmd = new Command { };
            cmd.Verb = args[0];
            cmd.Collection = args[1];

            for (int i = 2; i < args.Length; i++)
            {
                var key = args[i++];
                var value = args[i];
                cmd.Args.Add(key, value);
            }

            return cmd;
        }

    }

    

}
