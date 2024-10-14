using Microsoft.Xna.Framework;

namespace Broken.Entities
{
    public static class GameContext
    {
        public static Player Player;
        public static CharacterList ActiveCharacters = new();
        public static Room CurrentRoom;

        public static Vector2 CurrentRoomBounds
        {
            get
            {
                if (CurrentRoom == null)
                {
                    return new Vector2(1900, 600);
                }
                else
                {
                    return CurrentRoom.Bounds;
                }
            }
        }
        public static Vector2 PlayerPosition
        {
            get
            {
                return Player.Position;
            }
        }

        public static float GetRoomDepth(BoundingRectangle collider)
        {
            var height = CurrentRoomBounds.Y;
            var center = collider.Y + collider.Height;

            float depth = 1f - (center / height);
            if (depth > .9f)
            {
                depth = .9f;
            }

            if (depth < .1f)
            {
                depth = .1f;
            }
            return depth;
        }

    }
}
