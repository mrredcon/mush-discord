using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Constants;
using FalloutRPG.Data.Models.Characters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace FalloutRPG.Services.Roleplay
{
    public class ExperienceService
    {
        private Dictionary<ulong, Timer> cooldownTimers;
        private List<ulong> experienceEnabledChannels;
        private readonly Random _random;

        private const int DEFAULT_EXP_GAIN = 100;
        private const int DEFAULT_EXP_RANGE_FROM = 10;
        private const int DEFAULT_EXP_RANGE_TO = 50;
        private const int COOLDOWN_INTERVAL = 60000;
        private const char OOC_CHAR = '(';

        private readonly CharacterService _charService;
        private readonly SkillsService _skillsService;
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;

        public ExperienceService(
            CharacterService charService,
            SkillsService skillsService,
            DiscordSocketClient client,
            IConfiguration config,
            Random random)
        {
            _charService = charService;
            _skillsService = skillsService;
            _client = client;
            _config = config;

            cooldownTimers = new Dictionary<ulong, Timer>();
            LoadExperienceEnabledChannels();
            _random = random;
        }

        /// <summary>
        /// Processes experience to give if channel is an experience
        /// enabled channel.
        /// </summary>
        public async Task ProcessExperienceAsync(SocketCommandContext context)
        {
            if (!IsInExperienceEnabledChannel(context.Channel.Id)) return;

            var userInfo = context.User;
            var character = await _charService.GetPlayerCharacterAsync(userInfo.Id);

            if (character == null || context.Message.ToString().StartsWith(OOC_CHAR)) return;

            var expToGive = GetRandomExperience();

            if (await GiveExperienceAsync(character, expToGive))
            {
                await context.Channel.SendMessageAsync(
                    string.Format(Messages.EXP_LEVEL_UP, userInfo.Mention, character.Level));
            }
        }

        /// <summary>
        /// Gives experience to a player character.
        /// </summary>
        public async Task<bool> GiveExperienceAsync(PlayerCharacter character, int experience = DEFAULT_EXP_GAIN)
        {
            if (character == null) return false;
            if (cooldownTimers.ContainsKey(character.Player.DiscordId)) return false;

            var initialLevel = character.Level;

            character.Experience += experience;
            await _charService.SaveCharacterAsync(character);

            var levelDifference = character.Level - initialLevel;

            if (levelDifference >= 1)
                await OnLevelUpAsync(character, levelDifference);

            AddToCooldown(character.Player.DiscordId);
            
            return levelDifference >= 1; // true if leveled up, false otherwise
        }

        /// <summary>
        /// Gets a random amount of experience to give
        /// between a range of two numbers.
        /// </summary>
        public int GetRandomExperience(
            int rangeFrom = DEFAULT_EXP_RANGE_FROM,
            int rangeTo = DEFAULT_EXP_RANGE_TO)
        {
            return _random.Next(rangeFrom, rangeTo);
        }

        /// <summary>
        /// Calculate the experience required for a level.
        /// </summary>
        public int CalculateExperienceForLevel(int level)
        {
            if (level < 1 || level > 1000) return -1;
            if (level == 1)
                return 0;
            return level * 1000;
        }

        /// <summary>
        /// Calculates the level depending on the experience.
        /// </summary>
        public int CalculateLevelForExperience(int experience)
        {
            return experience / 1000 + 1;
        }

        /// <summary>
        /// Checks if the input Channel ID is an experience
        /// enabled channel.
        /// </summary>
        public bool IsInExperienceEnabledChannel(ulong channelId)
        {
            return experienceEnabledChannels.Contains(channelId);
        }

        /// <summary>
        /// Loads the experience enabled channels from the
        /// configuration file.
        /// </summary>
        private void LoadExperienceEnabledChannels()
        {
            try
            {
                experienceEnabledChannels = _config
                    .GetSection("roleplay:exp-channels")
                    .GetChildren()
                    .Select(x => UInt64.Parse(x.Value))
                    .ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("You have not specified any experience enabled channels in Config.json");
            }
        }

        /// <summary>
        /// Called when a character levels up.
        /// </summary>
        private async Task OnLevelUpAsync(PlayerCharacter character, int times = 1)
        {
            if (character == null) throw new ArgumentNullException("character");
            var user = _client.GetUser(character.Player.DiscordId);

            await user.SendMessageAsync(string.Format(Messages.SKILLS_LEVEL_UP, user.Mention, character.ExperiencePoints));
        }

        /// <summary>
        /// Adds a user's Discord ID to the cooldowns.
        /// </summary>
        private void AddToCooldown(ulong discordId)
        {
            var timer = new Timer();
            timer.Elapsed += (sender, e) => OnCooldownElapsed(sender, e, discordId);
            timer.Interval = COOLDOWN_INTERVAL;
            timer.Enabled = true;

            cooldownTimers.Add(discordId, timer);
        }

        /// <summary>
        /// Called when a cooldown has finished.
        /// </summary>
        private void OnCooldownElapsed(object sender, ElapsedEventArgs e, ulong discordId)
        {
            var timer = cooldownTimers[discordId];
            timer.Enabled = false;
            timer.Dispose();

            cooldownTimers.Remove(discordId);
        }
    }
}
