using FalloutRPG.Constants;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class NpcPresetService
    {
        private readonly IRepository<NpcPreset> _presetRepository;

        private readonly SkillsService _skillsService;
        private readonly SpecialService _specialService;

        public NpcPresetService(SkillsService skillsService, SpecialService specialService, IRepository<NpcPreset> presetRepository)
        {
            _skillsService = skillsService;
            _specialService = specialService;
            _presetRepository = presetRepository;
        }

        public async Task CreateNpcPreset(string name)
        {
            if (await GetNpcPreset(name) != null)
                throw new Exception(Exceptions.NPC_PRESET_ALREADY_EXISTS);

            NpcPreset preset = new NpcPreset { Name = name };
            await _presetRepository.AddAsync(preset);
        }

        public async Task SaveNpcPreset(NpcPreset npcPreset)
        {
            if (npcPreset == null)
                throw new ArgumentNullException("npcPreset");

            await _presetRepository.SaveAsync(npcPreset);
        }

        /// <summary>
        /// Returns an NpcPreset of the given name if it exists in the database, case-insensitively.
        /// </summary>
        /// <param name="typeString">The name of the NPC preset to find.</param>
        /// <returns>An NpcPreset with the given name in the database if it exists.</returns>
        public async Task<NpcPreset> GetNpcPreset(string typeString) => 
            await _presetRepository.Query.Where(x => x.Name.Equals(typeString, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
    }
}
