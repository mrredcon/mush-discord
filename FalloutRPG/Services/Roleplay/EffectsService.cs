using FalloutRPG.Constants;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class EffectsService
    {
        private readonly CharacterService _characterService;
        private readonly IRepository<Effect> _effectsRepository;
        private readonly SkillsService _skillsService;
        private readonly SpecialService _specialService;

        public EffectsService(
            CharacterService characterService,
            IRepository<Effect> effectsRepository,
            SkillsService skillsService,
            SpecialService specialService)
        {
            _characterService = characterService;
            _effectsRepository = effectsRepository;
            _skillsService = skillsService;
            _specialService = specialService;
        }

        public async Task CreateEffectAsync(string name, string desc) =>
            await _effectsRepository.AddAsync(new Effect { Name = name, Description = desc });

        public async Task DeleteEffectAsync(Effect effect) =>
            await _effectsRepository.DeleteAsync(effect);

        public async Task<Effect> GetEffectAsync(string name) =>
            await _effectsRepository.Query.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();

        public void ApplyEffect(Character character, Effect effect) =>
            ApplyEffect(character, effect);

        public void RemoveEffect(Character character, Effect effect) =>
            ApplyEffect(character, effect, true);

        private void ApplyEffect(Character character, Effect effect, bool removeEffect = false)
        {
            if (effect.SpecialAdditions != null && effect.SpecialAdditions.Count > 0)
            {
                foreach (var addition in effect.SpecialAdditions)
                {
                    var oldValue = _specialService.GetSpecial(character, addition.SpecialAttribute);

                    if (removeEffect)
                        _specialService.SetSpecial(character, addition.SpecialAttribute, oldValue - addition.EffectValue);
                    else
                        _specialService.SetSpecial(character, addition.SpecialAttribute, oldValue + addition.EffectValue);
                }
            }

            if (effect.SkillAdditions != null && effect.SkillAdditions.Count > 0)
            {
                foreach (var addition in effect.SkillAdditions)
                {
                    var oldValue = _skillsService.GetSkill(character, addition.Skill);

                    if (removeEffect)
                        _skillsService.SetSkill(character, addition.Skill, oldValue - addition.EffectValue);
                    else
                        _skillsService.SetSkill(character, addition.Skill, oldValue + addition.EffectValue);
                }
            }

            if (removeEffect)
            {
                character.ArmorClass -= effect.ArmorClassAddition;
                character.Effects.Remove(effect);
            }
            else
            {
                character.ArmorClass += effect.ArmorClassAddition;
                character.Effects.Add(effect);
            }
        }

        public string GetEffectInfo(Effect effect)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{effect.Name}: ");
            if (effect.ArmorClassAddition != 0)
                sb.Append($"**AC:** {effect.ArmorClassAddition} ");

            if (effect.SpecialAdditions != null && effect.SpecialAdditions.Count > 0)
            {
                sb.Append("**S.P.E.C.I.A.L. Buffs/Debuffs:** ");
                foreach (var entry in effect.SpecialAdditions)
                    sb.Append($"{Globals.SPECIAL_PROPER_NAMES[entry.SpecialAttribute]}: {entry.EffectValue} ");
            }

            if (effect.SkillAdditions != null && effect.SkillAdditions.Count > 0)
            {
                sb.Append("**Skill Buffs/Debuffs:** ");
                foreach (var entry in effect.SkillAdditions)
                    sb.Append($"{Globals.SKILL_PROPER_NAMES[entry.Skill]}: {entry.EffectValue} ");
            }

            return sb.ToString();
        }

        public string GetCharacterEffects(Character character)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var effect in character.Effects)
                sb.Append($"{GetEffectInfo(effect)}\n");

            return sb.ToString();
        }
    }
}
