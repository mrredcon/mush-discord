﻿using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Data.Models.Scenes;
using FalloutRPG.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    public class FalloutModule : ModuleBase<SocketCommandContext>
    {
        private readonly IRepository<Scene> _sceneRepository;

        public FalloutModule(IRepository<Scene> sceneRepository)
        {
            _sceneRepository = sceneRepository;
        }

        [Command("daysleft")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        [Alias("countdown", "days")]
        public async Task DaysLeftAsync()
        {
            var today = DateTime.Now;
            var release = new DateTime(2018, 11, 14);
            var span = (release - today);

            await ReplyAsync(
                $"There are {span.Days} days," +
                $" {span.Hours} hours," +
                $" {span.Minutes} minutes," +
                $" {span.Seconds} seconds " +
                $"and {span.Milliseconds} milliseconds left until release! (UTC)");
        }

        [RequireOwner]
        [Command("echo")]
        public async Task EchoAsync(string message)
        {
            await ReplyAsync(message);
        }
    }
}