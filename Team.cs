using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baseball
{
    public enum League
    {
        /// <summary>
        /// American League
        /// </summary>
        AL,
        /// <summary>
        /// National League
        /// </summary>
        NL,
        /// <summary>
        /// National Association
        /// </summary>
        NA,
        /// <summary>
        /// American Association
        /// </summary>
        AA,
        /// <summary>
        /// Union Association
        /// </summary>
        UA,
        /// <summary>
        /// Players League
        /// </summary>
        PL,
        /// <summary>
        /// Federal League
        /// </summary>
        FL,

        Unknown,
    }

    public class Park
    {
        public string Id { get; set; }
    }


    public class Team
    {
        public League League { get; set; }
        public string Name { get; set; }
    }



}
