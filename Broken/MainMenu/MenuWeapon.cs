using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken
{
    public class MenuWeapon : IGameObject
    {
        public Vector2 Position;
        public float Range;

        private GraphicsDeviceManager _graphics;
        private Texture2D _swordTexture;
        private Vector2 _velocity;
        private Vector2 _originalPos;

        public MenuWeapon(GraphicsDeviceManager graphics, Vector2 position, float speed, float range)
        {
            _graphics = graphics;
            Position = position;
            _originalPos = position;
            _velocity = new Vector2(0, speed);
            Range = range;
        }

        public void LoadContent(Game game)
        {
            _swordTexture = game.Content.Load<Texture2D>("My Assets/RuneSwordPixel");
        }

        public void Update(GameTime gameTime)
        {
            Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.Y > _originalPos.Y + Range || Position.Y < _originalPos.Y - Range)
            {
                _velocity.Y *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_swordTexture, Position, null, Color.White, MathHelper.Pi, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
