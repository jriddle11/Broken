using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken
{
    public class MainMenu
    {
        private GraphicsDeviceManager _graphics;
        private Texture2D _swordTexture;
        private Texture2D _mainMenuBg;
        private Vector2 _swordPosition;
        private Vector2 _swordVelocity;

        public MainMenu(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
            _swordPosition = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 + 550, graphics.GraphicsDevice.Viewport.Height - 50);
            _swordVelocity = new Vector2(0, -13);
        }

        public void LoadContent(Game game)
        {
            _swordTexture = game.Content.Load<Texture2D>("RuneSwordPixel");
            _mainMenuBg = game.Content.Load<Texture2D>("MainMenuBg");
        }

        public void Update(GameTime gameTime)
        {
            _swordPosition += _swordVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_swordPosition.Y < _graphics.GraphicsDevice.Viewport.Y + 720 || _swordPosition.Y > _graphics.GraphicsDevice.Viewport.Height - 30)
            {
                _swordVelocity.Y *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mainMenuBg, Vector2.Zero, Color.White);
            spriteBatch.Draw(_swordTexture, _swordPosition, null, Color.White, MathHelper.Pi, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
