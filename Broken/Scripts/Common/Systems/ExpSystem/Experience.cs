using Microsoft.Xna.Framework;

namespace Broken
{
    public struct Experience
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float MoveDuration;  // How long the experience moves
        public float MoveTime;      // How much time has passed moving
        public bool IsMoving => MoveTime < MoveDuration;

        // Animation-related
        public int CurrentFrame;
        public float FrameTime;     // How long since the last frame update
        private const float FrameDuration = 0.1f;  // 0.15 seconds per frame
        private const int TotalFrames = 7;          // Total number of frames in the animation

        public Experience(Vector2 position, Vector2 velocity, float moveDuration)
        {
            Position = position;
            Velocity = velocity;
            MoveDuration = moveDuration;
            MoveTime = 0f;

            // Initialize animation
            CurrentFrame = 0;
            FrameTime = 0f;
        }

        public void Update(float deltaTime)
        {
            // Handle movement
            if (IsMoving)
            {
                Position += Velocity * deltaTime;
                MoveTime += deltaTime;
            }

            // Handle animation
            FrameTime += deltaTime;
            if (FrameTime >= FrameDuration)
            {
                // Move to the next frame
                CurrentFrame = (CurrentFrame + 1) % TotalFrames;
                FrameTime = 0f;
            }
        }

        public void Reset(Vector2 position, Vector2 velocity, float moveDuration)
        {
            Position = position;
            Velocity = velocity;
            MoveDuration = moveDuration;
            MoveTime = 0f;

            // Reset animation
            CurrentFrame = 0;
            FrameTime = 0f;
        }
    }
}
