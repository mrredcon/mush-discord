using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FalloutRPG.Models
{
    public abstract class Character : BaseModel
    {
        public string Name { get; set; }

        public virtual Campaign Campaign { get; set; }

        public int Experience { get; set; }
        [NotMapped]
        public int Level
        {
            get
            {
                return Experience / 1000 + 1;
            }
        }

        public virtual Special Special { get; set; }
        public virtual SkillSheet Skills { get; set; }

        public int HitPoints { get; set; }
        public int HitPointsLimit { get; set; }

        public int ArmorClass { get; set; }

        public long Money { get; set; }

        public virtual List<Effect> Effects { get; set; }

        public virtual List<Item> Inventory { get; set; }
    }
}
