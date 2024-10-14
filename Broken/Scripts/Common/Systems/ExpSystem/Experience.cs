using Microsoft.Xna.Framework;
using System.Diagnostics;
using Broken.Entities;

namespace Broken
{
    public struct Experience
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float MoveDuration;
        public float MoveTime;
        public bool IsMoving => MoveTime < MoveDuration;
        public bool IsVisible;
        public bool IsNearPlayer;

        public int CurrentFrame;
        public float FrameTime;
        private const float FrameDuration = 0.1f;
        private const int TotalFrames = 7;

        public Experience(Vector2 position, Vector2 velocity, float moveDuration)
        {
            Position = position;
            Velocity = velocity;
            MoveDuration = moveDuration;
            MoveTime = 0f;
            IsVisible = false;
            IsNearPlayer = false;

            CurrentFrame = 0;
            FrameTime = 0f;
        }

        public void Update(float deltaTime)
        {
            if (IsMoving)
            {
                Position += Velocity * deltaTime;
                MoveTime += deltaTime;
            }
            else
            {
                var player = GameContext.Player;
                if (!IsNearPlayer)
                {
                    var distance = Vector2.Distance(Position, player.CenterFloorPosition);
                    IsNearPlayer = distance < player.Status.ExperienceGatherRange;
                    
                }
                else
                {
                    Velocity = DirectionHelper.GetDirectionalVelocity(Velocity, DirectionHelper.GetDirection(Position, player.CenterFloorPosition));
                    Position += Velocity * deltaTime * 700;
                }
            }

            FrameTime += deltaTime;
            if (FrameTime >= FrameDuration)
            {
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
            IsVisible = true;
            IsNearPlayer = false;

            CurrentFrame = 0;
            FrameTime = 0f;
        }
    }

}
