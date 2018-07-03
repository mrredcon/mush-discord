using FalloutRPG.Models.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Models.Encounters
{
    public class EnemyEncounter : BaseEncounter
    {
        public string Name { get; set; }

        public int Level { get; set; }
        
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }

        public int CombatSkill { get; set; }
        public int Barter { get; set; }
        public int Speech { get; set; }
    }
}
