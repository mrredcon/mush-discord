using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Models.Encounters
{
    public class ProcessedEncounter
    {
        public BaseEncounter Encounter { get; set; }

        public Dictionary<string, Func<SocketCommandContext, SocketReaction, Task>> Callbacks { get; set; }
    }
}