using FalloutRPG.Constants;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class ItemService
    {
        private readonly IRepository<Item> _itemRepo;

        public ItemService(IRepository<Item> itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task AddItemAsync(Item item) =>
            await _itemRepo.AddAsync(item);

        public async Task<Item> GetItemAsync(string name) =>
            await _itemRepo.Query.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();

        public async Task SaveItemAsync(Item item) =>
            await _itemRepo.SaveAsync(item);

        public List<Item> GetEquippedItems(Character character) =>
            character.Inventory.Where(x => x.Equipped == true).ToList();

        public int GetDamageThreshold(Character character) =>
            GetEquippedItems(character).OfType<ItemApparel>().Sum(x => x.DamageThreshold);

        public double GetDamageSkillMultiplier(ItemWeapon weapon, int skillValue)
        {
            double skillMultiplier = skillValue / weapon.SkillMinimum;
            
            if (skillMultiplier < 0.5)
                skillMultiplier = 0.5;
            else if (skillMultiplier > 1)
                skillMultiplier = 1;

            return skillMultiplier;
        }

        public bool HasAmmo(Character character, ItemWeapon weapon) =>
            character.Inventory.OfType<ItemAmmo>().Where(x => x.Equals(weapon.Ammo)).Count() >= weapon.AmmoOnAttack;

        public string GetCharacterInventory(Character character)
        {
            var inv = character.Inventory;

            StringBuilder sb = new StringBuilder();

            sb.Append("**Weapons:**\n");
            foreach (var item in inv.OfType<ItemWeapon>())
                sb.Append($"__*{item.Name}*__:\n" +
                    $"Damage: {item.Damage}\n" +
                    $"{item.Skill.ToString()} Skill: {item.SkillMinimum}\n" +
                    $"**Ammo Type:** {String.Join(", ", item.Ammo)}\n" +
                    $"Ammo Capacity: {item.AmmoCapacity}\n" +
                    $"Ammo Usage: {item.AmmoOnAttack}/Attack\n\n");

            sb.Append("**Apparel:**\n");
            foreach (var item in inv.OfType<ItemApparel>())
                sb.Append($"__*{item.Name}*__: DT {item.DamageThreshold}\n");

            sb.Append("**Consumables:**\n");
            foreach (var item in inv.OfType<ItemConsumable>().ToHashSet())
                sb.Append($"__*{item.Name}*__ x{inv.Count(x => x.Equals(item))}\n");

            sb.Append("**Miscellaneous:**\n");
            foreach (var item in inv.OfType<ItemMisc>())
                sb.Append($"__*{item.Name}*__\n");

            sb.Append("**Ammunition:**\n");
            foreach (var item in inv.OfType<ItemAmmo>().ToHashSet())
            {
                sb.Append($"__*{item.Name}:*__ x{inv.Count(x => x.Equals(item))}\n");
                if (item.DTMultiplier != 1)
                    sb.Append($"DT Multiplier: {item.DTMultiplier}\n");
                if (item.DTReduction != 0)
                    sb.Append($"DT Reduction: {item.DTReduction}\n");
            }

            return sb.ToString();
        }
    }
}
