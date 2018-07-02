using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Models.Encounters
{
    public class DialogEncounter : BaseEncounter
    {
        public List<string> Choices { get; set; }
    }
}
