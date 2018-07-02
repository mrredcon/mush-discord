using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Constants;
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
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;

        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            ExperienceService expService,
            CharacterService charService,
            EncounterService encounterService,
            IServiceProvider services,
            IConfiguration config)
        {
            _client = client;
            _commands = commands;
            _expService = expService;
            _charService = charService;
            _encounterService = encounterService;
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
                await ProcessExperienceAsync(context);
                await ProcessEncounterAsync(context);
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
        private async Task ProcessExperienceAsync(SocketCommandContext context)
        {
            if (!_expService.IsInExperienceEnabledChannel(context.Channel.Id)) return;

            var userInfo = context.User;
            var character = _charService.GetCharacter(userInfo.Id);

            if (character == null) return;

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
        private async Task ProcessEncounterAsync(SocketCommandContext context)
        {
            if (!_encounterService.IsInEncounterEnabledChannel(context.Channel.Id)) return;

            var userInfo = context.User;
            var character = _charService.GetCharacter(userInfo.Id);

            if (character == null) return;

            if (_encounterService.DoesCharacterGetInEncounter(character))
            {
                var encounter = _encounterService.GetRandomEncounter();

                var embed = EmbedTool.BuildBasicEmbed(encounter.Title, encounter.Description);

                await context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
            }
        }
    }
}
