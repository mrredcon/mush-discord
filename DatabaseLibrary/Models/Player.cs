namespace FalloutRPG.Data.Models
{
    public class Player : BaseModel
    {
        protected Player() { }

        public Player(ulong discordId)
        {
            DiscordId = discordId;
        }

        public ulong DiscordId { get; private set; }
    }
}
