using FalloutRPG.Constants;

namespace FalloutRPG.Data.Models
{
    public class EffectSpecial : BaseModel
    {
        public Globals.SpecialType SpecialAttribute { get; set; }
        public int EffectValue { get; set; }
    }
}
