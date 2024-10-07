using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Entities;

namespace Broken
{
    public class ExpSystem : IGameObject
    {
        private Texture2D _texture;
        private Experience[] _experiencePool;
        private int _experienceIndex;
        private int _maxExperience;
        private BoundingRectangle _floorCollider = new BoundingRectangle(Vector2.Zero, 40, 40);
        private Vector2 _floorPositionOffset = new Vector2(-20, -10);

        private const int FrameWidth = 712;
        private const int FrameHeight = 712;

        public ExpSystem(int maxExperience = 20)
        {
            _maxExperience = maxExperience;
            _experiencePool = new Experience[_maxExperience];
            _experienceIndex = 0;
        }

        public void LoadContent(Game game)
        {
            _texture = game.Content.Load<Texture2D>("My Assets/XP"); 
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < _experiencePool.Length; i++)
            {
                _experiencePool[i].Update(deltaTime);
            }

        }

        public void GenerateExperience(Vector2 position, float moveDuration = 1f)
        {
            float angle = (float)(RandomHelper.NextDouble() * MathHelper.TwoPi);
            Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * RandomHelper.Next(400, 600);

            if (_experiencePool[_experienceIndex].IsMoving == false)
            {
                _experiencePool[_experienceIndex].Reset(position, velocity, moveDuration);
            }

            _experienceIndex = (_experienceIndex + 1) % _maxExperience;
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            foreach (var experience in _experiencePool)
            {
                if (experience.IsMoving || !experience.IsMoving)
                {
                    Color color = Color.White * opacity;

                    int frameX = experience.CurrentFrame * FrameWidth;
                    Rectangle sourceRect = new Rectangle(frameX, 0, FrameWidth, FrameHeight);

                    _floorCollider.X = experience.Position.X + _floorPositionOffset.X;
                    _floorCollider.Y = experience.Position.Y + _floorPositionOffset.Y;

                    spriteBatch.Draw(
                        _texture,
                        experience.Position,
                        sourceRect,
                        color,
                        0f,
                        new Vector2(FrameWidth / 2, FrameHeight / 2),
                        .1f,
                        SpriteEffects.None,
                        GameScreen.Instance.GetRoomDepth(_floorCollider)
                    );
                }
            }
        }
    }
}
