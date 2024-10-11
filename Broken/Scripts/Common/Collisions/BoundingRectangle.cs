using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Broken
{
    /// <summary>
    /// Struct representing rectangular bounds
    /// </summary>
    public struct BoundingRectangle
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        public float Left => X;

        public float Top => Y;

        public float Right => X + Width;

        public float Bottom => Y + Height;

        public BoundingRectangle(RectangleRecord entity)
        {
            X = entity.X;
            Y = entity.Y;
            Width = entity.Width;
            Height = entity.Height;
        }

        public BoundingRectangle(Rectangle rect)
        {
            X = rect.X;
            Y = rect.Y;
            Width= rect.Width;
            Height= rect.Height;
        }

        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        public static List<RectangleRecord> ConvertToEntities(List<BoundingRectangle> list)
        {
            List<RectangleRecord> entities = new();
            foreach(BoundingRectangle rect in list) { entities.Add(ConvertToEntity(rect)); }
            return entities;
        }

        public static RectangleRecord ConvertToEntity(BoundingRectangle rect)
        {
            return new RectangleRecord() { X = rect.X, Y = rect.Y, Height = rect.Height, Width = rect.Width };
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
    }
}
