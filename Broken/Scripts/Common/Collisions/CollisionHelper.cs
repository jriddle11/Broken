using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Broken.Entities;

namespace Broken
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

        /// <summary>
        /// Resolves all collisions between static objects and dynamic characters
        /// </summary>
        /// <param name="characters"></param>
        /// <param name="staticRects"></param>
        /// <param name="maxIterations">The number of times to resolve collisions per update per character</param>
        public static void ResolveCollisions(CharacterList characters, List<BoundingRectangle> staticRects, int maxIterations)
        {

            foreach (var character in characters)
            {

                for (int i = 0; i < maxIterations; i++)
                {
                    bool collisionResolved = true;

                    foreach (var staticRect in staticRects)
                    {
                        if (character.Collider.CollidesWith(staticRect))
                        {
                            SeparateFromStaticRect(character, staticRect);
                            collisionResolved = false;
                        }
                    }

                    foreach (var otherCharacter in characters)
                    {
                        if (otherCharacter == character) continue;

                        if (character.Collider.CollidesWith(otherCharacter.Collider))
                        {
                            SeparateFromOtherCharacter(character, otherCharacter);
                            collisionResolved = false;
                        }
                    }

                    if (collisionResolved) break;
                }
            }
        }

        /// <summary>
        /// Seperates a dynamic character from a static object
        /// </summary>
        /// <param name="character"></param>
        /// <param name="staticRect"></param>
        private static void SeparateFromStaticRect(Character character, BoundingRectangle staticRect)
        {
            var right = character.Collider.Right - staticRect.Left;
            var left = staticRect.Right - character.Collider.Left;
            var top = character.Collider.Bottom - staticRect.Top;
            var bottom = staticRect.Bottom - character.Collider.Top;

            float overlapX = Math.Min(right, left);
            float overlapY = Math.Min(top, bottom);

            int xSign = (overlapX == right) ? 1 : -1;
            int ySign = (overlapY == bottom) ? -1 : 1;

            float adjustmentFactor = 0.5f; 

            if (Math.Abs(overlapX) < Math.Abs(overlapY))
            {
                character.Position = new Vector2(character.Position.X - overlapX * adjustmentFactor * xSign, character.Position.Y);
                character.UpdateCollider();
            }

            else
            {
                character.Position = new Vector2(character.Position.X, character.Position.Y - overlapY * adjustmentFactor * ySign);
                character.UpdateCollider();
            }

            int iterations = 0;
            while (character.Collider.CollidesWith(staticRect) && iterations < 10)
            {
                iterations++;

                if (Math.Abs(overlapX) < Math.Abs(overlapY))
                {
                    character.Position = new Vector2(character.Position.X - xSign * 0.1f, character.Position.Y);
                    character.UpdateCollider();
                }
                else
                {
                    character.Position = new Vector2(character.Position.X, character.Position.Y - ySign * 0.1f);
                    character.UpdateCollider();
                }
            }
        }

        /// <summary>
        /// Seperates dynamic characters from eachother
        /// </summary>
        /// <param name="character"></param>
        /// <param name="otherCharacter"></param>
        private static void SeparateFromOtherCharacter(Character character, Character otherCharacter)
        {
            var right = character.Collider.Right - otherCharacter.Collider.Left;
            var left = otherCharacter.Collider.Right - character.Collider.Left;
            var top = character.Collider.Bottom - otherCharacter.Collider.Top;
            var bottom = otherCharacter.Collider.Bottom - character.Collider.Top;

            float overlapX = Math.Min(right, left);
            float overlapY = Math.Min(top, bottom);

            int xSign = (overlapX == right) ? 1 : -1;
            int ySign = (overlapY == bottom) ? -1 : 1;

            if (Math.Abs(overlapX) < Math.Abs(overlapY))
            {
                character.Position = new Vector2(character.Position.X - xSign * Math.Abs(overlapX) / 2, character.Position.Y);
                otherCharacter.Position = new Vector2(otherCharacter.Position.X + xSign * Math.Abs(overlapX) / 2, otherCharacter.Position.Y);
            }
            else
            {
                character.Position = new Vector2(character.Position.X, character.Position.Y - ySign * Math.Abs(overlapY) / 2);
                otherCharacter.Position = new Vector2(otherCharacter.Position.X, otherCharacter.Position.Y + ySign * Math.Abs(overlapY) / 2);
            }

            character.UpdateCollider();
            otherCharacter.UpdateCollider();
        }
    }
}
