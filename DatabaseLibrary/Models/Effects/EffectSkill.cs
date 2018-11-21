using FalloutRPG.Constants;

namespace FalloutRPG.Data.Models.Effects
{
    public class EffectSkill : BaseModel
    {
        public Globals.SkillType Skill { get; set; }
        public int EffectValue { get; set; }
    }
}
