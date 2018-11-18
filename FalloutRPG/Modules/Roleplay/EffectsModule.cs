using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Helpers;
using FalloutRPG.Services.Roleplay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("effects")]
    [Alias("effect")]
    public class EffectsModule : ModuleBase
    {
        private readonly EffectsService _effectsService;

        public EffectsModule(EffectsService effectsService)
        {
            _effectsService = effectsService;
        }

        [Command("create")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task CreateEffectAsync(string name)
        {
            await ReplyAsync("This doesn't do anything right now!");
        }
    }

    [Group("character")]
    [Alias("char")]
    public class CharacterEffectsModule : ModuleBase
    {
        private readonly CharacterService _charService;
        private readonly EffectsService _effectsService;

        public CharacterEffectsModule(CharacterService charService, EffectsService effectsService)
        {
            _charService = charService;
            _effectsService = effectsService;
        }

        [Command("effects")]
        [Alias("effect", "wounds", "wound", "buffs", "buff", "debuffs", "debuff")]
        public async Task ShowCharacterEffectsAsync()
        {
            var userInfo = Context.User;
            var character = await _charService.GetPlayerCharacterAsync(userInfo.Id);

            if (character == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                return;
            }

            string info = _effectsService.GetCharacterEffects(character);

            await ReplyAsync(userInfo.Mention, embed: EmbedHelper.BuildBasicEmbed($"{character.Name}'s Effects:", info));
        }
    }
}
