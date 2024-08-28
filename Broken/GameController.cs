using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken
{
    public class GameController
    {

        private bool _isActive;
        private Texture2D _placeholderTexture;
        private GraphicsDeviceManager _graphics;

        public GameController(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        public void LoadContent(Game game)
        {
            _placeholderTexture = game.Content.Load<Texture2D>("My Assets/Placeholder");
        }

        public void StartGame()
        {
            _isActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isActive) return;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!_isActive) return;
            var bgScale = new Vector2((float)_graphics.GraphicsDevice.Viewport.Width / _placeholderTexture.Width, (float)_graphics.GraphicsDevice.Viewport.Height / _placeholderTexture.Height);
            spriteBatch.Draw(_placeholderTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, bgScale, SpriteEffects.None, 0f);
        }
    }
}
