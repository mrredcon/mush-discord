using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Callbacks;
using FalloutRPG.Models.Characters;
using FalloutRPG.Models.Encounters;
using System;
using System.Collections.Generic;
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
                    Description = "Y u gotta do this to me boi?"
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
        public BaseEncounter GetRandomEncounter()
        {
            var random = new Random();

            return Encounters[random.Next(Encounters.Count)];
        }

        public ProcessedEncounter ProcessEncounter(Character character, BaseEncounter baseEncounter)
        {
            if (baseEncounter is LootEncounter lootEncounter)
                return ProcessLootEncounter(character, lootEncounter);

            if (baseEncounter is DialogEncounter dialogEncounter)
                return ProcessDialogEncounter(character, dialogEncounter);

            if (baseEncounter is EnemyEncounter enemyEncounter)
                return ProcessEnemyEncounter(character, enemyEncounter);

            return null; 
        }

        private ProcessedEncounter ProcessLootEncounter(Character character, LootEncounter encounter)
        {
            // Callback: Pick lock
            // Callback: Force lock (Risk breaking pick)
            var callbacks = LootEncounterCallbacks.CreateCallbacks(character, encounter);

            var processed = new ProcessedEncounter()
            {
                Encounter = encounter,
                Callbacks = callbacks
            };

            return processed;
        }

        private ProcessedEncounter ProcessDialogEncounter(Character character, DialogEncounter encounter)
        {
            // Callbacks: Dialog Options 1, 2, 3, 4

            return null;
        }

        private ProcessedEncounter ProcessEnemyEncounter(Character character, EnemyEncounter encounter)
        {
            // Callback: Fight
            // Callback: Run
            // Callback: Talk way out of it
            // Callback: Bribe

            return null;
        }

        // I think ProcessedEncounter Model
        // BaseEncounter Encounter {get;set;}
        // List<Func<T, T>> Callbacks;
    }
}
