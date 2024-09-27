using System.Collections.Generic;

namespace Broken.Scripts.Models
{
    public class RoomConfig
    {
        public int ID { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }
        public List<string> Overlaps { get; set; }
        public List<RectangleEntity> WallColliders { get; set; }
        public List<Vector2Entity> TorchLocations { get; set; }
    }
}
