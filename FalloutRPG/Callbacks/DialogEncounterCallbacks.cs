using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Constants;
using FalloutRPG.Models.Characters;
using FalloutRPG.Models.Encounters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Callbacks
{
    public class DialogEncounterCallbacks
    {
        public static Dictionary<string, Func<SocketCommandContext, SocketReaction, Task>>
            CreateCallbacks(Character character, DialogEncounter encounter)
        {
            switch (encounter.Title)
            {
                case Encounters.DIALOG_MAN_NEEDS_HELP:
                    Task Help(SocketCommandContext c, SocketReaction r)
                    {


                        return Task.CompletedTask;
                    }

                    Task CharismaBarter(SocketCommandContext c, SocketReaction r)
                    {

                        return Task.CompletedTask;
                    }

                    Task Attack(SocketCommandContext c, SocketReaction r)
                    {
                        return Task.CompletedTask;
                    }

                    Task Run(SocketCommandContext c, SocketReaction r)
                    {
                        return Task.CompletedTask;
                    }
                    break;
            }

            return null;
        }
    }
}
