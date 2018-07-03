using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Callbacks;
using FalloutRPG.Constants;
using FalloutRPG.Models.Characters;
using FalloutRPG.Models.Encounters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class EncounterService
    {
        private List<BaseEncounter> Encounters;
        private List<ulong> EncounterEnabledChannels;

        public EncounterService()
        {
            // TODO: Load from database
            Encounters = new List<BaseEncounter>
            {
                new DialogEncounter()
                {
                    Title = "Dialog Encounter",
                    Description = "You have just gotten into a plain old boring encounter."
                },


                new EnemyEncounter()
                {
                    Title = "Enemy Encounter",
                    Description = "A motherfucking damn shitty encounter that does fuck all."
                },


                new LootEncounter()
                {
                    Title = "Loot Encounter",
                    Description = "You come across a footlocker. You fumble around trying to open it " +
                    "before realising it is locked. What do you do?",
                    LockpickRequired = 50,
                    Choices = new List<string>()
                    {
                        "Pick the lock. [Medium Lock]",
                        "Force the lock. [Risk Breaking Lock]"
                    }
                }
            };

            LoadEncounterEnabledChannels();
        }

        /// <summary>
        /// Checks to see if the player gets in an encounter.
        /// </summary>
        public bool DoesCharacterGetInEncounter(Character character)
        {
            // TODO: Improve this shit
            var random = new Random();

            if (random.Next(100) > 50)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if Channel ID is an Encounter Enabled Channel.
        /// </summary>
        public bool IsEncounterEnabledChannel(ulong channelId)
        {
            return EncounterEnabledChannels.Contains(channelId);
        }

        /// <summary>
        /// Loads the encounter enabled channels from
        /// the configuration file.
        /// </summary>
        private void LoadEncounterEnabledChannels()
        {
            // TODO: Load from config file
            EncounterEnabledChannels = new List<ulong>
            {
                462324906710007810
            };
        }

        /// <summary>
        /// Choose a random encounter type then a random
        /// encounter of that type.
        /// </summary>
        private BaseEncounter GetRandomEncounter()
        {
            var random = new Random();

            return Encounters[random.Next(Encounters.Count)];
        }

        public ReactionCallbackData BuildReactionCallbackData(
            SocketUser userInfo,
            Embed embed,
            ProcessedEncounter encounter)
        {
            var emojis = Emojis.NumberedList;
            var callbackData = new ReactionCallbackData(userInfo.Mention, embed, false, true);

            for (var i = 0; i < encounter.Callbacks.Count; i++)
            {
                callbackData.WithCallback(emojis[i], encounter.Callbacks.ElementAt(i).Value);
            }

            return callbackData;
        }

        public ProcessedEncounter ProcessEncounter(Character character)
        {
            var encounter = GetRandomEncounter();
            var content = BuildContentString(encounter);

            if (encounter is LootEncounter lootEncounter)
                return ProcessLootEncounter(character, lootEncounter, content);

            if (encounter is DialogEncounter dialogEncounter)
                return ProcessDialogEncounter(character, dialogEncounter, content);

            if (encounter is EnemyEncounter enemyEncounter)
                return ProcessEnemyEncounter(character, enemyEncounter, content);

            return null; 
        }

        private ProcessedEncounter ProcessLootEncounter(
            Character character,
            LootEncounter encounter,
            string content)
        {
            var callbacks = LootEncounterCallbacks.CreateCallbacks(character, encounter);

            Console.WriteLine("ProcessLootEncounter");

            return BuildProcessedEncounter(encounter, callbacks, content);
        }

        private ProcessedEncounter ProcessDialogEncounter(
            Character character, 
            DialogEncounter encounter,
            string content)
        {
            // Callbacks: Dialog Options 1, 2, 3, 4

            return null;
        }

        private ProcessedEncounter ProcessEnemyEncounter(
            Character character,
            EnemyEncounter encounter,
            string content)
        {
            // Callback: Fight
            // Callback: Run
            // Callback: Talk way out of it
            // Callback: Bribe

            return null;
        }

        private string BuildContentString(BaseEncounter encounter)
        {
            var content = new StringBuilder();
            var emojis = Emojis.NumberedList;

            content.Append($"**{encounter.Description}**\n\n");

            for (var i = 0; i < encounter.Choices.Count; i++)
            {
                content.Append($"{emojis[i]} {encounter.Choices[i]}\n\n");
            }

            return content.ToString();
        }

        private ProcessedEncounter BuildProcessedEncounter(
            BaseEncounter encounter,
            Dictionary<string, Func<SocketCommandContext, SocketReaction, Task>> callbacks,
            string content)
        {
            return new ProcessedEncounter()
            {
                Encounter = encounter,
                Callbacks = callbacks,
                Content = content
            };
        }
    }
}
