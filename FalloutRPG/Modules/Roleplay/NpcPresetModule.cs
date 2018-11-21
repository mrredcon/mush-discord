using Discord;
using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Helpers;
using FalloutRPG.Data.Models;
using FalloutRPG.Services;
using FalloutRPG.Services.Roleplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("npc preset")]
    [Alias("npc pre")]
    [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
    [RequireOwner(Group = "Permission")]
    public class NpcPresetModule : ModuleBase<SocketCommandContext>
    {
        private readonly HelpService _helpService;
        private readonly NpcPresetService _presetService;
        private readonly SkillsService _skillsService;
        private readonly SpecialService _specialService;

        public NpcPresetModule(HelpService helpService, NpcPresetService presetService, SkillsService skillsService, SpecialService specialService)
        {
            _helpService = helpService;
            _presetService = presetService;
            _skillsService = skillsService;
            _specialService = specialService;
        }

        [Command]
        [Alias("help")]
        public async Task ShowNpcPresetHelp()
        {
            await _helpService.ShowNpcPresetHelpAsync(Context);
        }

        [Command("create")]
        public async Task CreatePreset(string name)
        {
            try
            {
                await _presetService.CreateNpcPreset(name);
            }
            catch (Exception e)
            {
                await ReplyAsync(Messages.FAILURE_EMOJI + e.Message + Context.User.Mention);
            }
        }

        [Command("toggle")]
        public async Task TogglePreset(string name)
        {
            var preset = await _presetService.GetNpcPreset(name);
            if (preset == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_NPC_PRESET_NOT_FOUND, Context.User.Mention));
                return;
            }

            preset.Enabled = !preset.Enabled;
            await _presetService.SaveNpcPreset(preset);
        }

        [Command("edit")]
        public async Task EditPreset(string name, Globals.SkillType skill, int value)
        {
            var preset = await _presetService.GetNpcPreset(name);

            if (preset == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_NPC_PRESET_NOT_FOUND, Context.User.Mention));
                return;
            }

            _skillsService.SetSkill(preset.Skills, skill, value);
            await _presetService.SaveNpcPreset(preset);

            await ReplyAsync(String.Format(Messages.NPC_PRESET_EDIT, name, skill.ToString(), value, Context.User.Mention));
        }

        [Command("edit")]
        public async Task EditPreset(string name, Globals.SpecialType special, int value)
        {
            var preset = await _presetService.GetNpcPreset(name);

            if (preset == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_NPC_PRESET_NOT_FOUND, Context.User.Mention));
                return;
            }

            _specialService.SetSpecial(preset.Special, special, value);
            await _presetService.SaveNpcPreset(preset);

            await ReplyAsync(String.Format(Messages.NPC_PRESET_EDIT_SPECIAL, name, Context.User.Mention));
        }

        [Command("edit")]
        public async Task EditPreset(string name, int str, int per, int end, int cha, int @int, int agi, int luc)
        {
            var preset = await _presetService.GetNpcPreset(name);

            if (preset == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_NPC_PRESET_NOT_FOUND, Context.User.Mention));
                return;
            }

            preset.Special = new Special { Strength = str, Perception = per, Endurance = end, Charisma = cha, Intelligence = @int, Agility = agi, Luck = luc };
            await _presetService.SaveNpcPreset(preset);

            await ReplyAsync(String.Format(Messages.NPC_PRESET_EDIT_SPECIAL, name, Context.User.Mention));
        }

        [Command("view")]
        public async Task ViewPresetInfo(string name)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            NpcPreset preset = await _presetService.GetNpcPreset(name);

            if (preset == null)
                await dmChannel.SendMessageAsync(String.Format(Messages.ERR_NPC_PRESET_NOT_FOUND, name, Context.User.Mention));

            StringBuilder sb = new StringBuilder();

            sb.Append("**S.P.E.C.I.A.L.:**\n");
            foreach (var special in Globals.SPECIAL_PROPER_NAMES)
                sb.Append($"**{special.Value}:** {_specialService.GetSpecial(preset.Special, special.Key)}\n");

            sb.Append("\n**Skills:**\n");
            foreach (var skill in Globals.SKILL_PROPER_NAMES)
            {
                var skillValue = _skillsService.GetSkill(preset.Skills, skill.Key);

                if (skillValue == 0)
                    continue;

                sb.Append($"**{skill.Value}:** {skillValue}\n");
            }

            await dmChannel.SendMessageAsync(Context.User.Mention, embed: EmbedHelper.BuildBasicEmbed($"Preset info for {preset.Name}:", sb.ToString()));
        }
    }
}
