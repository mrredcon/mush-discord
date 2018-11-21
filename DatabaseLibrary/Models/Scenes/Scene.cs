using System.Collections.Generic;

namespace FalloutRPG.Data.Models.Scenes
{
    public class Scene : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public SceneState State { get; set; }

        public string OwnerName { get; set; }
        public int OwnerId { get; set; }

        public virtual ICollection<Pose> Poses { get; set; }
    }

    public enum SceneState
    {
        Active,
        Paused,
        Finished
    }
}
