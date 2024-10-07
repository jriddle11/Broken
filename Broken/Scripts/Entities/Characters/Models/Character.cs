using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Broken.Entities
{
    public abstract class Character : IGameObject
    {
        public uint ID;
        private static uint nextId = 0;

        protected Texture2DList Textures = new();

        public CharacterStatus Status = new();

        public IStatusHandler StatusHandler;

        public AttackHandler AttackHandler;

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Direction Direction { get; protected set; }

        public float Speed { get; set; } = 400f;

        public float Scale { get; set; } = .65f;

        #region Collision Variables
        public virtual bool IsDead { get; protected set; } = false;
        public virtual bool HasDamageCollision { get; protected set; } = true;
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
        public Vector2 CenterFloorPosition => new Vector2(FloorCollider.X + FloorCollider.Width / 2, FloorCollider.Y + FloorCollider.Height / 2);
        BoundingRectangle _floorCollider;

        #endregion

        #region Animation Variables
        public DrawRecord DrawRecord { get; protected set; } = new();
        public bool FollowingThrough => _currentFreezeFrame < _totalFreezeFrames;
        public bool AnimationEnding { get; protected set; }
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
        protected Timer AnimationTimer;

        int _totalFreezeFrames = 7;
        int _currentFreezeFrame = 0;
        int _currentFrame = 0;
        int _totalFrames = 7;
        #endregion

        #region Damage Variables
        public virtual bool TakingDamage { get; protected set; }
        Timer _knockbackTimer;
        Vector2 _knockbackVelocity;
        float _knockbackSpeed;
        #endregion

        public Character()
        {
            ID = nextId++;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (AnimationTimer.TimeIsUp(gameTime))
            {
                CurrentAnimationFrame++;
                AnimationFrameTick();
                AnimationTimer.Reset();
            }
        }

        protected abstract void AnimationFrameTick();

        public abstract void Draw(SpriteBatch spriteBatch, float opacity = 1f);

        public abstract void LoadContent(Game game);

        public virtual void Damage(int dmg, Vector2 knockbackVelocity, float knockbackSpeed, float knockbackTime)
        {
            TakingDamage = true;
            _knockbackVelocity = knockbackVelocity;
            _knockbackSpeed = knockbackSpeed;
            _knockbackTimer = new Timer(knockbackTime);
            StatusHandler.Damage(dmg);
        }

        protected virtual void DamageCheck(GameTime gameTime, bool canKnockBack)
        {
            if (canKnockBack)
            {
                Velocity = _knockbackVelocity;
                Velocity.Normalize();
                Velocity *= _knockbackSpeed;
                Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Move the character
                Position += Velocity;

                UpdateFloorCollider();
                CheckRoomCollisions();
                CheckEntityCollisions();
            }
           
            if (_knockbackTimer.TimeIsUp(gameTime))
            {
                TakingDamage = false;
                _knockbackTimer = null;
            }
        }
        protected virtual void ActionReset()
        {
            CurrentFreezeFrame = 0;
            CurrentAnimationFrame = 0;
            AnimationEnding = false;
        }

        protected virtual void ActionFollowThrough()
        {
            CurrentAnimationFrame -= 1;
            CurrentFreezeFrame += 1;
            AnimationEnding = true;
        }

        protected virtual void UpdateFloorCollider()
        {
            _floorCollider.X = Position.X + FloorColliderOffset.X;
            _floorCollider.Y = Position.Y + FloorColliderOffset.Y;
        }

        protected virtual void ApplyVelocity(GameTime gameTime)
        {
            Velocity = DirectionHelper.GetDirectionalVelocity(Velocity, Direction);
            Velocity.Normalize();
            Velocity *= Speed;
            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity;

            UpdateFloorCollider();
        }

        protected float GetRoomDepth()
        {
            return GameScreen.Instance.GetRoomDepth(FloorCollider);
        }

        protected virtual void CheckRoomCollisions()
        {
            var rects = GameScreen.Instance.CurrentRoom.RectangleColliders;
            if (rects != null)
            {
                foreach (BoundingRectangle r in rects)
                {
                    SeperateFromRectangleCollider(r);
                }
            }
            
        }

        protected virtual void CheckEntityCollisions()
        {
            return; // DISABLED
            foreach (var entity in GameScreen.Instance.CurrentCharacters)
            {
                if (entity.ID == this.ID) continue;
                SeperateFromRectangleCollider(entity.FloorCollider);
            }
        }

        private void SeperateFromRectangleCollider(BoundingRectangle r)
        {
            int i = 0;
            while (FloorCollider.CollidesWith(r) && i < 10)
            {
                i++;
                float overlapX = Math.Min(FloorCollider.Right - r.Left, r.Right - FloorCollider.Left);
                float overlapY = Math.Min(FloorCollider.Bottom - r.Top, r.Bottom - FloorCollider.Top);

                if (Math.Abs(overlapX) < Math.Abs(overlapY))
                {
                    Position = new Vector2(Position.X - (Velocity.X * 1.025f), Position.Y - (Velocity.Y * .025f));
                }
                else
                {
                    Position = new Vector2(Position.X - (Velocity.X * .025f), Position.Y - (Velocity.Y * 1.025f));
                }
                UpdateFloorCollider();
            }
        }
    }
}
