using Discord;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Helpers;
using FalloutRPG.Services;
using FalloutRPG.Services.Roleplay;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("character")]
    [Alias("char")]
    [Ratelimit(Globals.RATELIMIT_TIMES, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
    public class SkillsModule : ModuleBase<SocketCommandContext>
    {
        [Group("skills")]
        [Alias("skill", "sk")]
        public class CharacterSkillsModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;
            private readonly SkillsService _skillsService;
            private readonly HelpService _helpService;

            public CharacterSkillsModule(
                CharacterService charService,
                SkillsService skillsService,
                HelpService helpService)
            {
                _charService = charService;
                _skillsService = skillsService;
                _helpService = helpService;
            }

            [Command]
            [Alias("show", "view")]
            public async Task ShowSkillsAsync(IUser targetUser = null)
            {
                var userInfo = Context.User;
                var character = targetUser == null
                    ? await _charService.GetPlayerCharacterAsync(userInfo.Id)
                    : await _charService.GetPlayerCharacterAsync(targetUser.Id);

                if (character == null)
                {
                    await ReplyAsync(
                        string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                StringBuilder sb = new StringBuilder($"**Name:** {character.Name}\n");

                sb.Append("\n**Skills:**\n");
                foreach (var entry in Globals.SKILL_PROPER_NAMES)
                {
                    var skillValue = _skillsService.GetSkill(character, entry.Key);

                    if (skillValue == 0)
                        continue;

                    sb.Append($"**{entry.Value}:** {skillValue}\n");
                }

                var embed = EmbedHelper.BuildBasicEmbed("Command: $character skills", sb.ToString());

                await ReplyAsync(userInfo.Mention, embed: embed);
            }

            [Command("help")]
            [Alias("help")]
            public async Task ShowSkillsHelpAsync()
            {
                await _helpService.ShowSkillsHelpAsync(Context);
            }

            [Command("set")]
            [Alias("tag")]
            public async Task SetSkillsAsync(Globals.SkillType tag, int points)
            {
                var userInfo = Context.User;
                var character = await _charService.GetPlayerCharacterAsync(userInfo.Id);

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (_skillsService.AreSkillsTagged(character))
                {
                    await ReplyAsync(string.Format(Messages.ERR_SKILLS_ALREADY_SET, userInfo.Mention));
                    return;
                }

                try
                {
                    await _skillsService.TagSkill(character, tag, points);
                    await ReplyAsync(string.Format(Messages.SKILLS_SET_SUCCESS, userInfo.Mention));
                }
                catch (Exception e)
                {
                    await ReplyAsync($"{Messages.FAILURE_EMOJI} {e.Message} ({userInfo.Mention})");
                }
            }

            [Command("spend")]
            [Alias("put")]
            public async Task SpendSkillPointsAsync(Globals.SkillType skill, int points)
            {
                var userInfo = Context.User;
                var character = await _charService.GetPlayerCharacterAsync(userInfo.Id);

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (points < 1)
                {
                    await ReplyAsync(string.Format(Messages.ERR_SKILLS_POINTS_BELOW_ONE, userInfo.Mention));
                    return;
                }

                try
                {
                    _skillsService.PutPointsInSkill(character, skill, points);
                    await ReplyAsync(string.Format(Messages.SKILLS_SPEND_POINTS_SUCCESS, userInfo.Mention));
                }
                catch (Exception e)
                {
                    await ReplyAsync($"{Messages.FAILURE_EMOJI} {e.Message} ({userInfo.Mention})");
                }
            }
        }
    }
}
