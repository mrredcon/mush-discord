using Discord;
using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Data.Models;
using FalloutRPG.Services.Roleplay;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("item create"), Alias("items create")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class ItemCreateModule : ModuleBase<SocketCommandContext>
    {
        private readonly ItemService _itemService;

        public ItemCreateModule(ItemService itemService)
        {
            _itemService = itemService;
        }

        private async Task<bool> ItemExists(string itemName)
        {
            if (await _itemService.GetItemAsync(itemName) is Item)
            {
                await ReplyAsync(String.Format(Messages.ERR_ITEM_EXISTS, Context.User.Mention));
                return true;
            }

            return false;
        }

        [Command("ammo")]
        public async Task CreateItemAmmoAsync(string name, string desc, int value, double weight) =>
            await CreateItemAmmoAsync(name, desc, value, weight, 1, 0);

        [Command("ammo")]
        public async Task CreateItemAmmoAsync(string name, string desc, int value, double weight, double dtMult, int dtReduction)
        {
            if (await ItemExists(name))
                return;

            await _itemService.AddItemAsync(
                new ItemAmmo
                {
                    Name = name,
                    Description = desc,
                    Value = value,
                    Weight = weight,
                    DTMultiplier = dtMult,
                    DTReduction = dtReduction
                });

            await ReplyAsync(String.Format(Messages.ITEM_CREATE_SUCCESS, name, "Ammo", Context.User.Mention));
        }

        [Command("apparel")]
        public async Task CreateItemApparelAsync(string name, string desc, int value, double weight, string slot, int dt)
        {
            if (await ItemExists(name))
                return;

            if (Enum.TryParse(slot, true, out ApparelSlot appSlot))
            {
                await _itemService.AddItemAsync(
                    new ItemApparel
                    {
                        Name = name,
                        Description = desc,
                        Value = value,
                        Weight = weight,
                        ApparelSlot = appSlot,
                        DamageThreshold = dt
                    });

                await ReplyAsync(String.Format(Messages.ITEM_CREATE_SUCCESS, name, "Ammo", Context.User.Mention));
            }
            else
                await ReplyAsync(String.Format(Messages.ERR_ITEM_INVALID_SLOT, Context.User.Mention));
        }

        [Command("consumable")]
        public async Task CreateItemConsumableAsync(string name, string desc, int value, double weight)
        {
            if (await ItemExists(name))
                return;

            await _itemService.AddItemAsync(
                new ItemConsumable
                {
                    Name = name,
                    Description = desc,
                    Value = value,
                    Weight = weight
                });

            await ReplyAsync(String.Format(Messages.ITEM_CREATE_SUCCESS, name, "Consumable", Context.User.Mention));
        }

        [Command("misc")]
        public async Task CreateItemMiscAsync(string name, string desc, int value, double weight)
        {
            if (await ItemExists(name))
                return;

            await _itemService.AddItemAsync(
                new ItemMisc
                {
                    Name = name,
                    Description = desc,
                    Value = value,
                    Weight = weight
                });

            await ReplyAsync(String.Format(Messages.ITEM_CREATE_SUCCESS, name, "Misc", Context.User.Mention));
        }

        [Command("pack")]
        public async Task CreateItemPackAsync(string name, string desc, int value, double weight)
        {
            if (await ItemExists(name))
                return;

            await _itemService.AddItemAsync(
                new ItemPack
                {
                    Name = name,
                    Description = desc,
                    Value = value,
                    Weight = weight,
                    ItemChances = new List<PackEntry>()
                });

            await ReplyAsync(String.Format(Messages.ITEM_CREATE_SUCCESS, name, "Pack", Context.User.Mention));
        }

        [Command("weapon")]
        public async Task CreateItemWeaponAsync(string name, string desc, int value, double weight, int damage,
            Globals.SkillType skill, int skillMin, string ammo, int ammoCapacity, int ammoOnAttack)
        {
            if (await ItemExists(name))
                return;

            Item item = await _itemService.GetItemAsync(ammo);

            if (item is ItemAmmo ammoItem)
            {
                var weapon = new ItemWeapon
                {
                    Name = name,
                    Description = desc,
                    Value = value,
                    Weight = weight,
                    Damage = damage,
                    Skill = skill,
                    SkillMinimum = skillMin,
                    AmmoCapacity = ammoCapacity,
                    AmmoOnAttack = ammoOnAttack,
                    AmmoRemaining = ammoCapacity
                };
                weapon.Ammo.Add(ammoItem);
                await _itemService.AddItemAsync(weapon);

                await ReplyAsync(String.Format(Messages.ITEM_CREATE_SUCCESS, name, "Weapon", Context.User.Mention));
            }
            else
            {
                await ReplyAsync(String.Format(Messages.ERR_ITEM_INVALID_AMMO, Context.User.Mention));
            }
        }
    }
}
