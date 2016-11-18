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

/*

transaction: revert commands within scope of trans, store? place to store it. in database?
insert, call execute to database, no transaction they continue to execute

in begin command, this command should tell databse to remember commands, list of command,
rollback command it should erase up to last transaction. in db internal list of commands.
first time hit trans, 

*/
    

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
            
            var cmd = ParseArgs(args);
            DatabaseCommand cm = DatabaseCommand.FromCommand(cmd);
            
            cm.Execute(db);

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
