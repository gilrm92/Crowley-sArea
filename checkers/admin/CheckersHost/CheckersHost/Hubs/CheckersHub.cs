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

        public void GetAvaiableOpponents()
        {
            LogManager.Log("Geting avaiable opponents");
            string myConnectionId = Context.ConnectionId;
            IList<Player> avaiablePlayers = _playersOnline.Where(p => p.ConnectionId != myConnectionId).ToList();
            LogManager.Log(string.Format("Returning avaiable opponents: {0}", avaiablePlayers.Count));

            Clients.Client(myConnectionId).getAvaiableOpponentsCallback(avaiablePlayers);
        }

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