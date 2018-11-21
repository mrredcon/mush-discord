namespace FalloutRPG.Data.Models.Characters
{
    public class NpcPreset : BaseModel
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public virtual SkillSheet Skills { get; set; }
        public virtual Special Special { get; set; }
    }
}
