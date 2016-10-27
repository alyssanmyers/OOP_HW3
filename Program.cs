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

            var cmd = ParseArgs(args);
            DatabaseCommand.FromCommand(cmd);

           
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
