using FalloutRPG.Constants;
using System.Collections.Generic;

namespace FalloutRPG.Data.Models.Characters
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

        public List<(Globals.SkillType Skill, int Value)> Skills2 { get; set; }

        void test()
        {
            Skills2[Skills2.FindIndex(x => x.Skill == Globals.SkillType.Firearms)] = (Globals.SkillType.Firearms, 2);
        }
    }
}
