using System.Collections.Generic;

namespace FalloutRPG.Data.Models.Items
{
    public class ItemPack : Item
    {
        public virtual ICollection<PackEntry> ItemChances { get; set; }
    }
}
