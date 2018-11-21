using FalloutRPG.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FalloutRPG.Data
{
    public class RpgContext : DbContext
    {
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Effect> Effects { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<NpcPreset> NpcPresets { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Scene> Scenes { get; set; }

        public RpgContext(DbContextOptions<RpgContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PlayerCharacter>();
            builder.Entity<ItemAmmo>();
            builder.Entity<ItemApparel>();
            builder.Entity<ItemConsumable>();
            builder.Entity<ItemMisc>();
            builder.Entity<ItemPack>();
            builder.Entity<ItemWeapon>();

            base.OnModelCreating(builder);
        }
    }
}
