﻿namespace FalloutRPG.Data.Models.Items
{
    public class ItemAmmo : Item
    {
        public double DamageMultiplier { get; set; }

        public double DTMultiplier { get; set; }
        public int DTReduction { get; set; }
    }
}
