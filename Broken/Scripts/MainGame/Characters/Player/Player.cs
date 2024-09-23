using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.Common;
using Broken.Scripts.Models;
using System.Collections.Generic;
using System;

namespace Broken.Scripts.MainGame
{
    /// <summary>
    /// Definition of the player character
    /// </summary>
    public class Player : Character
    {
        public float AttackSpeed { get; set; } = .3f;
        public float RollDelay { get; set; } = .8f;
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
        Timer _animationTimer;
        Timer _attackSpeedTimer;
        Timer _rollDelayTimer;

        //helpers
        float _playerHalfSize => (TEXTURE_WIDTH * Scale) / 2;
        Vector2 _centerPosition => new Vector2(Position.X + _playerHalfSize, Position.Y + _playerHalfSize);
        bool _isActing => !(_actionState == PlayerAction.Idle || _actionState == PlayerAction.Running);

        //textures
        Texture2D _runSpriteSheet;
        Texture2D _idleSpriteSheet;
        Texture2D _swordSwingSpriteSheet;
        Texture2D _swordSlashSpriteSheet;
        Texture2D _swordSlamSpriteSheet;
        Texture2D _rollSpriteSheet;
        Texture2D _swordTrailSpriteSheet;

        public void LoadContent(Game game)
        {
            _runSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerRunning");
            _idleSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerIdle");
            _swordSwingSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerSwordSwing");
            _swordSlashSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerSwordSlash");
            _swordSlamSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerSwordSlam");
            _rollSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerRoll");
            _swordTrailSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerSwordTrail");

            _animationTimer = new Timer(0.07f);
            _attackSpeedTimer = new Timer(AttackSpeed);
            _rollDelayTimer = new Timer(RollDelay);

            FloorCollider = new BoundingRectangle(Vector2.Zero, 60, 60);
            FloorColliderOffset = new Vector2(232.375f - 25, 232.375f + 30);
            UpdateFloorCollider();
        }

        public void Update(GameTime gameTime, List<BoundingRectangle> staticRectColliders = null)
        {
            _attackSpeedTimer.Update(gameTime);
            _rollDelayTimer.Update(gameTime);

            if (_animationTimer.TimeIsUp(gameTime))
            {
                CurrentAnimationFrame++;
                if (CurrentAnimationFrame >= TotalAnimationFrames)
                {
                    if(_actionState == PlayerAction.Idle || _actionState == PlayerAction.Running || !FollowingThrough)
                    {
                        PlayerActionReset();
                    }
                    else
                    {
                        PlayerActionFollowThrough();
                    }
                }

                _animationTimer.Reset();
            }

            CheckPlayerActionInput();

            CheckPlayerMovementInput(gameTime, staticRectColliders);

            CheckForEntityCollisions();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            bool animationFlipped = CheckIfAnimationShouldFlip(out int animationDirection);

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
                    sprite = _runSpriteSheet;
                    _animationTimer.ChangeSetTime(ANIMATION_RUN_FRAMERATE);
                    break;
                case PlayerAction.Swinging:
                    sprite = _swordSwingSpriteSheet;
                    _animationTimer.ChangeSetTime(ANIMATION_ATTACK_FRAMERATE);
                    break;
                case PlayerAction.Slashing:
                    sprite = _swordSlashSpriteSheet;
                    _animationTimer.ChangeSetTime(ANIMATION_ATTACK_FRAMERATE);
                    break;
                case PlayerAction.Slamming:
                    sprite = _swordSlamSpriteSheet;
                    _animationTimer.ChangeSetTime(ANIMATION_ATTACK_FRAMERATE);
                    break;
                case PlayerAction.Rolling:
                    sprite = _rollSpriteSheet;
                     _animationTimer.ChangeSetTime(ANIMATION_ROLL_FRAMERATE);
                    break;
                default:
                    sprite = _idleSpriteSheet;
                    _animationTimer.ChangeSetTime(ANIMATION_IDLE_FRAMERATE);
                    break;
            }

            spriteBatch.Draw(sprite, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, Scale, animationFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, GetRoomDepth());
            if ((_actionState == PlayerAction.Swinging || _actionState == PlayerAction.Slashing || _actionState == PlayerAction.Slamming) && !ActionEnding)
            {
                DrawSwordTrail(spriteBatch, animationFlipped);
            }

            //DevManager.DrawRectangle(spriteBatch, FloorCollider, Color.White);
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

            spriteBatch.Draw(_swordTrailSpriteSheet, Position + new Vector2(_playerHalfSize, _playerHalfSize) + rangeVector, trailSRectangle, Color.LightBlue * 0.3f, rotation, new Vector2(256, 150), 1f, effect, .01f);
        }


        private void PlayerActionReset()
        {
            ActionReset();
            _actionState = PlayerAction.Idle;
            Speed = RUN_SPEED;
        }

        private void PlayerActionFollowThrough()
        {
            ActionFollowThrough();
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
        }

        private void StartNextAttack()
        {
            _attackSpeedTimer.Reset();
            Direction = InputManager.GetMouseDirection(_centerPosition);
            Speed = ATTACK_MOVING_SPEED;
            ActionReset();
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

        private void CheckPlayerMovementInput(GameTime gameTime, List<BoundingRectangle> rectColliders = null)
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

            // Normalize then apply speed * velocity
            ApplyVelocity(gameTime);

            //check for collisons
            CheckRoomCollisions(rectColliders);
        }

        private enum PlayerAction
        {
            Idle, Running, Swinging, Slashing, Slamming, Rolling
        }
    }
}
