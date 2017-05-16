using CheckersHost.Entitys;
using CheckersHost.Log;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersHost.Hubs
{
    public class CheckersHub : Hub
    {
        private static IList<Player> _playersOnline = new List<Player>();
        private static IList<Match> _matchesHappening = new List<Match>();

        public void RegisterPlayer(string name, string email)
        {
            LogManager.Log(string.Format("new player. Name: {0}, Email: {1}, ConnectionId: {2}", name, email, Context.ConnectionId));
            Player newPlayer = new Player();
            newPlayer.Name = name;
            newPlayer.Email = email;
            newPlayer.ConnectionId = Context.ConnectionId;
            _playersOnline.Add(newPlayer);
            LogManager.Log("Player registered");

            Clients.All.registerPlayerCallback();
        }

        //public void CheckConnection() 
        //{
        //    try
        //    {
        //        Player player = _playersOnline.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        //        Clients.Client(Context.ConnectionId).checkSessionCallback(playerConverted);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Log(string.Format("Error! Message:{0}, Stack: {1}", ex.Message, ex.StackTrace));
        //    }
        //}

        public void GetAvaiableOpponents()
        {
            LogManager.Log("Geting avaiable opponents");
            string myConnectionId = Context.ConnectionId;
            IList<Player> avaiablePlayers = _playersOnline.Where(p => p.ConnectionId != myConnectionId).ToList();
            LogManager.Log(string.Format("Returning avaiable opponents: {0}", avaiablePlayers.Count));

            Clients.Client(myConnectionId).getAvaiableOpponentsCallback(avaiablePlayers);
        }

        //public void CheckForOpponent()
        //{
        //    string myConnectionId = Context.ConnectionId;
        //    bool opponentFound = false;

        //    LogManager.Log(string.Format("Looking for opponents. ConnectionId {0}", myConnectionId));
        //    Player opponent = _playersOnline.FirstOrDefault(p => p.ConnectionId != myConnectionId && !p.IsPlaying);
        //    LogManager.Log(string.Format("Opponent is null: {0}", opponent != null));
        //    if (opponent != null)
        //    {
        //        LogManager.Log(string.Format("Opponent found. ConnectionId: {0}, Name: {1}", opponent.ConnectionId, opponent.Name));
        //        Match newMatch = new Match();
        //        newMatch.PlayerOne = _playersOnline.FirstOrDefault(p => p.ConnectionId == myConnectionId);
        //        newMatch.PlayerTwo = opponent;
        //        newMatch.PlayerOne.IsPlaying = true;
        //        newMatch.PlayerTwo.IsPlaying = true;
        //        _matchesHappening.Add(newMatch);

        //        opponentFound = true;
        //        Clients.Clients(newMatch.PlayersConnectionIds).startMatchCallback(opponentFound, newMatch);
        //    }

        //    LogManager.Log(string.Format("OpponentFound: {0}", opponentFound));
        //    Clients.Client(myConnectionId).startMatchCallback(opponentFound);
        //}

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            LogManager.Log(string.Format("Client disconnected. ConnectionId: {0}", Context.ConnectionId));
            _playersOnline.Remove(_playersOnline.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId));

            return base.OnDisconnected(stopCalled);
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            
            LogManager.Log(string.Format("Client reconnected. ConnectionId: {0}", Context.ConnectionId));
            Player player = _playersOnline.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            Clients.Client(Context.ConnectionId).checkSessionCallback(player);
            return base.OnReconnected();
        }
    }
}