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

        Direction _direction;

        //collision
        BoundingRectangle _floorCollider;

        const int PLAYER_X_SIZE = 512;
        const int PLAYER_Y_SIZE = 512;

        //animation
        int animationDirection;
        int currentFrame = 0;
        int totalFrames = 7;
        float timePerFrame = 0.07f;
        float elapsedTime = 0;
        float _scale = .5f;
        bool animationIsFlipped = false;

        //textures
        Texture2D _runSpriteSheet;
        Texture2D _idleSpriteSheet;

        public void LoadContent(Game game)
        {
            _runSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerRunning");
            _idleSpriteSheet = game.Content.Load<Texture2D>("My Assets/Player/PlayerIdle");
            _floorCollider = new BoundingRectangle(Vector2.Zero, 40, 40);
            UpdateFloorCollider();
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
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

            if (InputManager.IsHolding)
            {
                spriteBatch.Draw(_runSpriteSheet, Position, sourceRectangle, Color.White * opacity, 0f, Vector2.Zero, _scale, animationIsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.5f);
            }
            else
            {
                spriteBatch.Draw(_idleSpriteSheet, Position, sourceRectangle, Color.White * opacity, 0f, Vector2.Zero, _scale, animationIsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.5f);

            }
        }

        public void Update(GameTime gameTime, List<BoundingRectangle> rectCols = null)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime >= timePerFrame)
            {
                currentFrame++;
                if (currentFrame >= totalFrames)
                {
                    currentFrame = 0;
                }

                elapsedTime = 0;
            }

            timePerFrame = 0.15f;
            if (InputManager.IsHolding)
            {
                timePerFrame = 0.07f;

                CheckPlayerInput();

                PlayerMovement(gameTime, rectCols);
            }
        }

        private void CheckPlayerInput()
        {
            if (InputManager.HoldingLeft) _direction = Direction.Left;
            if (InputManager.HoldingRight) _direction = Direction.Right;
            if (InputManager.HoldingUp) _direction = Direction.Up;
            if (InputManager.HoldingDown) _direction = Direction.Down;
            if (InputManager.HoldingLeft && InputManager.HoldingUp) _direction = Direction.ULeft;
            if (InputManager.HoldingRight && InputManager.HoldingUp) _direction = Direction.URight;
            if (InputManager.HoldingLeft && InputManager.HoldingDown) _direction = Direction.DLeft;
            if (InputManager.HoldingRight && InputManager.HoldingDown) _direction = Direction.DRight;
        }

        private void PlayerMovement(GameTime gameTime, List<BoundingRectangle> rectCols = null)
        {
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
                    if (_floorCollider.CollidesWith(r))
                    {
                        // Check for overlaps
                        float overlapX = Math.Max(0, Math.Max(Position.X + 110 - r.X, r.X + r.Width - (Position.X + 110)));
                        float overlapY = Math.Max(0, Math.Max(Position.Y + 170 - r.Y, r.Y + r.Height - (Position.Y + 170)));

                        if (overlapX < overlapY)
                        {
                            Position = new Vector2(Position.X - Velocity.X, Position.Y);
                        }
                        else
                        {
                            Position = new Vector2(Position.X, Position.Y - Velocity.Y);
                        }

                        UpdateFloorCollider();
                    }
                }
            }
        }

        private void UpdateFloorCollider()
        {
            _floorCollider.X = Position.X + 110;
            _floorCollider.Y = Position.Y + 170;
        }
    }

    public enum Direction
    {
        Down, Right, Up, DRight, URight, Left, DLeft, ULeft
    }
}
