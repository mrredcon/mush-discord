using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Models.Characters;
using FalloutRPG.Models.Encounters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FalloutRPG.Callbacks
{
    public class LootEncounterCallbacks
    {
        public static Dictionary<string, Func<SocketCommandContext, SocketReaction, Task>> 
            CreateCallbacks(Character character, LootEncounter encounter)
        {
            var callbacks = new Dictionary<string, Func<SocketCommandContext, SocketReaction, Task>>();

            Task PickLock(SocketCommandContext c, SocketReaction r)
            {
                if (character.Skills.Lockpick >= encounter.LockpickRequired)
                {
                    c.Channel.SendMessageAsync($"You manage to pick the lock and get what's inside! ({c.User.Mention})");
                }
                else
                {
                    c.Channel.SendMessageAsync($"You don't have the required skills to pick this lock. ({c.User.Mention})");
                }

                return Task.CompletedTask;
            }

            Task ForceLock(SocketCommandContext c, SocketReaction r)
            {
                c.Channel.SendMessageAsync($"{c.User.Mention}: You try and fail to force the lock.");
                return Task.CompletedTask;
            }

            callbacks.Add("PickLock", PickLock);
            callbacks.Add("ForceLock", ForceLock);

            return callbacks;
        }
    }
}
