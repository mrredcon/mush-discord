using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Models.Encounters
{
    public abstract class BaseEncounter : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Choices { get; set; }
    }
}
