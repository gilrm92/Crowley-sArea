using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersHost.Entitys
{
    public class Match
    {
        public Match()
        {
            Guid idMatch = new Guid();
            IdMatch = idMatch.ToString();

            Players = new List<Player>();
        }

        public string IdMatch { get; set; }

        public IList<Player> Players { get; set; }

        public IList<string> PlayersConnectionIds
        {
            get
            {
                return Players.Select(p => p.ConnectionId).ToList();
            }
            private set { }
        }
    }
}