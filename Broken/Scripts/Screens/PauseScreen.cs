using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.UI;
using Broken.Audio;

namespace Broken
{
    public class PauseScreen
    {
        public bool IsActive;
        public bool Paused;

        public event Action Resume;
        public event Action Pause;
        public event Action Quit;

        Texture2D _pauseMenuBg;
        MenuText[] _menuButtons;
        int _selection = 0;

        public PauseScreen()
        {
            Initialize();
        }

        public void Initialize()
        {
            int screenHeight = OutputManager.ScreenHeight;
            int screenWidth = OutputManager.ScreenWidth;
            Color highlightColor = new Color(99, 215, 210);

            _menuButtons = new MenuText[]
            {
                new MenuText("RESUME", new Vector2(screenWidth / 2, (screenHeight / 2) - 200), Color.WhiteSmoke, highlightColor),
                new MenuText("CONTROLS", new Vector2(screenWidth / 2, (screenHeight / 2) - 90), Color.WhiteSmoke, highlightColor),
                new MenuText("OPTIONS", new Vector2(screenWidth / 2, (screenHeight / 2) + 20), Color.WhiteSmoke, highlightColor),
                new MenuText("QUIT", new Vector2(screenWidth / 2, (screenHeight / 2) + 130), Color.WhiteSmoke, highlightColor)
            };
        }

        public void LoadContent(Game game)
        {
            foreach (var button in _menuButtons) button.LoadContent(game);
            _pauseMenuBg = game.Content.Load<Texture2D>("My Assets/Menu/PauseBg");
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            
            CheckMenuInteractions(gameTime);
        }

        private void CheckMenuInteractions(GameTime gameTime)
        {
            if (InputManager.PressedEscape)
            {
                if (Paused)
                {
                    Resume?.Invoke();
                }
                else
                {
                    Pause?.Invoke();
                }
            }
            if (!Paused) return;

            #region Highlighting Buttons
            //Mouse
            bool isHovering = false;
            ClearButtonHighlights();
            for (int i = 0; i < _menuButtons.Length; ++i)
            {
                if (_menuButtons[i].IsMouseHovering(gameTime))
                {
                    _menuButtons[i].IsHighlighted = true;
                    _menuButtons[i].Scale = 0.85f;
                    _selection = i;
                    isHovering = true;
                    break;
                }
            }
            if (!isHovering)
            {
                _menuButtons[_selection].Scale = 0.85f;
                _menuButtons[_selection].IsHighlighted = true;
            }
            #endregion

            #region Selecting Buttons

            if (InputManager.Mouse1Clicked && _menuButtons[_selection].IsMouseHovering(gameTime))
            {
                HandleSelection();
            }


            #endregion
        }

        private void HandleSelection()
        {
            switch (_selection)
            {
                case 0:
                    //Selected "RESUME"
                    Resume?.Invoke();
                    break;
                case 1:
                    //Selected "CONTROLS"

                    break;
                case 2:
                    //Selected "OPTIONS"

                    break;
                case 3:
                    //Selected "QUIT"
                    Quit?.Invoke();
                    break;
            }
        }

        private void ClearButtonHighlights()
        {
            foreach (var button in _menuButtons)
            {
                button.IsHighlighted = false;
                button.Scale = .75f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1f)
        {
            if (!IsActive || !Paused) return;
            var bgScale = new Vector2((float)OutputManager.ScreenWidth / _pauseMenuBg.Width, (float)OutputManager.ScreenHeight / _pauseMenuBg.Height);
            spriteBatch.Draw(_pauseMenuBg, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, bgScale, SpriteEffects.None, 1f);
            foreach (var button in _menuButtons) button.Draw(spriteBatch, opacity);
        }
    }
}
