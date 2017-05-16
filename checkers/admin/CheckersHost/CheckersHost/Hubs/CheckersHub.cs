using CheckersHost.Entitys;
using CheckersHost.Log;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public void AskForMatch(string connectionId) 
        {
            try
            {
                LogManager.Log(string.Format("asking for connectionId: {0}", connectionId));
                string connectionIdRequest = Context.ConnectionId;
                string nameRequest = _playersOnline.FirstOrDefault(p => p.ConnectionId == connectionIdRequest).Name;

                Clients.Client(connectionId).askForMatchCallback(connectionIdRequest, nameRequest);
            }
            catch (Exception ex)
            {
                LogManager.Log(string.Format("Error. Message: {0}, Stack: {1}", ex.Message, ex.StackTrace));
            }
            
        }

        public void ResponseForMatch(string connectionIdToResponse, bool confirmation)
        {
            Clients.Client(connectionIdToResponse).responseFormMatchCallback(confirmation);
        }

        public void PrepareMatch(string connectionIdRequestedMatch)
        {
            try
            {
                Player playerOne = _playersOnline.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
                Player playerTwo = _playersOnline.FirstOrDefault(p => p.ConnectionId == connectionIdRequestedMatch);

                Match match = new Match();
                match.Players.Add(playerOne);
                match.Players.Add(playerTwo);

                _matchesHappening.Add(match);
                Clients.Clients(match.PlayersConnectionIds).prepareMatchCallback(match.IdMatch);

            }
            catch (Exception ex)
            {
                LogManager.Log(string.Format("Error. Message: {0}, Stack: {1}", ex.Message, ex.StackTrace));
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            LogManager.Log(string.Format("Client disconnected. ConnectionId: {0}", Context.ConnectionId));
            _playersOnline.Remove(_playersOnline.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId));

            //Should take match and update it.
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            LogManager.Log(string.Format("Client reconnected. ConnectionId: {0}", Context.ConnectionId));
            return base.OnReconnected();
        }
    }
}