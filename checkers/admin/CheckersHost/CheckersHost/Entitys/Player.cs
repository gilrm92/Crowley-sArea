using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersHost.Entitys
{
    public class Player
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        public bool IsPlaying { get; set; }

        [JsonProperty("ConnectionId")]
        public string ConnectionId { get; set; }
    }
}