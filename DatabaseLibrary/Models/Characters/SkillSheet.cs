using FalloutRPG.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FalloutRPG.Models
{
    public class SkillSheet : BaseModel
    {
        public int CharacterId { get; set; }

        // Strength
        public int Archery { get; set; }
        public int Athletics { get; set; }
        public int Construction { get; set; }
        public int HeavyWeapons { get; set; }
        public int Intimidation { get; set; }
        public int Melee { get; set; }
        public int Unarmed { get; set; }

        // Perception
        public int Alertness { get; set; }
        public int Art { get; set; }
        public int Brewing { get; set; }
        public int Cooking { get; set; }
        public int Deception { get; set; }
        public int Explosives { get; set; }
        public int FirstAid { get; set; }
        public int Gunsmith { get; set; }
        public int Husbandry { get; set; }
        public int Investigation { get; set; }
        public int Music { get; set; }
        public int Repair { get; set; }
        public int Security { get; set; }

        // Endurance
        public int Blacksmith { get; set; }
        public int Resistance { get; set; }
        public int Survival { get; set; }
        public int Toughness { get; set; }

        // Charisma
        public int Barter { get; set; }
        public int Command { get; set; }
        public int Courage { get; set; }
        public int Manipulation { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }
        public int Seduction { get; set; }
        public int Streetwise { get; set; }

        // Intelligence
        public int EnergyWeapons { get; set; }
        public int Hacking { get; set; }
        public int History { get; set; }
        public int Literature { get; set; }
        public int Medicine { get; set; }
        public int Pharmaceuticals { get; set; }
        public int Science { get; set; }
        public int Tactics { get; set; }
        public int Technology { get; set; }

        // Agility
        public int Acrobatics { get; set; }
        public int Dodge { get; set; }
        public int Drive { get; set; }
        public int Firearms { get; set; }
        public int Pilot { get; set; }
        public int Riding { get; set; }
        public int Stealth { get; set; }
        public int Tailoring { get; set; }

        // Luck
        public int Escape { get; set; }
        public int Gamble { get; set; }
        public int ImprovisedWeapons { get; set; }
        public int JuryRig { get; set; }
        public int Scavenge { get; set; }

        [NotMapped]
        public int[] SkillsArray
        {
            get
            {
                return new int[]
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
                };
            }
        }
    }
}
