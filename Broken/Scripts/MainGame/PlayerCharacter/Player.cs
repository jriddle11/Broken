using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.Common;
using System.Collections.Generic;
using System;

namespace Broken.Scripts.MainGame
{
    /// <summary>
    /// Definition of the player character
    /// </summary>
    public class Player
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Speed { get; set; } = 400f;
        public float AttackSpeed { get; set; } = .3f;
        public float RollDelay { get; set; } = .8f;

        Direction _direction;
        ActionState _actionState;

        //collision
        BoundingRectangle _floorCollider;

        const float ATTACK_MOVING_SPEED = 400;
        const float ATTACK_RECOVER_SPEED = 50f;
        const float ROLL_SPEED = 575;
        const float RUN_SPEED = 450;
        const int PLAYER_X_SIZE = 712;
        const int PLAYER_Y_SIZE = 712;

        Vector2 _centerPosition => new Vector2(Position.X + ((PLAYER_X_SIZE * _scale) / 2), Position.Y + ((PLAYER_Y_SIZE * _scale) / 2));

        //animation
        int animationDirection;
        int currentFrame = 0;
        int totalFrames = 7;
        float timePerFrame = 0.07f;
        float elapsedTime = 0;
        float _scale = .65f;
        bool animationIsFlipped = false;

        //combat
        Timer _attackSpeedTimer;
        Timer _rollDelayTimer;
        int followThroughFrames = 7;
        int currentFollowThroughFrame = 0;
        bool followingThrough => currentFollowThroughFrame < followThroughFrames;
        bool isActing => !(_actionState == ActionState.Idle || _actionState == ActionState.Running);
        bool attackEnding;

        //textures
        Texture2D _runSpriteSheet;
        Texture2D _idleSpriteSheet;
        Texture2D _swordSwingSpriteSheet;
        Texture2D _swordSlashSpriteSheet;
        Texture2D _swordSlamSpriteSheet;
        Texture2D _rollSpriteSheet;
        Texture2D _swordTrailSpriteSheet;

        Texture2D _pixel;
        Color _pixColor = Color.White * .5f;

        public void LoadContent(Game game)
        {
            _pixel = new Texture2D(OutputManager.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _runSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerRunning");
            _idleSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerIdle");
            _swordSwingSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerSwordSwing");
            _swordSlashSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerSwordSlash");
            _swordSlamSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerSwordSlam");
            _rollSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerRoll");
            _swordTrailSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerSwordTrail");
            _attackSpeedTimer = new Timer(AttackSpeed);
            _rollDelayTimer = new Timer(RollDelay);

            _floorCollider = new BoundingRectangle(Vector2.Zero, 60, 60);
            UpdateFloorCollider();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            animationIsFlipped = false;
            animationDirection = (int)_direction;
            switch (_direction)
            {
                case Direction.Left:
                    animationIsFlipped = true;
                    animationDirection = 1;
                    break;
                case Direction.DLeft:
                    animationIsFlipped = true;
                    animationDirection = 3;
                    break;
                case Direction.ULeft:
                    animationIsFlipped = true;
                    animationDirection = 4;
                    break;
            }

            Rectangle sourceRectangle = new Rectangle(
                currentFrame * PLAYER_X_SIZE,
                animationDirection * PLAYER_Y_SIZE,
                PLAYER_X_SIZE,
                PLAYER_Y_SIZE
            );

            Texture2D sprite;

            switch (_actionState) {
                case ActionState.Running:
                    sprite = _runSpriteSheet;
                    timePerFrame = 0.07f;
                    break;
                case ActionState.Swinging:
                    sprite = _swordSwingSpriteSheet;
                    timePerFrame = 0.05f;
                    break;
                case ActionState.Slashing:
                    sprite = _swordSlashSpriteSheet;
                    timePerFrame = 0.05f;
                    break;
                case ActionState.Slamming:
                    sprite = _swordSlamSpriteSheet;
                    timePerFrame = 0.05f;
                    break;
                case ActionState.Rolling:
                    sprite = _rollSpriteSheet;
                    timePerFrame = 0.10f;
                    break;
                default:
                    sprite = _idleSpriteSheet;
                    timePerFrame = 0.15f;
                    break;
            }

            spriteBatch.Draw(sprite, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, _scale, animationIsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.5f);
            if ((_actionState == ActionState.Swinging || _actionState == ActionState.Slashing || _actionState == ActionState.Slamming) && !attackEnding)
            {
                //DrawSwordTrail(spriteBatch);
            }

            //spriteBatch.Draw(_pixel, new Rectangle((int)_floorCollider.X, (int)_floorCollider.Y, (int)_floorCollider.Width, (int)_floorCollider.Height), _pixColor);
        }

        private void DrawSwordTrail(SpriteBatch spriteBatch)
        {
            Rectangle trailSRectangle = new Rectangle(
               currentFrame * 512,
               animationDirection * 300,
               512,
               300
               );
            spriteBatch.Draw(_swordTrailSpriteSheet, Position + new Vector2(-35, 250), trailSRectangle, Color.LightBlue * 0.8f, 0f, Vector2.Zero, 1f, animationIsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.5f);

        }

        public void Update(GameTime gameTime, List<BoundingRectangle> rectCols = null)
        {
            _attackSpeedTimer.Update(gameTime);
            _rollDelayTimer.Update(gameTime);
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime >= timePerFrame)
            {
                currentFrame++;
                if (currentFrame >= totalFrames)
                {
                    if (_actionState == ActionState.Swinging && followingThrough)
                    {
                        ActionRecovery();
                    }
                    else if(_actionState == ActionState.Swinging && !followingThrough)
                    {
                        _actionState = ActionState.Idle;
                        ActionReset();
                    }
                    else if (_actionState == ActionState.Slashing && followingThrough)
                    {
                        ActionRecovery();
                    }
                    else if (_actionState == ActionState.Slashing && !followingThrough)
                    {
                        _actionState = ActionState.Idle;
                        ActionReset();
                    }
                    else if (_actionState == ActionState.Slamming && followingThrough)
                    {
                        ActionRecovery();
                    }
                    else if (_actionState == ActionState.Slamming && !followingThrough)
                    {
                        _actionState = ActionState.Idle;
                        ActionReset();
                    }
                    else if (_actionState == ActionState.Rolling && followingThrough)
                    {
                        ActionRecovery();
                    }
                    else if (_actionState == ActionState.Rolling && !followingThrough)
                    {
                        _actionState = ActionState.Idle;
                        ActionReset();
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }

                elapsedTime = 0;
            }

            CheckPlayerActionInput();

            CheckPlayerMovementInput(gameTime, rectCols);

        }

        private void ActionReset()
        {
            currentFollowThroughFrame = 0;
            currentFrame = 0;
            Speed = RUN_SPEED;
            attackEnding = false;
        }

        private void ActionRecovery()
        {
            Speed = ATTACK_RECOVER_SPEED;
            currentFrame -= 1;
            currentFollowThroughFrame += 1;
            attackEnding = true;
        }

        private void CheckPlayerActionInput()
        {
            bool attackAttempted = _attackSpeedTimer.TimesUp && InputManager.Mouse1Clicked;
            bool rollAttempted = _attackSpeedTimer.TimesUp && InputManager.PressedSpace && _rollDelayTimer.TimesUp;

            if(rollAttempted && !isActing)
            {
                StartRoll();
            }
            if (attackAttempted && !isActing)
            {
                _actionState = ActionState.Swinging;
                StartNextAttack();
            }
            else if(attackAttempted && _actionState == ActionState.Swinging && followingThrough)
            {
                _actionState = ActionState.Slashing;
                StartNextAttack();
            }
            else if(attackAttempted && _actionState == ActionState.Slashing && followingThrough)
            {
                _actionState = ActionState.Slamming;
                StartNextAttack();
                Speed *= 1.5f;
            }
        }

        private void StartNextAttack()
        {
            _attackSpeedTimer.Reset();
            _direction = InputManager.GetMouseDirection(_centerPosition);
            Speed = ATTACK_MOVING_SPEED;
            currentFollowThroughFrame = 0;
            currentFrame = 0;
            attackEnding = false;
        }

        private void StartRoll()
        {
            _direction = InputManager.GetKeyboardDirection(_direction);
            _actionState = ActionState.Rolling;
            _attackSpeedTimer.Reset();
            _rollDelayTimer.Reset();
            Speed = ROLL_SPEED;
            currentFollowThroughFrame = 7;
            currentFrame = 0;
        }

        private void CheckPlayerMovementInput(GameTime gameTime, List<BoundingRectangle> rectCols = null)
        {
            if (!InputManager.IsHolding && !isActing)
            {
                _actionState = ActionState.Idle;
                return;
            }
            else if (!isActing)
            {
                _direction = InputManager.GetKeyboardDirection(_direction);
                _actionState = ActionState.Running;
            }

            switch (_direction)
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

            // Normalize and apply speed
            Velocity.Normalize();
            Velocity *= Speed;
            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the player
            Position += Velocity;

            UpdateFloorCollider();

            if (rectCols != null)
            {
                foreach (BoundingRectangle r in rectCols)
                {
                    while (_floorCollider.CollidesWith(r))
                    {
                        float overlapX = Math.Min(_floorCollider.Right - r.Left, r.Right - _floorCollider.Left);
                        float overlapY = Math.Min(_floorCollider.Bottom - r.Top, r.Bottom - _floorCollider.Top);

                        if (Math.Abs(overlapX) < Math.Abs(overlapY))
                        {
                            Position = new Vector2(Position.X - Velocity.X, Position.Y - (Velocity.Y * .1f));
                        }
                        else
                        {
                            Position = new Vector2(Position.X - (Velocity.X *.1f), Position.Y - Velocity.Y);
                        }
                        UpdateFloorCollider();
                    }
                }
            }
        }

        private void UpdateFloorCollider()
        {
            _floorCollider.X = Position.X + 232.375f - 25;
            _floorCollider.Y = Position.Y + 232.375f + 30;
        }

        public BoundingRectangle GetFloorCollider()
        {
            return _floorCollider;
        }
    }

    public enum ActionState
    {
        Idle, Running, Swinging, Slashing, Slamming, Rolling
    }
}
