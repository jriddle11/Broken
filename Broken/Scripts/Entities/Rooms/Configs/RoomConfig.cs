using System.Collections.Generic;

namespace Broken.Entities
{
    public class RoomConfig
    {
        public int ID { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }
        public List<string> Overlaps { get; set; }
        public List<RectangleRecord> WallColliders { get; set; }
        public List<Vector2Record> TorchLocations { get; set; }
    }
}
