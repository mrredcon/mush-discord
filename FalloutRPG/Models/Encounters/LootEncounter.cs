using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Models.Encounters
{
    public class LootEncounter : BaseEncounter
    {
        public List<string> LootItems { get; set; }
        public int LockpickRequired { get; set; }
    }
}
