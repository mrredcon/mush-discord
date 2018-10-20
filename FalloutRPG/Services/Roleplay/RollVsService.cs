using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Linq;

namespace FalloutRPG.Services.Roleplay
{
    public class RollVsService
    {
        private readonly EffectsService _effectsService;
        private readonly ItemService _itemService;
        private readonly RollService _rollService;
        private readonly SkillsService _skillsService;

        public RollVsService(EffectsService effectsService, ItemService itemService, RollService rollService, SkillsService skillsService)
        {
            _effectsService = effectsService;
            _itemService = itemService;
            _rollService = rollService;
            _skillsService = skillsService;
        }
    }
}