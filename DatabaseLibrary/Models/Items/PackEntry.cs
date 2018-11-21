namespace FalloutRPG.Data.Models.Items
{
    public class PackEntry : Item
    {
        public virtual Item Item { get; set; }
        public int Quantity { get; set; }

        public double PercentChance { get; set; }
    }
}
