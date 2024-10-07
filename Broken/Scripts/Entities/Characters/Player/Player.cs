using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace Broken.Entities
{ 
    /// <summary>
    /// Definition of the player character
    /// </summary>
    public class Player : Character
    { 
        public float AttackSpeed { get; set; } = .28f;
        public float RollDelay { get; set; } = .9f;
        public float SwordReach { get; set; } = 175;
        public override bool HasDamageCollision => _actionState != PlayerAction.Rolling;

        //combat constants
        const float ATTACK_MOVING_SPEED = 250;
        const float ATTACK_RECOVER_SPEED = 50f;
        const float ROLL_SPEED = 575;
        const float RUN_SPEED = 450;

        //animation constants
        const float ANIMATION_RUN_FRAMERATE = 0.07f;
        const float ANIMATION_IDLE_FRAMERATE = 0.16f;
        const float ANIMATION_ATTACK_FRAMERATE = 0.05f;
        const float ANIMATION_ROLL_FRAMERATE = 0.09f;

        //size constants
        const int TEXTURE_WIDTH = 712;
        const int TEXTURE_HEIGHT = 712;

        PlayerAction _actionState;

        //timers
        Timer _attackSpeedTimer;
        Timer _rollDelayTimer;

        // Sound effect variables
        private SoundEffect _swordSound;
        private SoundEffectInstance _soundInstance;

        //helpers
        public Vector2 CenterPosition => new Vector2(Position.X + _playerHalfSize, Position.Y + _playerHalfSize);
        float _playerHalfSize => (TEXTURE_WIDTH * Scale) / 2;
        bool _isActing => !(_actionState == PlayerAction.Idle || _actionState == PlayerAction.Running);

        public Player(Vector2 position)
        {
            Position = position;
        }

        public override void LoadContent(Game game)
        {
            Textures.Add("run", "My Assets/Player/PlayerRunning", game);
            Textures.Add("idle", "My Assets/Player/PlayerIdle", game);
            Textures.Add("swing", "My Assets/Player/PlayerSwordSwing", game);
            Textures.Add("slash", "My Assets/Player/PlayerSwordSlash", game);
            Textures.Add("slam", "My Assets/Player/PlayerSwordSlam", game);
            Textures.Add("roll", "My Assets/Player/PlayerRoll", game);
            Textures.Add("trail", "My Assets/Player/PlayerSwordTrail", game);
            _swordSound = game.Content.Load<SoundEffect>("External Assets/SoundEffects/SwordSwing");
            Initialize();
        }

        private void Initialize()
        {
            _soundInstance = _swordSound.CreateInstance();
            _soundInstance.Volume = 0.5f;
            AnimationTimer = new Timer(ANIMATION_IDLE_FRAMERATE);
            _attackSpeedTimer = new Timer(AttackSpeed);
            _rollDelayTimer = new Timer(RollDelay);

            AttackHandler = new PlayerAttackHandler(ID);

            FloorCollider = new BoundingRectangle(Vector2.Zero, 60, 60);
            FloorColliderOffset = new Vector2(232.375f - 25, 232.375f + 30);
            UpdateFloorCollider();
            Status.Experience = 10;
            Status.MaxExperience = 15;
        }

        private void Attack()
        {
            AttackHandler.Attack(this, GameScreen.Instance.CurrentCharacters);
            //_swordSound.Play();
        }

        public override void Update(GameTime gameTime)
        {
            _attackSpeedTimer.Update(gameTime);
            _rollDelayTimer.Update(gameTime);

            base.Update(gameTime);

            CheckPlayerActionInput();

            var pos = Position;
            CheckPlayerMovementInput(gameTime);

            CheckRoomCollisions();

            CheckEntityCollisions();
        }

        protected override void AnimationFrameTick()
        {
            if (CurrentAnimationFrame >= TotalAnimationFrames)
            {
                if (_actionState == PlayerAction.Idle || _actionState == PlayerAction.Running || !FollowingThrough)
                {
                    ActionReset();
                }
                else
                {
                    ActionFollowThrough();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float opacity = 1f)
        {

            bool animationFlipped = DirectionHelper.CheckIfAnimationShouldFlip(Direction, out int animationDirection);


            Rectangle sourceRectangle = new Rectangle(
                CurrentAnimationFrame * TEXTURE_WIDTH,
                animationDirection * TEXTURE_HEIGHT,
                TEXTURE_WIDTH,
                TEXTURE_HEIGHT
            );

            Texture2D sprite;

            //change sprite sheet and animation frame rate
            switch (_actionState)
            {
                case PlayerAction.Running:
                    sprite = Textures["run"];
                    AnimationTimer.ChangeSetTime(ANIMATION_RUN_FRAMERATE);
                    break;
                case PlayerAction.Swinging:
                    sprite = Textures["swing"];
                    AnimationTimer.ChangeSetTime(ANIMATION_ATTACK_FRAMERATE);
                    break;
                case PlayerAction.Slashing:
                    sprite = Textures["slash"];
                    AnimationTimer.ChangeSetTime(ANIMATION_ATTACK_FRAMERATE);
                    break;
                case PlayerAction.Slamming:
                    sprite = Textures["slam"];
                    AnimationTimer.ChangeSetTime(ANIMATION_ATTACK_FRAMERATE);
                    break;
                case PlayerAction.Rolling:
                    sprite = Textures["roll"];
                    AnimationTimer.ChangeSetTime(ANIMATION_ROLL_FRAMERATE);
                    break;
                default:
                    sprite = Textures["idle"];
                    AnimationTimer.ChangeSetTime(ANIMATION_IDLE_FRAMERATE);
                    break;
            }

            spriteBatch.Draw(sprite, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, Scale, animationFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, GetRoomDepth());
            if ((_actionState == PlayerAction.Swinging || _actionState == PlayerAction.Slashing || _actionState == PlayerAction.Slamming) && !AnimationEnding)
            {
                DrawSwordTrail(spriteBatch, animationFlipped);
            }

            //AttackHandler.DrawColliders(spriteBatch);
        }

        private void DrawSwordTrail(SpriteBatch spriteBatch, bool animationFlipped)
        {
            Rectangle trailSRectangle = new Rectangle(
               CurrentAnimationFrame * 512,
               0,
               512,
               300
               );

            //sprite adjustment logic
            SpriteEffects effect = animationFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            float rotation = 0f;
            Vector2 rangeVector = Vector2.Zero;
            switch (Direction)
            {
                case Direction.Left:
                    rangeVector = new Vector2(-SwordReach, 0);
                    rotation = -(float)Math.PI / 2;
                    effect = SpriteEffects.FlipVertically;
                    if (_actionState != PlayerAction.Swinging)
                    {
                        effect = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    }
                    break;
                case Direction.DLeft:
                    rangeVector = new Vector2(-SwordReach / 1.5f, SwordReach / 1.5f);
                    rotation = (float)Math.PI / 4;
                    effect = SpriteEffects.FlipHorizontally;
                    if (_actionState != PlayerAction.Swinging)
                    {
                        effect = SpriteEffects.None;
                    }
                    break;
                case Direction.ULeft:
                    rangeVector = new Vector2(-SwordReach / 1.5f, -SwordReach / 1.5f);
                    rotation = ((float)Math.PI * 3) / 4;
                    if (_actionState != PlayerAction.Swinging)
                    {
                        effect = SpriteEffects.None;
                    }
                    break;
                case Direction.Right:
                    rangeVector = new Vector2(SwordReach, 0);
                    rotation = (float)Math.PI / 2;
                    effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    if (_actionState != PlayerAction.Swinging)
                    {
                        effect = SpriteEffects.FlipVertically;
                    }
                    break;
                case Direction.DRight:
                    rangeVector = new Vector2(SwordReach / 1.5f, SwordReach / 1.5f);
                    rotation = -(float)Math.PI / 4;
                    if (_actionState != PlayerAction.Swinging)
                    {
                        effect = SpriteEffects.FlipHorizontally;
                    }
                    break;
                case Direction.URight:
                    rangeVector = new Vector2(SwordReach / 1.5f, -SwordReach / 1.5f);
                    rotation = -((float)Math.PI * 3) / 4;
                    if (_actionState != PlayerAction.Swinging)
                    {
                        effect = SpriteEffects.FlipHorizontally;
                    }
                    break;
                case Direction.Up:
                    rangeVector = new Vector2(0, -SwordReach);
                    effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    if (_actionState != PlayerAction.Swinging)
                    {
                        effect = SpriteEffects.FlipVertically;
                    }
                    break;
                case Direction.Down:
                    rangeVector = new Vector2(0, SwordReach);
                    if (_actionState != PlayerAction.Swinging)
                    {
                        effect = SpriteEffects.FlipHorizontally;
                    }
                    break;
            }

            spriteBatch.Draw(Textures["trail"], Position + new Vector2(_playerHalfSize, _playerHalfSize) + rangeVector, trailSRectangle, Color.LightBlue * 0.3f, rotation, new Vector2(256, 150), 1f, effect, .01f);
            
        }

        protected override void ActionReset()
        {
            base.ActionReset();
            _actionState = PlayerAction.Idle;
            Speed = RUN_SPEED;
        }

        protected override void ActionFollowThrough()
        {
            base.ActionFollowThrough();
            Speed = ATTACK_RECOVER_SPEED;
        }

        private void CheckPlayerActionInput()
        {
            bool attackAttempted = _attackSpeedTimer.TimesUp && InputManager.Mouse1Clicked;
            bool rollAttempted = _attackSpeedTimer.TimesUp && InputManager.PressedSpace && _rollDelayTimer.TimesUp;

            if(rollAttempted && !_isActing)
            {
                StartRoll();
            }
            if (attackAttempted && !_isActing)
            {
                _actionState = PlayerAction.Swinging;
                StartNextAttack();
            }
            else if(attackAttempted && _actionState == PlayerAction.Swinging && FollowingThrough)
            {
                _actionState = PlayerAction.Slashing;
                StartNextAttack();
            }
            else if(attackAttempted && _actionState == PlayerAction.Slashing && FollowingThrough)
            {
                _actionState = PlayerAction.Slamming;
                StartNextAttack();
                Speed *= 1.5f;
            }
            else if(rollAttempted && _actionState != PlayerAction.Rolling && _isActing && CurrentFreezeFrame > 1)
            {
                StartRoll();
            }
        }

        private void StartNextAttack()
        {
            _attackSpeedTimer.Reset();
            Direction = InputManager.GetMouseDirection(CenterPosition);
            Attack();
            Speed = ATTACK_MOVING_SPEED;
            base.ActionReset();
        }

        private void StartRoll()
        {
            Direction = InputManager.GetKeyboardDirection(Direction);
            _actionState = PlayerAction.Rolling;
            _attackSpeedTimer.Reset();
            _rollDelayTimer.Reset();
            Speed = ROLL_SPEED;
            CurrentFreezeFrame = 7;
            CurrentAnimationFrame = 0;
        }

        private void CheckPlayerMovementInput(GameTime gameTime)
        {
            if (!InputManager.IsHolding && !_isActing)
            {
                _actionState = PlayerAction.Idle;
                return;
            }
            else if (!_isActing)
            {
                Direction = InputManager.GetKeyboardDirection(Direction);
                _actionState = PlayerAction.Running;
            }

            ApplyVelocity(gameTime);
        }

        private enum PlayerAction
        {
            Idle, Running, Swinging, Slashing, Slamming, Rolling
        }
    }
}
