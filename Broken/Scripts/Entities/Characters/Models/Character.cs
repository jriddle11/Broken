using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Broken.Audio;

namespace Broken.Entities
{
    public abstract class Character : IGameObject
    {
        public uint ID;
        private static uint nextId = 0;

        protected TextureLibrary Textures = new();

        public CharacterStatus Status = new();

        public IStatusHandler StatusHandler;

        public AttackHandler AttackHandler;

        public Vector2 Position, Velocity;

        public Direction Direction { get; protected set; }

        public float Speed = 400f;

        public float Scale = .65f;

        #region Collision Variables
        public virtual bool IsDead { get; protected set; } = false;
        public virtual bool HasDamageCollision { get; protected set; } = true;
        public BoundingRectangle Collider
        {
            get
            {
                return _collider;
            }
            protected set
            {
                _collider = value;
            }
        }
        public Vector2 ColliderOffset = Vector2.Zero;
        public Vector2 CenterFloorPosition => new Vector2(Collider.X + Collider.Width / 2, Collider.Y + Collider.Height / 2);
        BoundingRectangle _collider;

        #endregion

        #region Animation Variables
        public DrawRecord DrawRecord { get; protected set; } = new();
        public bool FollowingThrough => CurrentFreezeFrame < TotalFreezeFrames;
        public bool AnimationEnding { get; protected set; }
        protected int CurrentAnimationFrame;
        protected int TotalAnimationFrames = 7;
        protected int CurrentFreezeFrame;
        protected int TotalFreezeFrames = 7;
        protected Timer AnimationTimer;

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

                UpdateCollider();
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

        public virtual void UpdateCollider()
        {
            _collider.X = Position.X + ColliderOffset.X;
            _collider.Y = Position.Y + ColliderOffset.Y;
        }

        protected virtual void ApplyVelocity(GameTime gameTime)
        {
            Velocity = DirectionHelper.GetDirectionalVelocity(Velocity, Direction);
            Velocity.Normalize();
            Velocity *= Speed;
            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity;

            UpdateCollider();
        }

        protected float GetRoomDepth()
        {
            return GameScreen.Instance.GetRoomDepth(Collider);
        }
    }
}
