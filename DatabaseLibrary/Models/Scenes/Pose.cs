using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Data.Models
{
    public class Pose : BaseModel
    {
        public string OwnerName { get; set; }

        public string Content { get; set; }
    }
}
