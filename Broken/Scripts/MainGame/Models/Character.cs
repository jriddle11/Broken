using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.Common;
using Broken.Scripts.MainGame;
using System.Collections.Generic;
using System;

namespace Broken.Scripts
{
    public abstract class Character
    {
        public uint ID;
        public CharacterStats Stats;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Speed { get; set; } = 400f;
        public float Scale { get; set; } = .65f;
        public BoundingRectangle FloorCollider
        {
            get
            {
                return _floorCollider;
            }
            protected set
            {
                _floorCollider = value;
            }
        }
        public Vector2 FloorColliderOffset = Vector2.Zero;
        public bool FollowingThrough => _currentFreezeFrame < _totalFreezeFrames;
        public bool ActionEnding { get; protected set; }
        public virtual bool HasDamageCollision { get; protected set; } = true;
        public virtual bool TakingDamage => _takingDamage;

        protected Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }
        protected int CurrentAnimationFrame
        {
            get
            {
                return _currentFrame;
            }
            set
            {
                _currentFrame = value;
            }
        }
        protected int TotalAnimationFrames
        {
            get
            {
                return _totalFrames;
            }
            set
            {
                _totalFrames = value;
            }
        }
        protected int CurrentFreezeFrame
        {
            get
            {
                return _currentFreezeFrame;
            }
            set
            {
                _currentFreezeFrame = value;
            }
        }
        protected int TotalFreezeFrames
        {
            get
            {
                return _totalFreezeFrames;
            }
            set
            {
                _totalFreezeFrames = value;
            }
        }

        private static uint nextId = 0;
        Direction _direction;
        BoundingRectangle _floorCollider;

        //animation
        int _totalFreezeFrames = 7;
        int _currentFreezeFrame = 0;
        int _currentFrame = 0;
        int _totalFrames = 7;
        
        //combat
        bool _takingDamage = false;
        Timer _knockbackTimer;
        Vector2 _knockbackVelocity;
        float _knockbackSpeed;

        public Character()
        {
            ID = nextId++;
        }

        public virtual void Damage(float dmg, Vector2 knockbackVelocity, float knockbackSpeed, float knockbackTime)
        {
            _takingDamage = true;
            _knockbackVelocity = knockbackVelocity;
            _knockbackSpeed = knockbackSpeed;
            _knockbackTimer = new Timer(knockbackTime);
        }

        protected virtual void KnockBack(GameTime gameTime, List<BoundingRectangle> rects)
        {
            Velocity = _knockbackVelocity;
            Velocity.Normalize();
            Velocity *= _knockbackSpeed;
            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the character
            Position += Velocity;

            UpdateFloorCollider();
            CheckRoomCollisions(rects);
            if (_knockbackTimer.TimeIsUp(gameTime))
            {
                _takingDamage = false;
                _knockbackTimer = null;
            }
        }

        protected virtual void ActionReset()
        {
            CurrentFreezeFrame = 0;
            CurrentAnimationFrame = 0;
            ActionEnding = false;
        }

        protected virtual void ActionFollowThrough()
        {
            CurrentAnimationFrame -= 1;
            CurrentFreezeFrame += 1;
            ActionEnding = true;
        }

        protected virtual bool CheckIfAnimationShouldFlip(out int animationDirection)
        {
            animationDirection = (int)Direction;
            switch (Direction)
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

        protected virtual void UpdateFloorCollider()
        {
            _floorCollider.X = Position.X + FloorColliderOffset.X;
            _floorCollider.Y = Position.Y + FloorColliderOffset.Y;
        }

        protected virtual void ApplyVelocity(GameTime gameTime)
        {
            switch (Direction)
            {
                case Direction.Down:
                    Velocity = new Vector2(0, .8f);
                    break;

                case Direction.Right:
                    Velocity = new Vector2(1, 0);
                    break;

                case Direction.Up:
                    Velocity = new Vector2(0, -.8f);
                    break;

                case Direction.DRight:
                    Velocity = new Vector2(1, .8f);
                    Velocity *= .8f;
                    break;

                case Direction.URight:
                    Velocity = new Vector2(1, -.8f);
                    Velocity *= .8f;
                    break;

                case Direction.Left:
                    Velocity = new Vector2(-1, 0);
                    break;

                case Direction.DLeft:
                    Velocity = new Vector2(-1, .8f);
                    Velocity *= .8f;
                    break;

                case Direction.ULeft:
                    Velocity = new Vector2(-1, -.8f);
                    Velocity *= .8f;
                    break;
            }

            Velocity.Normalize();
            Velocity *= Speed;
            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the character
            Position += Velocity;

            UpdateFloorCollider();
        }

        protected virtual float GetRoomDepth()
        {
            var height = GameController.Instance.CurrentRoomSize.Y;
            var center = FloorCollider.Y + FloorCollider.Height;

            float depth = 1f - (center / height);
            if(depth > .9f)
            {
                depth = .9f;
            }

            if(depth < .1f)
            {
                depth = .1f;
            }
            return depth;
        }

        protected virtual void CheckRoomCollisions(List<BoundingRectangle> rects)
        {
            if (rects != null)
            {
                foreach (BoundingRectangle r in rects)
                {
                    SeperateFromRectangleCollider(r);
                }
            }
        }

        protected virtual void CheckForEntityCollisions()
        {
            foreach (var entity in GameController.Instance.CurrentActiveEntities)
            {
                if (entity.ID == this.ID) continue;
                SeperateFromRectangleCollider(entity.FloorCollider);
            }
        }

        private void SeperateFromRectangleCollider(BoundingRectangle r)
        {
            while (FloorCollider.CollidesWith(r))
            {
                float overlapX = Math.Min(FloorCollider.Right - r.Left, r.Right - FloorCollider.Left);
                float overlapY = Math.Min(FloorCollider.Bottom - r.Top, r.Bottom - FloorCollider.Top);

                if (Math.Abs(overlapX) < Math.Abs(overlapY))
                {
                    Position = new Vector2(Position.X - Velocity.X, Position.Y - (Velocity.Y * .1f));
                }
                else
                {
                    Position = new Vector2(Position.X - (Velocity.X * .1f), Position.Y - Velocity.Y);
                }
                UpdateFloorCollider();
            }
        }
    }
}
