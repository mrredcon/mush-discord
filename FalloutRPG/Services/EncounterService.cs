using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Services
{
    public class EncounterService
    {
        private List<Encounter> Encounters;
        private List<ulong> EncounterEnabledChannels;

        public EncounterService()
        {
            Encounters = new List<Encounter>
            {
                new Encounter()
                {
                    Title = "Oh shit! An encounter!",
                    Description = "You have just gotten into a plain old boring encounter."
                },


                new Encounter()
                {
                    Title = "Fuck sake, here we go!",
                    Description = "A motherfucking damn shitty encounter that does fuck all."
                },


                new Encounter()
                {
                    Title = "Really nigga? Really?",
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
            var random = new Random();

            if (random.Next(100) > 50)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if Channel ID is an Encounter Enabled Channel.
        /// </summary>
        public bool IsInEncounterEnabledChannel(ulong channelId)
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
        public Encounter GetRandomEncounter()
        {
            var random = new Random();

            return Encounters[random.Next(Encounters.Count)];
        }
    }
}
