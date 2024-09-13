using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Broken.Scripts.Common;

namespace Broken.Scripts.Models
{
    public class RoomConfig
    {
        public int ID { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }
        public List<RectangleEntity> WallColliders { get; set; }
    }
}
