using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Models
{
    public class ItemPack : Item
    {
        public virtual ICollection<PackEntry> ItemChances { get; set; }
    }
}
