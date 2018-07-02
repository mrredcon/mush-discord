using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Callbacks
{
    public class TestReactionCallback : IReactionCallback
    {
        public RunMode RunMode => RunMode.Sync;

        public ICriterion<SocketReaction> Criterion { get; }

        public TimeSpan? Timeout { get; }

        public SocketCommandContext Context { get; }

        public TestReactionCallback(
             SocketCommandContext context,
             ICriterion<SocketReaction> criterion = null)
        {
            Context = context;
            Criterion = criterion ?? new EmptyCriterion<SocketReaction>();
            Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<bool> HandleCallbackAsync(SocketReaction reaction)
        {
            Console.WriteLine("IT FUCKING WORKED DUDE");

            return true;
        }
    }
}
