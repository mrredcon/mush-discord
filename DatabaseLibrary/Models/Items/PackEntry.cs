using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FalloutRPG.Data.Models
{
    public class PackEntry : Item
    {
        public virtual Item Item { get; set; }
        public int Quantity { get; set; }

        public double PercentChance { get; set; }
    }
}
