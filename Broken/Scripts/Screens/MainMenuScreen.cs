using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Broken.UI;
using Broken.Audio;
using System;

namespace Broken
{
    /// <summary>
    /// Main menu controller class
    /// </summary>
    public class MainMenuScreen
    {
        public bool IsActive = true;

        public event Action StartGame;
        public event Action Quit;

        Texture2D _mainMenuBg;
        MenuText[] _menuButtons;
        List<IGameObject> _staticGameObjects = new();
        TitleCardSequence _titleCardSequence = new();

        Timer _timer;
        bool _openingSequenceIsActive = true;
        bool _menuTimerStarted = false;
        int _selection = 0;

        //constants
        //13 seconds until start
        const float TITLE_HANG_TIME = 2f;
        const float TITLE_FADE_TIME = 3f;
        const float KEY_FADE_IN_TIME = 1f;
        const float KEY_HANG_TIME = 2f;
        const float KEY_FADE_OUT_TIME = 2f;
        const float MENU_FADE_IN_TIME = 3f;

        public MainMenuScreen()
        {
            Initialize();
        }

        private void Initialize()
        {
            Color highlightColor = new Color(99, 215, 210);
            Color fogColor = new Color(49, 97, 94);
            int screenHeight = OutputManager.ScreenHeight;
            var weaponPos = new Vector2(1320, screenHeight / 2 - 450);
            var fogPos = new Vector2(1480, screenHeight / 2);

            _menuButtons = new MenuText[]
            {
                new MenuText("START", new Vector2(500, (screenHeight / 2) - 200), Color.WhiteSmoke, highlightColor),
                new MenuText("SETTINGS", new Vector2(500, (screenHeight / 2) - 90), Color.WhiteSmoke, highlightColor),
                new MenuText("CREDITS", new Vector2(500, (screenHeight / 2) + 20), Color.WhiteSmoke, highlightColor),
                new MenuText("QUIT", new Vector2(500, (screenHeight / 2) + 130), Color.WhiteSmoke, highlightColor)
            };

            _staticGameObjects.Add(new ParticleSystem(fogPos, "My Assets/Menu/Fog", fogColor, 0.1f, 50f));
            _staticGameObjects.Add(new MenuWeapon(weaponPos, -13, 25));

            _titleCardSequence.Add(new("My Assets/Menu/Title", 0f, TITLE_HANG_TIME, TITLE_FADE_TIME));
            _titleCardSequence.Add(new("My Assets/Menu/MouseAndKey", KEY_FADE_IN_TIME, KEY_HANG_TIME, KEY_FADE_OUT_TIME));
        }

        public void LoadContent(Game game)
        {
            _mainMenuBg = game.Content.Load<Texture2D>("My Assets/Menu/MainMenuBg");
            foreach (var button in _menuButtons) button.LoadContent(game);
            foreach(var obj in _staticGameObjects) obj.LoadContent(game);
            _titleCardSequence.LoadContent(game);
            _titleCardSequence.Play();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            if (!MusicPlayer.IsPlayingSong("WorthyAdversary"))
            {
                MusicPlayer.Play("WorthyAdversary", true, .5f);
            }
            
            if (!_titleCardSequence.IsDone)
            {
                _titleCardSequence.Update(gameTime);
                return;
            }
            else if(_titleCardSequence.IsDone && !_menuTimerStarted && _openingSequenceIsActive)
            {
                _menuTimerStarted = true;
                _timer = new Timer(MENU_FADE_IN_TIME);
            }
            else if(_menuTimerStarted && _openingSequenceIsActive)
            {
                if (_timer.TimeIsUp(gameTime))
                {
                    _openingSequenceIsActive = false;
                }
            }

            foreach (var obj in _staticGameObjects) obj.Update(gameTime);
            if (_openingSequenceIsActive) return;

            CheckMenuInteractions(gameTime);
        }

        private void CheckMenuInteractions(GameTime gameTime)
        {
            #region Highlighting Buttons

            //Mouse
            bool isHovering = false;
            ClearButtonHighlights();
            for (int i = 0; i < _menuButtons.Length; ++i)
            {
                if (_menuButtons[i].IsMouseHovering(gameTime))
                {
                    _menuButtons[i].IsHighlighted = true;
                    _menuButtons[i].Scale = 1.1f;
                    _selection = i;
                    isHovering = true;
                    break;
                }
            }
            if (!isHovering)
            {
                _menuButtons[_selection].Scale = 1.1f;
                _menuButtons[_selection].IsHighlighted = true;
            }
            #endregion

            #region Selecting Buttons

            if (InputManager.Mouse1Clicked && _menuButtons[_selection].IsMouseHovering(gameTime))
            {
                HandleSelection();
            }

            if (InputManager.PressedSpace && !_openingSequenceIsActive)
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
                    //Selected "START"
                    MusicPlayer.SlowStop(1.5f);
                    StartGame?.Invoke();
                    break;
                case 1:
                    //Selected "SETTINGS"

                    break;
                case 2:
                    //Selected "CREDITS"

                    break;
                case 3:
                    //Selected "QUIT"
                    Quit?.Invoke();
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive) return;

            if (!_titleCardSequence.IsDone)
            {
                _titleCardSequence.Draw(spriteBatch, 1);
                return;
            }

            if (!_menuTimerStarted) return;
            var bgScale = new Vector2((float)OutputManager.ScreenWidth / _mainMenuBg.Width, (float)OutputManager.ScreenHeight / _mainMenuBg.Height);
            var timeLeft = (float)_timer.TimePercentLeft;
            spriteBatch.Draw(_mainMenuBg, Vector2.Zero, null, Color.White * timeLeft, 0f, Vector2.Zero, bgScale, SpriteEffects.None, 1f);
            foreach (var obj in _staticGameObjects) obj.Draw(spriteBatch, timeLeft);
            if (_openingSequenceIsActive) return;
            foreach (var button in _menuButtons) button.Draw(spriteBatch, timeLeft);
        }

        private void ClearButtonHighlights()
        {
            foreach (var button in _menuButtons)
            {
                button.IsHighlighted = false;
                button.Scale = 1f;
            }
        }
    }
}
