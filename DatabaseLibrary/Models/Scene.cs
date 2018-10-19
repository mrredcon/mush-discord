using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Models
{
    public class Scene : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public bool State { get; set; }

        public string OwnerName { get; set; }
        public int OwnerId { get; set; }

        public virtual ICollection<Pose> Poses { get; set; }
    }
}
