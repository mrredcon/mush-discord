using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class SpecialService
    {
        public const int DEFAULT_SPECIAL_POINTS = 29;
        public const int MIN_SPECIAL = 1;
        public const int MAX_SPECIAL = 8;

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
            if (character == null) throw new ArgumentNullException("character");

            if (!IsSpecialInRange(special))
                throw new ArgumentException(Exceptions.CHAR_SPECIAL_NOT_IN_RANGE);

            if (special.Sum() != DEFAULT_SPECIAL_POINTS)
                throw new ArgumentException(Exceptions.CHAR_SPECIAL_DOESNT_ADD_UP);

            character.Special = InitializeSpecial(special);

            await _charService.SaveCharacterAsync(character);
        }

        /// <summary>
        /// Returns the value of the specified character's given special.
        /// </summary>
        /// <returns>Returns 0 if character or special values are null.</returns>
        public int GetSpecial(Special specialSheet, Globals.SpecialType special)
        {
            if (specialSheet == null || !IsSpecialSet(specialSheet))
                return 0;

            return (int)typeof(Special).GetProperty(special.ToString()).GetValue(specialSheet);
        }

        /// <summary>
        /// Returns the value of the specified character's given special.
        /// </summary>
        /// <returns>Returns 0 if character or special values are null.</returns>
        public int GetSpecial(Character character, Globals.SpecialType special) =>
            GetSpecial(character?.Special, special);

        /// <summary>
        /// Returns the value of the specified character's given skill.
        /// </summary>
        /// <returns>Returns false if character or skills are null.</returns>
        public bool SetSpecial(Special specialSheet, Globals.SpecialType special, int newValue)
        {
            if (!IsSpecialSet(specialSheet))
                return false;

            typeof(Special).GetProperty(special.ToString()).SetValue(specialSheet, newValue);
            return true;
        }

        /// <summary>
        /// Returns the value of the specified character's given skill.
        /// </summary>
        /// <returns>Returns false if character or skills are null.</returns>
        public bool SetSpecial(Character character, Globals.SpecialType special, int newValue) =>
            SetSpecial(character?.Special, special, newValue);

        /// <summary>
        /// Checks if each number in SPECIAL is between 1 and 10
        /// and ensures there are 7 elements in the input array.
        /// </summary>
        private bool IsSpecialInRange(int[] special)
        {
            if (special.Length != 7) return false;

            foreach (int sp in special)
                if (sp < MIN_SPECIAL || sp > MAX_SPECIAL)
                    return false;

            // Unique MUSH rules :/
            if (special.Where(sp => sp == 8).Count() > 2)
                return false;

            return true;
        }

        /// <summary>
        /// Initializes a special.
        /// </summary>
        private Special InitializeSpecial(int[] special)
        {
            return new Special()
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
        public bool IsSpecialSet(Special specialSheet)
        {
            if (specialSheet == null) return false;

            foreach (int value in specialSheet.SpecialArray)
                if (value < MIN_SPECIAL) return false;

            return true;
        }

        /// <summary>
        /// Checks if a character's special has been set.
        /// </summary>
        public bool IsSpecialSet(Character character) =>
            IsSpecialSet(character?.Special);
    }
}
