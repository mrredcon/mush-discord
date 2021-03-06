﻿using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Data;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Data.Models;
using FalloutRPG.Services;
using FalloutRPG.Services.Roleplay;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using FalloutRPG.Data.Models.Characters;
using FalloutRPG.Data.Models.Effects;
using FalloutRPG.Data.Models.Items;
using FalloutRPG.Data.Models.Scenes;

namespace FalloutRPG
{
    public class Program
    {
        private IConfiguration config;

        /// <summary>
        /// The entry point of the program.
        /// </summary>
        public static void Main(string[] args)
                => new Program().MainAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Initializes the services from the service provider.
        /// </summary>
        /// <remarks>
        /// await Task.Delay(-1) is required or else the program
        /// will end.
        /// </remarks>
        public async Task MainAsync()
        {
            config = BuildConfig();

            var services = BuildServiceProvider();

            services.GetRequiredService<LogService>();
            await services.GetRequiredService<CommandHandler>().InstallCommandsAsync();
            await services.GetRequiredService<StartupService>().StartAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// Builds the service provider for dependency injection.
        /// </summary>
        private IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            }))
            .AddSingleton(config)
            .AddSingleton<CommandHandler>()
            .AddSingleton<LogService>()
            .AddSingleton<StartupService>()
            .AddSingleton<HelpService>()
            .AddSingleton<PlayerService>()
            .AddSingleton<Random>()

            // Roleplay
            .AddSingleton<SkillsService>()
            .AddSingleton<SpecialService>()
            .AddSingleton<StartupService>()
            .AddSingleton<CharacterService>()
            .AddSingleton<RollService>()
            .AddSingleton<RollDiceService>()
            .AddSingleton<ExperienceService>()
            .AddSingleton<NpcService>()
            .AddSingleton<NpcPresetService>()
            .AddSingleton<CampaignService>()
            .AddSingleton<CampaignCheatService>()
            .AddSingleton<EffectsService>()
            .AddSingleton<ItemService>()

            // Addons
            .AddSingleton<InteractiveService>()

            // Database
            .AddEntityFrameworkSqlite()
                .AddDbContext<RpgContext>(optionsAction: options => options
                .UseLazyLoadingProxies()
                .UseSqlite("Filename=MUSHDB.db"))
            .AddTransient<IRepository<Campaign>, EfRepository<Campaign>>()
            .AddTransient<IRepository<Character>, EfRepository<Character>>()
            .AddTransient<IRepository<Effect>, EfRepository<Effect>>()
            .AddTransient<IRepository<Item>, EfRepository<Item>>()
            .AddTransient<IRepository<NpcPreset>, EfRepository<NpcPreset>>()
            .AddTransient<IRepository<Player>, EfRepository<Player>>()
            .AddTransient<IRepository<Scene>, EfRepository<Scene>>()
            .AddTransient<IRepository<SkillSheet>, EfRepository<SkillSheet>>()
            .AddTransient<IRepository<Special>, EfRepository<Special>>()
            .BuildServiceProvider();

        /// <summary>
        /// Builds the configuration from the Config.json file.
        /// </summary>
        private IConfiguration BuildConfig() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Config.json")
            .Build();
    }
}