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
        }

        public string IdMatch { get; set; }
        
        private Player _playerOne;
        public Player PlayerOne
        {
            get 
            {
                return _playerOne;
            }
            set
            {
                PlayersConnectionIds.Add(value.ConnectionId);
                _playerOne = value;
            }
        }
        private Player _playerTwo;
        public Player PlayerTwo
        {
            get 
            {
                return _playerTwo;
            }
            set
            {
                PlayersConnectionIds.Add(value.ConnectionId);
                _playerTwo = value;
            }
        }
        public IList<string> PlayersConnectionIds { get; private set; }
    }
}