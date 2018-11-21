using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FalloutRPG.Data.Models
{
    public class NpcPreset : BaseModel
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public virtual SkillSheet Skills { get; set; }
        public virtual Special Special { get; set; }
    }
}
