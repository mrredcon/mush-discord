﻿using FalloutRPG.Data.Models.Effects;
using System.Collections.Generic;

namespace FalloutRPG.Data.Models.Items
{
    public abstract class Item : BaseModel
    {
        public Item()
        {
            Effects = new List<Effect>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Value { get; set; }

        public double Weight { get; set; }

        public bool Equipped { get; set; }

        public virtual ICollection<Effect> Effects { get; set; }

        public override string ToString() => Name;
    }
}
