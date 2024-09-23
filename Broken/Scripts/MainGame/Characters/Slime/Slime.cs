using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.Common;
using System.Collections.Generic;
using System;
using Broken.Scripts.Models;

namespace Broken.Scripts.MainGame
{
    public class Slime : Character
    {
        public float AttackChargeTime { get; set; } = 1f;

        SlimeAction _actionState;
        EnemyState _enemyState;
        EnemyCombatAI _combatAI;
        AICombatDecision _currentCombatDecision;

        //size constants
        const int TEXTURE_WIDTH = 712;
        const int TEXTURE_HEIGHT = 712;

        //combat constants
        const float JUMP_SPEED = 600f;

        //animation constants
        const float ANIMATION_JUMP_FRAMERATE = 0.05f;
        const float ANIMATION_LAND_FRAMERATE = 0.05f;
        const float ANIMATION_IDLE_FRAMERATE = 0.13f;
        const float ANIMATION_ATTACK_FRAMERATE = 0.05f;

        //timers
        Timer _animationTimer;

        //textures
        Texture2D _idleSpriteSheet;
        Texture2D _chargeSpriteSheet;
        Texture2D _jumpSpriteSheet;
        Texture2D _landSpriteSheet;

        //combat
        bool _isIdle => _actionState == SlimeAction.Idle;
        bool _isCharged => _actionState == SlimeAction.Charging && CurrentAnimationFrame >= TotalAnimationFrames;

        public void LoadContent(Game game)
        {
            FloorCollider = new BoundingRectangle(Position.X, Position.Y, 90, 60);
            FloorColliderOffset = new Vector2(232.375f - 45, 232.375f + 30);
            UpdateFloorCollider();

            _combatAI = new EnemyCombatAI(1f);
            _enemyState = new EnemyState() { AwareOfPlayer = true };

            _idleSpriteSheet = game.Content.Load<Texture2D>("My Assets/Slime/SlimeIdle");
            _chargeSpriteSheet = game.Content.Load<Texture2D>("My Assets/Slime/SlimeCharge");
            _jumpSpriteSheet = game.Content.Load<Texture2D>("My Assets/Slime/SlimeJump");
            _landSpriteSheet = game.Content.Load<Texture2D>("My Assets/Slime/SlimeLand");

            _animationTimer = new Timer(ANIMATION_IDLE_FRAMERATE);

            Speed = JUMP_SPEED;
        }

        public void Update(GameTime gameTime, List<BoundingRectangle> rectColliders = null)
        {
            if (TakingDamage)
            {
                KnockBack(gameTime, rectColliders);
                return;
            }
            _combatAI.Update(gameTime);

            if (_animationTimer.TimeIsUp(gameTime))
            {
                CurrentAnimationFrame++;

                if (CurrentAnimationFrame >= TotalAnimationFrames)
                {
                    if (_isIdle || !FollowingThrough)
                    {
                        ActionReset();
                    }
                    else
                    {
                        ActionFollowThrough();
                    }
                }
                _animationTimer.Reset();
            }

            CheckActionState();

            CheckMovement(gameTime, rectColliders);

            CheckForEntityCollisions();
        }

        private void CheckMovement(GameTime gameTime, List<BoundingRectangle> rectColliders)
        {

            if(_actionState == SlimeAction.Charging && CurrentAnimationFrame < 6)
            {
                Direction = DirectionHelper.GetDirection(Position, GameController.Instance.PlayerPosition);
                if (_currentCombatDecision == AICombatDecision.MoveAwayFromPlayer) Direction = DirectionHelper.GetOppositeDirection(Direction);
            }
            if(_actionState == SlimeAction.Jumping || _actionState == SlimeAction.Landing)
            { 
                
                ApplyVelocity(gameTime);
                CheckRoomCollisions(rectColliders);
            }
            if (_actionState == SlimeAction.Landing && CurrentAnimationFrame > 2) Speed = 0;
        }

        private void CheckActionState()
        {
            if (_isIdle && _combatAI.HasActedOnLastDecision)
            {
                _currentCombatDecision = _combatAI.GetNextDecision(_enemyState);
            }

            if (_currentCombatDecision == AICombatDecision.DoNothing) return;

            if(_isIdle && _actionState != SlimeAction.Charging)
            {
                _actionState = SlimeAction.Charging;
                CurrentFreezeFrame = TotalFreezeFrames;
            }
            else if (_actionState == SlimeAction.Charging && !FollowingThrough)
            {
                ActOnDecision();
                ActionReset();
            }
            else if (_actionState == SlimeAction.Jumping && !FollowingThrough)
            {
                ActionReset();
                _actionState = SlimeAction.Landing;
                CurrentFreezeFrame = 6;
            }
            else if (_actionState == SlimeAction.Landing && !FollowingThrough)
            {
                ActionReset();
                FinishedActing();
            }
        }

        public void ActOnDecision()
        {
            switch (_currentCombatDecision)
            {
                case AICombatDecision.MoveToPlayer:
                    MoveTowardsPlayer();
                    break;
                case AICombatDecision.Attack:
                    AttackPlayer();
                    break;
            }
        }

        private void MoveTowardsPlayer()
        {
            _actionState = SlimeAction.Jumping;
            CurrentFreezeFrame = 6;
            Speed = JUMP_SPEED;
        }

        private void AttackPlayer()
        {

        }

        public void FinishedActing()
        {
            _currentCombatDecision = AICombatDecision.DoNothing;
            _actionState= SlimeAction.Idle;
            _combatAI.ActionComplete();
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            bool animationFlipped = CheckIfAnimationShouldFlip(out int animationDirection);

            Rectangle sourceRectangle = new Rectangle(
                CurrentAnimationFrame * TEXTURE_WIDTH,
                animationDirection * TEXTURE_HEIGHT,
                TEXTURE_WIDTH,
                TEXTURE_HEIGHT
            );

            Texture2D sprite;

            switch (_actionState)
            { 
                case SlimeAction.Charging:
                    _animationTimer.ChangeSetTime(ANIMATION_IDLE_FRAMERATE);
                    sprite = _chargeSpriteSheet;
                    break;
                case SlimeAction.Jumping:
                    _animationTimer.ChangeSetTime(ANIMATION_JUMP_FRAMERATE);
                    sprite = _jumpSpriteSheet;
                    break;
                case SlimeAction.Landing:
                    _animationTimer.ChangeSetTime(ANIMATION_LAND_FRAMERATE);
                    sprite = _landSpriteSheet;
                    break;
                default:
                    _animationTimer.ChangeSetTime(ANIMATION_IDLE_FRAMERATE);
                    sprite = _idleSpriteSheet;
                    break;

            }

            spriteBatch.Draw(sprite, Position, sourceRectangle, Color.LightGreen, 0f, Vector2.Zero, Scale, animationFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, GetRoomDepth());

            //DevManager.DrawRectangle(spriteBatch, FloorCollider, Color.White);
        }

        private enum SlimeAction
        {
            Idle, Charging, Jumping, Landing, PoweringUp, Multiplying, Leaping
        }
    }
}
