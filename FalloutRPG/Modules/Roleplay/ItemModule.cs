using Discord;
using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Data.Models;
using FalloutRPG.Services.Roleplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("item")]
    [Alias("items")]
    public class ItemModule : ModuleBase<SocketCommandContext>
    {
        private readonly CharacterService _characterService;
        private readonly ItemService _itemService;

        private readonly Random _random;

        public ItemModule(CharacterService characterService, ItemService itemService, Random random)
        {
            _characterService = characterService;
            _itemService = itemService;

            _random = random;
        }

        [Command("info")]
        public async Task ViewItemInfoAsync([Remainder]string itemName)
        {
            var item = await _itemService.GetItemAsync(itemName);

            if (item == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_ITEM_NOT_FOUND, Context.User.Mention));
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"**Description:** {item.Description}\n" +
                $"**Value:** {item.Value}\n" +
                $"**Weight:** {item.Weight} lbs\n" +
                $"**Effects:** {String.Join(", ", item.Effects)}\n");

            if (item is ItemWeapon wep)
            {
                sb.Append($"**Damage:** {wep.Damage}\n");
                sb.Append($"**Ammo Type:** {String.Join(", ", wep.Ammo)}\n");
                sb.Append($"**Capacity:** {wep.AmmoCapacity}\n");
                sb.Append($"**Ammo Usage:** {wep.AmmoOnAttack}/Attack\n");
            }
            else if (item is ItemAmmo ammo)
            {
                if (ammo.DTMultiplier != 1)
                    sb.Append($"**DT Multiplier:** {ammo.DTMultiplier}\n");
                if (ammo.DTReduction != 0)
                    sb.Append($"**DT Reduction:** {ammo.DTReduction}\n");
            }
            else if (item is ItemApparel apparel)
            {
                sb.Append($"**Damage Threshold:** {apparel.DamageThreshold}\n");
                sb.Append($"**Apparel Slot:** {apparel.ApparelSlot}\n");
            }

            await ReplyAsync(embed: Helpers.EmbedHelper.BuildBasicEmbed(item.Name, sb.ToString()), message: Context.User.Mention);
        }

        [Command("addammo")]
        [Alias("ammoadd", "add ammo", "ammo add")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddAmmoAsync(string weaponName, string ammoName)
        {
            Item weaponItem = await _itemService.GetItemAsync(weaponName);
            Item ammoItem = await _itemService.GetItemAsync(ammoName);

            if (weaponItem is ItemWeapon weapon && ammoItem is ItemAmmo ammo)
            {
                weapon.Ammo.Add(ammo);
                await _itemService.SaveItemAsync(ammo);
                await ReplyAsync(Messages.SUCCESS_EMOJI + " done successfully baby");
            }
            else
            {
                await ReplyAsync("invalid weapon or ammo");
            }
        }

        [Command("assign")]
        [Alias("pack assign")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AssignItemAsync(string itemName, string packName, double percentChance)
        {
            if (!(await _itemService.GetItemAsync(packName) is ItemPack pack))
            {
                await ReplyAsync("pack not found ");
                return;
            }

            var item = await _itemService.GetItemAsync(itemName);
            if (item == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_ITEM_NOT_FOUND, Context.User.Mention));
                return;
            }

            pack.ItemChances.Add(new PackEntry() { Item = item, PercentChance = percentChance });
            await _itemService.SaveItemAsync(pack);
            await ReplyAsync("done assigning pack tinhgy");
        }

        [Command("unassign")]
        [Alias("pack unassign")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task UnassignItemAsync(string itemName, string packName, double percentChance)
        {
            if (!(await _itemService.GetItemAsync(packName) is ItemPack pack))
            {
                await ReplyAsync(String.Format(Messages.ERR_ITEM_NOT_FOUND, Context.User.Mention));
                return;
            }

            var temp = await _itemService.GetItemAsync(itemName);

            // validate input; check if itemName matches a valid item and check if the pack contains an entry with said item
            if (!(temp is Item item) || !(pack.ItemChances.Where(x => x.Item.Equals(item)).First() is PackEntry match))
            {
                await ReplyAsync(String.Format(Messages.ERR_ITEM_NOT_FOUND, Context.User.Mention));
                return;
            }

            pack.ItemChances.Remove(match);
            await _itemService.SaveItemAsync(pack);
            await ReplyAsync(String.Format(Messages.ITEM_PACK_UNASSIGN, item.Name, Context.User.Mention));
        }

        [Command("open")]
        [Alias("pack open")]
        public async Task OpenPackAsync(string packName)
        {
            var character = await _characterService.GetPlayerCharacterAsync(Context.User.Id);
            if (character == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_CHAR_NOT_FOUND, Context.User.Mention));
                return;
            }

            var pack = await _itemService.GetItemAsync(packName) as ItemPack;
            if (!(pack is ItemPack) || !character.Inventory.Contains(pack))
            {
                await ReplyAsync(String.Format(Messages.ERR_ITEM_NOT_FOUND, Context.User.Mention));
                return;
            }

            var converted = new List<PackEntry>(pack.ItemChances.Count);

            var sum = 0.0;

            var probabilityTotal = pack.ItemChances.Sum(x => x.PercentChance);
            // https://stackoverflow.com/revisions/46735565/2
            foreach (var item in pack.ItemChances.Take(pack.ItemChances.Count - 1))
            {
                sum += item.PercentChance / probabilityTotal;
                converted.Add(new PackEntry { PercentChance = sum, Item = item.Item });
            }
            converted.Add(new PackEntry { PercentChance = 1.0, Item = pack.ItemChances.Last().Item });

            double rng = _random.NextDouble();

            var selected = converted.SkipWhile(i => i.PercentChance < rng).First();

            character.Inventory.Remove(pack);
            for (int quantity = 0; quantity < selected.Quantity; quantity++)
                character.Inventory.Add(selected);
            await _characterService.SaveCharacterAsync(character);

            await ReplyAsync(String.Format(Messages.ITEM_PACK_OPEN, selected.Name, selected.Quantity, Context.User.Mention));
        }
    }
}
