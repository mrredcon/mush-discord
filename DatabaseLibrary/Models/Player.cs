using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
