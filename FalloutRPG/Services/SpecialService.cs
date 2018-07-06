﻿using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class SpecialService
    {
        private const int DEFAULT_SPECIAL_POINTS = 40;

        private readonly CharacterService _charService;

        public SpecialService(CharacterService charService)
        {
            _charService = charService;
        }

        /// <summary>
        /// Set character's special.
        /// </summary>
        public async Task SetInitialSpecialAsync(Character character, int[] special)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

            if (special.Length != 7)
                throw new ArgumentException(Exceptions.CHAR_SPECIAL_LENGTH);

            if (special.Sum() != DEFAULT_SPECIAL_POINTS)
                throw new ArgumentException(Exceptions.CHAR_SPECIAL_DOESNT_ADD_UP);

            InitializeSpecial(character, special);

            await _charService.SaveCharacterAsync(character);
        }

        /// <summary>
        /// Initializes character's special.
        /// </summary>
        private void InitializeSpecial(Character character, int[] special)
        {
            character.Special = new Special()
            {
                Strength = special[0],
                Perception = special[1],
                Endurance = special[2],
                Charisma = special[3],
                Intelligence = special[4],
                Agility = special[5],
                Luck = special[6]
             };
        }

        /// <summary>
        /// Checks if a character's special has been set.
        /// </summary>
        public bool IsSpecialSet(Character character)
        {
            if (character == null) return false;

            var properties = character.Special.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.Equals("CharacterId") || prop.Name.Equals("Id"))
                    continue;

                var value = Convert.ToInt32(prop.GetValue(character.Special));
                if (value == 0) return false;
            }

            return true;
        }
    }
}
