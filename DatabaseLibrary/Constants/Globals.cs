using System;
using System.Collections.Generic;

namespace FalloutRPG.Constants
{
    public class Globals
    {
        public enum SkillType
        {
            // Strength
            Archery, Athletics, Construction, HeavyWeapons, Intimidation, Melee, Unarmed,
            // Perception
            Alertness, Art, Brewing, Cooking, Deception, Explosives, FirstAid, Gunsmith, Husbandry, Investigation, Music, Repair, Security,
            // Endurance
            Blacksmith, Resistance, Survival, Toughness,
            // Charisma
            Barter, Command, Courage, Manipulation, Performance, Persuasion, Seduction, Streetwise,
            // Intelligence
            EnergyWeapons, Hacking, History, Literature, Medicine, Pharmaceuticals, Science, Tactics, Technology,
            // Agility
            Acrobatics, Dodge, Drive, Firearms, Pilot, Riding, Stealth, Tailoring,
            // Luck
            Escape, Gamble, ImprovisedWeapons, JuryRig, Scavenge
        }

        public enum SpecialType
        {
            Strength,
            Perception,
            Endurance,
            Charisma,
            Intelligence,
            Agility,
            Luck
        }

        public enum ItemType
        {
            Ammo,
            Apparel,
            Consumable,
            Misc,
            Weapon
        }

        public static readonly Dictionary<string, SkillType> SKILL_ALIASES = new Dictionary<string, SkillType>(StringComparer.OrdinalIgnoreCase)
        {
            // Strength
            { "Archery", SkillType.Archery },
            { "Archer", SkillType.Archery },

            { "Athletics", SkillType.Athletics },

            { "Construction", SkillType.Construction },
            { "Building", SkillType.Construction },

            { "Heavy Weapons", SkillType.HeavyWeapons },
            { "HeavyWeapons", SkillType.HeavyWeapons },

            { "Intimidation", SkillType.Intimidation },

            { "Melee", SkillType.Melee },
            { "Melee Weapons", SkillType.Melee },

            { "Unarmed", SkillType.Unarmed },
            { "Hand to hand", SkillType.Unarmed },
            { "Hand-to-hand", SkillType.Unarmed },
            { "Fists", SkillType.Unarmed },

            // Perception
            { "Alertness", SkillType.Alertness },
            { "Alert", SkillType.Alertness },

            { "Art", SkillType.Art },
 
            { "Brewing", SkillType.Brewing },
            { "Brew", SkillType.Brewing },

            { "Cooking", SkillType.Cooking },
            { "Cook", SkillType.Cooking },

            { "Deception", SkillType.Deception },
            { "Decept", SkillType.Deception },

            { "Explosives", SkillType.Explosives },

            { "First Aid", SkillType.FirstAid },
            { "FirstAid", SkillType.FirstAid },

            { "Gunsmith", SkillType.Gunsmith },
            { "Gunmaking", SkillType.Gunsmith },
            { "Gunbuilding", SkillType.Gunsmith },

            { "Husbandry", SkillType.Husbandry },

            { "Investigation", SkillType.Investigation },
            { "Investigate", SkillType.Investigation },

            { "Music", SkillType.Music },
 
            { "Repair", SkillType.Repair },
            { "Fix", SkillType.Repair },

            { "Security", SkillType.Security },
            { "Lockpick", SkillType.Security },
            { "Lockpicking", SkillType.Security },

            // Endurance
            { "Blacksmith", SkillType.Blacksmith },

            { "Resistance", SkillType.Resistance },
            { "Resist", SkillType.Resistance },

            { "Survival", SkillType.Survival },

            { "Toughness", SkillType.Toughness },
            { "Tough", SkillType.Toughness },

            // Charisma
            { "Barter", SkillType.Barter },
            { "Haggle", SkillType.Barter },
            { "Trading", SkillType.Barter },
            { "Trade", SkillType.Barter },

            { "Command", SkillType.Command },

            { "Courage", SkillType.Courage },

            { "Manipulation", SkillType.Manipulation },
            { "Manipulate", SkillType.Manipulation },

            { "Performance", SkillType.Performance },

            { "Persuasion", SkillType.Persuasion },
            { "Persuade", SkillType.Persuasion },

            { "Seduction", SkillType.Seduction },
            { "Seduce", SkillType.Seduction },

            { "Streetwise", SkillType.Streetwise },
            { "Street smarts", SkillType.Streetwise },
            { "Street smart", SkillType.Streetwise },

            // Intelligence
            { "EnergyWeapons", SkillType.EnergyWeapons },
            { "EnergyWeapon", SkillType.EnergyWeapons },
            { "Energy Weapons", SkillType.EnergyWeapons },
            { "Energy Weapon", SkillType.EnergyWeapons },
            { "Energy", SkillType.EnergyWeapons },

            { "Hacking", SkillType.Hacking },
            { "Hack", SkillType.Hacking },

            { "History", SkillType.History },

            { "Literature", SkillType.Literature },
            { "Books", SkillType.Literature },
            { "Book", SkillType.Literature },

            { "Medicine", SkillType.Medicine },

            { "Pharmaceuticals", SkillType.Pharmaceuticals },
            { "Drugs", SkillType.Pharmaceuticals },
            { "Drug", SkillType.Pharmaceuticals },

            { "Science", SkillType.Science },

            { "Tactics", SkillType.Tactics },

            { "Technology", SkillType.Technology },
            { "Tech", SkillType.Technology },

            // Agility
            { "Acrobatics", SkillType.Acrobatics },
            { "Acrobat", SkillType.Acrobatics },

            { "Dodge", SkillType.Dodge },
 
            { "Drive", SkillType.Drive },
            { "Driver", SkillType.Drive },
            { "Driving", SkillType.Drive },

            { "Firearms", SkillType.Firearms },
            { "Firearm", SkillType.Firearms },
            { "Guns", SkillType.Firearms },
            { "Gun", SkillType.Firearms },

            { "Pilot", SkillType.Pilot },
            { "Piloting", SkillType.Pilot },

            { "Riding", SkillType.Riding },

            { "Stealth", SkillType.Stealth },

            { "Tailoring", SkillType.Tailoring },

            // Luck
            { "Escape", SkillType.Escape },

            { "Gamble", SkillType.Gamble },

            { "ImprovisedWeapons", SkillType.ImprovisedWeapons },
            { "ImprovisedWeapon", SkillType.ImprovisedWeapons },
            { "Improvised Weapons", SkillType.ImprovisedWeapons },
            { "Improvised Weapon", SkillType.ImprovisedWeapons },
            { "Improvised Weaps", SkillType.ImprovisedWeapons },
            { "Improvised Weps", SkillType.ImprovisedWeapons },
            { "Improvised Weap", SkillType.ImprovisedWeapons },
            { "Improvised Wep", SkillType.ImprovisedWeapons },
            
            { "JuryRig", SkillType.JuryRig },
            { "Jury Rig", SkillType.JuryRig },
            { "Jury Rigging", SkillType.JuryRig },

            { "Scavenge", SkillType.Scavenge },
            { "Scavenging", SkillType.Scavenge },
        };

        public static readonly Dictionary<string, SpecialType> SPECIAL_ALIASES = new Dictionary<string, SpecialType>(StringComparer.OrdinalIgnoreCase)
        {
            { "Strength", SpecialType.Strength },
            { "STR", SpecialType.Strength },

            { "Perception", SpecialType.Perception },
            { "PER", SpecialType.Perception },

            { "Endurance", SpecialType.Endurance },
            { "END", SpecialType.Endurance },

            { "Charisma", SpecialType.Charisma },
            { "CHA", SpecialType.Charisma },

            { "Intelligence", SpecialType.Intelligence },
            { "INT", SpecialType.Intelligence },

            { "Agility", SpecialType.Agility },
            { "AGI", SpecialType.Agility },
            { "AGL", SpecialType.Agility },

            { "Luck", SpecialType.Luck },
            { "LCK", SpecialType.Luck },
            { "LUC", SpecialType.Luck },
        };

        public static string[] SKILL_NAMES = new string[]
        {
            // Strength
            "Archery", "Athletics", "Construction", "HeavyWeapons", "Intimidation", "Melee", "Unarmed",
            // Perception
            "Alertness", "Art", "Brewing", "Cooking", "Deception", "Explosives", "FirstAid", "Gunsmith", "Husbandry", "Investigation", "Music", "Repair", "Security",
            // Endurance
            "Blacksmith", "Resistance", "Survival", "Toughness",
            // Charisma
            "Barter", "Command", "Courage", "Manipulation", "Performance", "Persuasion", "Seduction", "Streetwise",
            // Intelligence
            "EnergyWeapons", "Hacking", "History", "Literature", "Medicine", "Pharmaceuticals", "Science", "Tactics", "Technology",
            // Agility
            "Acrobatics", "Dodge", "Drive", "Firearms", "Pilot", "Riding", "Stealth", "Tailoring",
            // Luck
            "Escape", "Gamble", "ImprovisedWeapons", "JuryRig", "Scavenge"
        };

        public static string[] SPECIAL_NAMES = new string[]
        {
            "Strength",
            "Perception",
            "Endurance",
            "Charisma",
            "Intelligence",
            "Agility",
            "Luck"
        };

        public const int RATELIMIT_SECONDS = 2;
        public const int RATELIMIT_TIMES = 3;
    }
}
