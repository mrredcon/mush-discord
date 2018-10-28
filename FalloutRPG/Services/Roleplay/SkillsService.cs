using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class SkillsService
    {
        private const int DEFAULT_SKILL_POINTS = 10;
        public const int MAX_SKILL_LEVEL = 12;
        public const int MIN_TAG = 0;
        public const int MAX_TAG = 6;
        public const int POINTS_TAG = 36;

        private readonly CharacterService _charService;
        private readonly SpecialService _specService;

        public SkillsService(CharacterService charService, SpecialService specService)
        {
            _charService = charService;
            _specService = specService;
        }

        /// <summary>
        /// Checks if character's skills are tagged.
        /// </summary>
        public bool AreSkillsTagged(SkillSheet skillSheet)
        {
            if (skillSheet == null)
                return false;
            if (skillSheet.SkillsArray.Sum() >= POINTS_TAG)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if character's skills are set.
        /// </summary>
        public bool AreSkillsTagged(Character character) =>
            AreSkillsTagged(character?.Skills);

        /// <summary>
        /// Returns the value of the specified character's given skill.
        /// </summary>
        /// <returns>Returns 0 if character or skills are null.</returns>
        public int GetSkill(SkillSheet skillSheet, Globals.SkillType skill)
        {
            if (skillSheet == null)
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
            if (skillSheet == null)
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

            if (points < 1) return;

            //if (points > character.SkillPoints)
                //throw new Exception(Exceptions.CHAR_NOT_ENOUGH_SKILL_POINTS);

            var skillVal = GetSkill(character, skill);

            if ((skillVal + points) > MAX_SKILL_LEVEL)
                throw new Exception(Exceptions.CHAR_SKILL_POINTS_GOES_OVER_MAX);

            SetSkill(character, skill, skillVal + points);
            //character.SkillPoints -= points;
        }

        /// <summary>
        /// Used during chargen to set initial (tag) skills
        /// </summary>
        /// <param name="character">The character to set tag skills</param>
        /// <param name="tag">The skill to tag</param>
        /// <param name="points">The value to set 'tag' equal to.</param>
        /// <returns>Remaining points.</returns>
        public async Task<int> TagSkill(PlayerCharacter character, Globals.SkillType tag, int points)
        {
            if (character == null) throw new ArgumentNullException("character");

            if (!IsTagInRange(character.Skills, points))
                throw new ArgumentException(Exceptions.CHAR_TAGS_OUT_OF_RANGE);

            // Refund skill points used if overwriting the same skill
            character.TagPoints += GetSkill(character, tag);

            if (character.TagPoints - points < 0)
                throw new Exception(Exceptions.CHAR_NOT_ENOUGH_SKILL_POINTS);

            SetSkill(character, tag, points);
            character.TagPoints -= points;

            await _charService.SaveCharacterAsync(character);
            return character.TagPoints;
        }

        private bool IsTagInRange(SkillSheet skills, int points)
        {
            if (points < MIN_TAG || points > MAX_TAG)
                return false;

            // Unique MUSH rules :/
            if (skills.SkillsArray.Where(sk => sk == MAX_TAG).Count() > 2)
                return false;
            if (points == MAX_TAG && skills.SkillsArray.Where(sk => sk == MAX_TAG).Count() >= 2)
                return false;

            return true;
        }
    }
}
