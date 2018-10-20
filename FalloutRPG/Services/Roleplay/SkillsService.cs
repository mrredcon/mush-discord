using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class SkillsService
    {
        private const int DEFAULT_SKILL_POINTS = 10;
        public const int MAX_SKILL_LEVEL = 200;

        private readonly CharacterService _charService;
        private readonly SpecialService _specService;

        public SkillsService(CharacterService charService, SpecialService specService)
        {
            _charService = charService;
            _specService = specService;
        }

        /// <summary>
        /// Checks if character's skills are set.
        /// </summary>
        public bool AreSkillsSet(SkillSheet skillSheet)
        {
            if (skillSheet == null)
                return false;

            var properties = skillSheet.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.Equals("CharacterId") || prop.Name.Equals("Id"))
                    continue;

                var value = Convert.ToInt32(prop.GetValue(skillSheet));
                if (value == 0) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if character's skills are set.
        /// </summary>
        public bool AreSkillsSet(Character character) =>
            AreSkillsSet(character?.Skills);

        /// <summary>
        /// Returns the value of the specified character's given skill.
        /// </summary>
        /// <returns>Returns 0 if character or skills are null.</returns>
        public int GetSkill(SkillSheet skillSheet, Globals.SkillType skill)
        {
            if (!AreSkillsSet(skillSheet))
                return 0;

            return (int)typeof(SkillSheet).GetProperty(skill.ToString()).GetValue(skillSheet);
        }

        /// <summary>
        /// Returns the value of the specified character's given skill.
        /// </summary>
        /// <returns>Returns 0 if character or skills are null.</returns>
        public int GetSkill(Character character, Globals.SkillType skill) =>
            GetSkill(character?.Skills, skill);

        /// <summary>
        /// Returns the value of the specified character's given skill.
        /// </summary>
        /// <returns>Returns false if character or skills are null.</returns>
        public bool SetSkill(SkillSheet skillSheet, Globals.SkillType skill, int newValue)
        {
            if (!AreSkillsSet(skillSheet))
                return false;

            typeof(SkillSheet).GetProperty(skill.ToString()).SetValue(skillSheet, newValue);
            return true;
        }

        /// <summary>
        /// Returns the value of the specified character's given skill.
        /// </summary>
        /// <returns>Returns false if character or skills are null.</returns>
        public bool SetSkill(Character character, Globals.SkillType skill, int newValue) =>
            SetSkill(character?.Skills, skill, newValue);

        /// <summary>
        /// Gives character their skill points from leveling up.
        /// </summary>
        public void GiveSkillPoints(PlayerCharacter character)
        {
            if (character == null) throw new ArgumentNullException("character");

            var points = CalculateSkillPoints(character.Special.Intelligence);

            character.SkillPoints += points;
        }

        /// <summary>
        /// Calculate skill points given on level up.
        /// </summary>
        /// <remarks>
        /// Uses the Fallout New Vegas formula. (10 + (INT / 2))
        /// </remarks>
        public int CalculateSkillPoints(int intelligence)
        {
            return DEFAULT_SKILL_POINTS + (intelligence / 2);
        }

        /// <summary>
        /// Puts an amount of points in a specified skill.
        /// </summary>
        public void PutPointsInSkill(PlayerCharacter character, Globals.SkillType skill, int points)
        {
            if (character == null) throw new ArgumentNullException("character");

            if (!AreSkillsSet(character))
                throw new Exception(Exceptions.CHAR_SKILLS_NOT_SET);

            if (points < 1) return;

            if (points > character.SkillPoints)
                throw new Exception(Exceptions.CHAR_NOT_ENOUGH_SKILL_POINTS);

            var skillVal = GetSkill(character, skill);

            if ((skillVal + points) > MAX_SKILL_LEVEL)
                throw new Exception(Exceptions.CHAR_SKILL_POINTS_GOES_OVER_MAX);

            SetSkill(character, skill, skillVal + points);
            character.SkillPoints -= points;
        }
    }
}
