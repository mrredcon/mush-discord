using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Callbacks;
using FalloutRPG.Constants;
using FalloutRPG.Models.Characters;
using FalloutRPG.Util;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly ExperienceService _expService;
        private readonly CharacterService _charService;
        private readonly EncounterService _encounterService;
        private readonly InteractiveService _interactiveService;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;

        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            ExperienceService expService,
            CharacterService charService,
            EncounterService encounterService,
            InteractiveService interactiveService,
            IServiceProvider services,
            IConfiguration config)
        {
            _client = client;
            _commands = commands;
            _expService = expService;
            _charService = charService;
            _encounterService = encounterService;
            _interactiveService = interactiveService;
            _services = services;
            _config = config;
        }

        /// <summary>
        /// Installs the commands and subscribes to MessageReceived event.
        /// </summary>
        public async Task InstallCommandsAsync()
        {
            await _commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _services);
            _client.MessageReceived += HandleCommandAsync;
        }

        /// <summary>
        /// Handles incoming commands if it begins with specified prefix.
        /// If there is no prefix, it will process experience and encounters.
        /// </summary>
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null || message.Author.IsBot) return;

            int argPos = 0;
            var context = new SocketCommandContext(_client, message);

            if (!(message.HasStringPrefix(_config["prefix"], ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {
                var userInfo = context.User;
                var character = _charService.GetCharacter(userInfo.Id);

                if (character == null) return;

                await ProcessExperienceAsync(context, userInfo, character);
                await ProcessEncounterAsync(context, userInfo, character);
                return;
            }

            context = new SocketCommandContext(_client, message);

            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }

        /// <summary>
        /// Processes experience to give if channel is an experience
        /// enabled channel.
        /// </summary>
        private async Task ProcessExperienceAsync(
            SocketCommandContext context,
            SocketUser userInfo,
            Character character)
        {
            if (!_expService.IsExperienceEnabledChannel(context.Channel.Id))
                return;

            var expToGive = _expService.GetRandomExperience();

            if (await _expService.GiveExperienceAsync(character, expToGive))
            {
                var level = _expService.CalculateLevelForExperience(character.Experience);
                await context.Channel.SendMessageAsync(
                    string.Format(Messages.EXP_LEVEL_UP, userInfo.Mention, level));
            }
        }

        /// <summary>
        /// Process encounter if channel is in encounter channel.
        /// </summary>
        private async Task ProcessEncounterAsync(
            SocketCommandContext context,
            SocketUser userInfo,
            Character character)
        {
            if (!_encounterService.IsEncounterEnabledChannel(context.Channel.Id))
                return;

            if (_encounterService.DoesCharacterGetInEncounter(character))
            {
                var emojis = new Emoji[]
                {
                    new Emoji("1⃣"),
                    new Emoji("2⃣")
                };

                var encounter = _encounterService.GetRandomEncounter();

                var embed = EmbedTool.BuildBasicEmbed(encounter.Title, encounter.Description);

                var processed = _encounterService.ProcessEncounter(character, encounter);

                var reactioncallbackdata = new ReactionCallbackData(userInfo.Mention, embed, false, true);
                
                for (var i = 0; i < processed.Callbacks.Count; i++)
                {
                    reactioncallbackdata.WithCallback(emojis[i], processed.Callbacks.ElementAt(i).Value);
                }

                var message = await _interactiveService.SendMessageWithReactionCallbacksAsync(
                    context, reactioncallbackdata, true);
            }
        }
    }
}
