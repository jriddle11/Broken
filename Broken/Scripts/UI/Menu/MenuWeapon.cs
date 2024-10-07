using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Broken.UI
{
    /// <summary>
    /// Weapon to float on the main menu
    /// </summary>
    public class MenuWeapon : IGameObject
    {
        public Vector2 Position;
        public float Range;

        Texture2D _swordTexture;
        Vector2 _velocity;
        Vector2 _originalPos;

        public MenuWeapon(Vector2 position, float speed, float range)
        {
            Position = position;
            _originalPos = position;
            _velocity = new Vector2(0, speed);
            Range = range;
        }

        public void LoadContent(Game game)
        {
            _swordTexture = game.Content.Load<Texture2D>("My Assets/Menu/RuneSword");
        }

        public void Update(GameTime gameTime)
        {
            Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.Y > _originalPos.Y + Range || Position.Y < _originalPos.Y - Range)
            {
                _velocity.Y *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1f)
        {
            spriteBatch.Draw(_swordTexture, Position, null, new Color(221, 252, 255) * Math.Clamp(opacity * 3,0, 1), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
