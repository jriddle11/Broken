using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Broken
{
    public class DrawRecord
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Color Color { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float LayerDepth { get; set; }
    }
}
