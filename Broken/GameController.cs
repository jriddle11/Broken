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

            spriteBatch.Draw(_placeholderTexture, Vector2.Zero, Color.White);
        }
    }
}
