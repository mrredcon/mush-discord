using FalloutRPG.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FalloutRPG.Models
{
    public class Special : BaseModel
    {
        public int CharacterId { get; set; }

        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }

        [NotMapped]
        public int[] SpecialArray
        {
            get
            {
                return new int[]
                    {
                        Strength,
                        Perception,
                        Endurance,
                        Charisma,
                        Intelligence,
                        Agility,
                        Luck
                    };
            }
        }
    }
}
