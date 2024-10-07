using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken.UI
{
    public class TitleCard : IGameObject
    {
        public bool IsActive => _fadingIn || _fadingOut || _onDisplay;

        public bool IsDone = false;

        string _texturePath;
        Texture2D _texture;
        Timer _timer;

        bool _fadingIn;
        bool _fadingOut;
        bool _onDisplay;

        bool FadedIn => _timer.TimesUp && _fadingIn && _onDisplay && !_fadingOut;
        bool DisplayDone => _timer.TimesUp && _onDisplay && !_fadingIn && !_fadingOut;
        bool FadedOut => _timer.TimesUp && _onDisplay && !_fadingIn && _fadingOut;

        bool IsFading => _fadingIn || _fadingOut;

        float _fadeInTime;
        float _fadeOutTime;
        float _displayTime;

        public TitleCard(string texturePath,float fadeInTime, float displayTime, float fadeOutTime)
        {
            _texturePath = texturePath;
            _fadeInTime = fadeInTime;
            _fadeOutTime = fadeOutTime;
            _displayTime = displayTime;
            _timer = new Timer(0f);
        }

        public void Display()
        {
            FadeIn();
        }

        private void FadeIn()
        {
            _onDisplay = true;
            _fadingIn = true;
            _timer = new Timer(_fadeInTime);
        }

        private void DisplayCard()
        {
            _fadingIn = false;
            _timer = new Timer(_displayTime);
        }

        private void FadeOut()
        {
            _fadingOut = true;
            _timer = new Timer(_fadeOutTime);
        }

        private void Finish()
        {
            _fadingOut = false;
            _onDisplay = false;
            IsDone = true;
        }
        

        public void LoadContent(Game game)
        {
            _texture = game.Content.Load<Texture2D>(_texturePath);
        }

        public void Update(GameTime gameTime)
        {
            if (!_onDisplay) return;
            _timer.Update(gameTime);

            if (FadedIn){
                DisplayCard();
            }
            else if (DisplayDone)
            {
                FadeOut();
            }
            else if (FadedOut)
            {
                Finish();
            }
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            if(!_onDisplay) return;
            var color = Color.White;
            if(_fadingIn) color *= 1 - (float)_timer.TimeLeft;
            if (_fadingOut) color *= (float)_timer.TimeLeft;
            var bgScale = new Vector2((float)OutputManager.ScreenWidth / _texture.Width, (float)OutputManager.ScreenHeight / _texture.Height);
            spriteBatch.Draw(_texture, Vector2.Zero, null, color, 0f, Vector2.Zero, bgScale, SpriteEffects.None, 1f);
        }
    }
}
