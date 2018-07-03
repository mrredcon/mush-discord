using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Models.Encounters
{
    public class EnemyEncounter : BaseEncounter
    {
        public string EnemyName { get; set; }
        public int Level { get; set; }
        public int Charisma { get; set; }
        public int Barter { get; set; }
    }
}
