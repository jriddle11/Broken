using Microsoft.Xna.Framework;
using Broken.Scripts;
using Broken.Scripts.Models;
using System.Collections.Generic;

namespace Broken.Scripts
{
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

        public BoundingRectangle(RectangleEntity entity)
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

        public static List<RectangleEntity> ConvertToEntities(List<BoundingRectangle> list)
        {
            List<RectangleEntity> entities = new();
            foreach(BoundingRectangle rect in list) { entities.Add(ConvertToEntity(rect)); }
            return entities;
        }

        public static RectangleEntity ConvertToEntity(BoundingRectangle rect)
        {
            return new RectangleEntity() { X = rect.X, Y = rect.Y, Height = rect.Height, Width = rect.Width };
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
