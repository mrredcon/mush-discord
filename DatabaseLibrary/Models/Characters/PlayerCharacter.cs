using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Models
{
    public class PlayerCharacter : Character
    {
        protected PlayerCharacter() { }

        public PlayerCharacter(Player player)
        {
            Player = player;
        }

        public virtual Player Player { get; set; }

        public bool Active { get; set; }

        public string Description { get; set; }
        public string Story { get; set; }

        public int ExperiencePoints { get; set; }
        public int TagPoints { get; set; }
        public bool IsReset { get; set; }
    }
}
