using System;

namespace FalloutRPG.Models.Characters
{
    public class Character : BaseModel
    {
        public ulong DiscordId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Description { get; set; }
        public string Story { get; set; }

        public int Experience { get; set; }
        public int Level
        {
            get
            {
                return Convert.ToInt32((Math.Sqrt(Experience + 125) / (10 * Math.Sqrt(5))));
            }
            private set { }
        }

        public Special Special { get; set; }
        public SkillSheet Skills { get; set; }
    }
}
