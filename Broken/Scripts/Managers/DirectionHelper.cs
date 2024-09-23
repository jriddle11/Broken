using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


namespace Broken
{
    public static class DirectionHelper
    {
        public static Direction GetDirection(Vector2 origin, Vector2 destination)
        {
            Vector2 directionVector = destination - origin;
            float angle = (float)Math.Atan2(directionVector.Y, directionVector.X);
            float angleInDegrees = MathHelper.ToDegrees(angle);

            if (angleInDegrees < 0)
            {
                angleInDegrees += 360;
            }

            if (angleInDegrees >= 337.5 || angleInDegrees < 22.5)
            {
                return Direction.Right;
            }
            else if (angleInDegrees >= 22.5 && angleInDegrees < 67.5)
            {
                return Direction.DRight;
            }
            else if (angleInDegrees >= 67.5 && angleInDegrees < 112.5)
            {
                return Direction.Down;
            }
            else if (angleInDegrees >= 112.5 && angleInDegrees < 157.5)
            {
                return Direction.DLeft;
            }
            else if (angleInDegrees >= 157.5 && angleInDegrees < 202.5)
            {
                return Direction.Left;
            }
            else if (angleInDegrees >= 202.5 && angleInDegrees < 247.5)
            {
                return Direction.ULeft;
            }
            else if (angleInDegrees >= 247.5 && angleInDegrees < 292.5)
            {
                return Direction.Up;
            }
            else // if (angleInDegrees >= 292.5 && angleInDegrees < 337.5)
            {
                return Direction.URight;
            }
        }

        public static Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Direction.Left;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Up;
                case Direction.DRight:
                    return Direction.ULeft;
                case Direction.ULeft:
                    return Direction.DRight;
                case Direction.DLeft:
                    return Direction.URight;
                case Direction.URight:
                    return Direction.DLeft;
                default:
                    return Direction.Down;
            }
        }
    }
}
