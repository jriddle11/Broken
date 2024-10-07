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

        public static Direction GetDirectionCounterClockwise(Direction originalDirection)
        {
            switch (originalDirection)
            {
                case Direction.Left:
                    return Direction.DLeft;
                case Direction.DLeft:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.DRight;
                case Direction.DRight:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.URight;
                case Direction.URight:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.ULeft;
                case Direction.ULeft:
                    return Direction.Left;
                default:
                    return Direction.Down;
            }
        }

        public static Direction GetDirectionClockwise(Direction originalDirection)
        {
            switch (originalDirection)
            {
                case Direction.Left:
                    return Direction.ULeft;
                case Direction.ULeft:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.URight;
                case Direction.URight:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.DRight;
                case Direction.DRight:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.DLeft;
                case Direction.DLeft:
                    return Direction.Left;
                default:
                    return Direction.Down;
            }
        }

        public static Vector2 GetDirectionalVelocity(Vector2 startVelocity, Direction direction)
        {
            var velocity = startVelocity;
            switch (direction)
            {
                case Direction.Down:
                    velocity = new Vector2(0, .8f);
                    break;

                case Direction.Right:
                    velocity = new Vector2(1, 0);
                    break;

                case Direction.Up:
                    velocity = new Vector2(0, -.8f);
                    break;

                case Direction.DRight:
                    velocity = new Vector2(1, .8f);
                    velocity *= .8f;
                    break;

                case Direction.URight:
                    velocity = new Vector2(1, -.8f);
                    velocity *= .8f;
                    break;

                case Direction.Left:
                    velocity = new Vector2(-1, 0);
                    break;

                case Direction.DLeft:
                    velocity = new Vector2(-1, .8f);
                    velocity *= .8f;
                    break;

                case Direction.ULeft:
                    velocity = new Vector2(-1, -.8f);
                    velocity *= .8f;
                    break;
            }

            return velocity;
        }

        public static bool CheckIfAnimationShouldFlip(Direction direction, out int animationDirection)
        {
            animationDirection = (int)direction;
            switch (direction)
            {
                case Direction.Left:
                    animationDirection = 1;
                    return true;
                case Direction.DLeft:
                    animationDirection = 3;
                    return true;
                case Direction.ULeft:
                    animationDirection = 4;
                    return true;
            }
            return false;
        }
    }

    
}
