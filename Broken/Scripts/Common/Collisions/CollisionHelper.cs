using System;
using Microsoft.Xna.Framework;
using Broken.Scripts;

namespace Broken.Scripts
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects collision between 2 bounding circles
        /// </summary>
        /// <param name="a">The first circle</param>
        /// <param name="b">The second circle</param>
        /// <returns>returns true for collision</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects collision between 2 bounding rectangles
        /// </summary>
        /// <param name="a">The first rectangele</param>
        /// <param name="b">The second rectangle</param>
        /// <returns>returns true for collision</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom|| a.Bottom < b.Top);
        }

        /// <summary>
        /// Detects collision between circle and rectangles
        /// </summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>returns true if collision detected</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }

        /// <summary>
        /// Detects collision between circle and rectangles
        /// </summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>returns true if collision detected</returns>
        public static bool Collides(BoundingRectangle r, BoundingCircle c)
        {
            return Collides(c, r);
        }
    }
}
