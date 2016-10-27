using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baseball
{
    public class Game
    {
        public DateTime Date { get; set; }
        public Park Park { get; set; }
        public int Attendance { get; set; }
        public Team Visitor { get; set; }
        public int VisitorScore { get; set; }
        public Team Home { get; set; }
        public int HomeScore { get; set; }
    }
}
