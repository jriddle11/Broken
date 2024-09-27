using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts;
using System.Collections.Generic;
using System;
using Broken.Scripts.Models;

namespace Broken.Scripts.MainGame
{
    public class Slime : Entity
    {
        public Color Color;

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
        float ANIMATION_IDLE_FRAMERATE = 0.13f;
        const float ANIMATION_ATTACK_FRAMERATE = 0.05f;

        //combat
        bool _isIdle => _actionState == SlimeAction.Idle;

        public Slime(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public override void LoadContent(Game game)
        {
            Textures.Add("idle", "My Assets/Slime/SlimeIdle", game);
            Textures.Add("charge", "My Assets/Slime/SlimeCharge", game);
            Textures.Add("jump", "My Assets/Slime/SlimeJump", game);
            Textures.Add("land", "My Assets/Slime/SlimeLand", game);
            Initialize();
        }

        private void Initialize()
        {
            FloorCollider = new BoundingRectangle(Position.X, Position.Y, 60, 60);
            FloorColliderOffset = new Vector2(232.375f - 30, 232.375f + 30);
            UpdateFloorCollider();

            ANIMATION_IDLE_FRAMERATE -= RandomHelper.NextFloat(0f, .05f);
            _combatAI = new EnemyCombatAI(3f - RandomHelper.NextFloat(0f, 2f));
            _enemyState = new EnemyState() { AwareOfPlayer = true };
            AnimationTimer = new Timer(ANIMATION_IDLE_FRAMERATE);
            Speed = JUMP_SPEED;
        }

        public override void Update(GameTime gameTime)
        {
            
            _combatAI.Update(gameTime);

            base.Update(gameTime);

            if (TakingDamage)
            {

                if (_isIdle)
                {
                    DamageCheck(gameTime, true);
                    return;
                }
                else
                {
                    DamageCheck(gameTime, false);
                }

            }

            CheckActionState();

            CheckMovement(gameTime);

            CheckRoomCollisions();

            CheckEntityCollisions();
        }

        protected override void AnimationFrameTick()
        {
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
        }

        private void CheckMovement(GameTime gameTime)
        {

            if (_actionState == SlimeAction.Idle)
            {
                return;
            }
            if(_actionState == SlimeAction.Jumping && CurrentAnimationFrame < 2)
            {
                Direction = DirectionHelper.GetDirection(Position, GameController.Instance.PlayerPosition);
                if (_currentCombatDecision == AICombatDecision.MoveAwayFromPlayer) Direction = DirectionHelper.GetOppositeDirection(Direction);
            }
            if(_actionState == SlimeAction.Jumping || _actionState == SlimeAction.Landing)
            { 
                ApplyVelocity(gameTime);
            }
            if (_actionState == SlimeAction.Landing && CurrentAnimationFrame > 2) Speed = 0;
        }

        private void CheckActionState()
        {
            if (_isIdle && _combatAI.HasActedOnLastDecision)
            {
                _currentCombatDecision = _combatAI.GetNextDecision(_enemyState);
            }

            if (_currentCombatDecision == AICombatDecision.DoNothing)
            {
                _actionState = SlimeAction.Idle;
                return;
            }

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

        private void ActOnDecision()
        {
            switch (_currentCombatDecision)
            {
                case AICombatDecision.MoveToPlayer:
                    MoveAction();
                    break;
                case AICombatDecision.Attack:
                    AttackAction();
                    break;
            }
        }

        private void MoveAction()
        {
            _actionState = SlimeAction.Jumping;
            CurrentFreezeFrame = 6;
            Speed = JUMP_SPEED;
        }

        private void AttackAction()
        {

        }

        public void FinishedActing()
        {
            _currentCombatDecision = AICombatDecision.DoNothing;
            _actionState= SlimeAction.Idle;
            _combatAI.ActionComplete();
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

            switch (_actionState)
            { 
                case SlimeAction.Charging:
                    AnimationTimer.ChangeSetTime(ANIMATION_IDLE_FRAMERATE);
                    sprite = Textures["charge"];
                    break;
                case SlimeAction.Jumping:
                    AnimationTimer.ChangeSetTime(ANIMATION_JUMP_FRAMERATE);
                    sprite = Textures["jump"];
                    break;
                case SlimeAction.Landing:
                    AnimationTimer.ChangeSetTime(ANIMATION_LAND_FRAMERATE);
                    sprite = Textures["land"];
                    break;
                default:
                    AnimationTimer.ChangeSetTime(ANIMATION_IDLE_FRAMERATE);
                    sprite = Textures["idle"];
                    break;

            }

            spriteBatch.Draw(sprite, Position, sourceRectangle, TakingDamage ? Color.Red * .5f : Color, 0f, Vector2.Zero, Scale, animationFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, GetRoomDepth());

            //DevManager.DrawRectangle(spriteBatch, FloorCollider, Color.White);
        }

        private enum SlimeAction
        {
            Idle, Charging, Jumping, Landing, PoweringUp, Multiplying, Leaping
        }
    }
}
