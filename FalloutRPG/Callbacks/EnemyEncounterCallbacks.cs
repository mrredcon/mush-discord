using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Models.Characters;
using FalloutRPG.Models.Encounters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Callbacks
{
    public class EnemyEncounterCallbacks
    {
        public static Dictionary<string, Func<SocketCommandContext, SocketReaction, Task>>
            CreateCallbacks(Character character, EnemyEncounter encounter)
        {
            var callbacks = new Dictionary<string, Func<SocketCommandContext, SocketReaction, Task>>();

            Task Attack(SocketCommandContext c, SocketReaction r)
            {
                if (character.Level > encounter.Level)
                {
                    c.Channel.SendMessageAsync($"You defeated your enemy! ({c.User.Mention})");
                }
                else
                {
                    c.Channel.SendMessageAsync($"You were out gunned and left for dead. They also took some shit. ({c.User.Mention})");
                }

                return Task.CompletedTask;
            }

            Task Run(SocketCommandContext c, SocketReaction r)
            {
                c.Channel.SendMessageAsync($"You ran away like a little coward. ({c.User.Mention})");
                return Task.CompletedTask;
            }

            Task Charisma(SocketCommandContext c, SocketReaction r)
            {
                if (character.Special.Charisma > encounter.Charisma)
                {
                    c.Channel.SendMessageAsync($"You manage to talk your way out of the encounter. ({c.User.Mention})");
                }
                else
                {
                    c.Channel.SendMessageAsync($"Your efforts to talk your way out of it failed. You were shot and left for dead. ({c.User.Mention})");
                }

                return Task.CompletedTask;
            }

            Task Barter(SocketCommandContext c, SocketReaction r)
            {
                if (character.Skills.Barter > encounter.Barter)
                {
                    c.Channel.SendMessageAsync($"You fail to sell anything. ({c.User.Mention})");
                }
                else
                {
                    c.Channel.SendMessageAsync($"You managed to bribe your way out of it. ({c.User.Mention})");
                }

                return Task.CompletedTask;
            }


            callbacks.Add("Attack", Attack);
            callbacks.Add("Run", Run);
            callbacks.Add("Charisma", Charisma);
            callbacks.Add("Barter", Barter);

            return callbacks;
        }
    }
}
