using Microsoft.Xna.Framework;
using Broken.Scripts.Interfaces;
using Broken.Scripts.Models;

namespace Broken.Scripts.Common
{
    public struct BoundingRectangle : ICollider
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

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }

        public bool CollidesWith(ICollider other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
