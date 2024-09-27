﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Broken.Scripts;
using System.Collections.Generic;

namespace Broken
{
    /// <summary>
    /// Main menu controller class
    /// </summary>
    public class MainMenuController
    {
        public int HighlightedOption = 0;
        public bool MenuIsActive = true;

        Texture2D _mainMenuBg;
        Texture2D _titleCard;
        MenuText[] _menuButtons;
        List<IGameObject> _staticGameObjects = new();
        float _titleShownTime = 1f; //2f
        float _menuFadeInTime = 1f; //2f
        Timer _titleOutTimer;
        Timer _menuInTimer;
        bool _openingSequenceIsActive = true;
        bool _titleCardIsActive = true;
        Song _mainTheme;

        public MainMenuController()
        {
            Color highlightColor = new Color(99, 215, 210);
            Color fogColor = new Color(49, 97, 94);
            int screenWidth = OutputManager.ScreenWidth;
            int screenHeight = OutputManager.ScreenHeight;
            var weaponPos = new Vector2( 1320, screenHeight / 2 - 450);
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
            _titleOutTimer = new Timer(_titleShownTime);
        }

        public void LoadContent(Game game)
        {
            _mainMenuBg = game.Content.Load<Texture2D>("My Assets/Menu/MainMenuBg");
            _titleCard = game.Content.Load<Texture2D>("My Assets/Menu/Title");
            _mainTheme = game.Content.Load<Song>("External Assets/Music/mainMenuTheme");
            foreach (var button in _menuButtons) button.LoadContent(game);
            foreach(var obj in _staticGameObjects) obj.LoadContent(game);
        }

        public void Update(BrokenGame game, GameTime gameTime)
        {
            if (!MenuIsActive) return;
            if (_openingSequenceIsActive)
            {
                if (_titleCardIsActive && _titleOutTimer.TimeIsUp(gameTime))
                {
                    _menuInTimer = new Timer(_menuFadeInTime);
                    _titleCardIsActive = false;
                }
                if (_menuInTimer != null)
                {
                    if (_menuInTimer.TimeIsUp(gameTime))
                    {
                        _openingSequenceIsActive = false;
                    }
                }
            }

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.IsRepeating = true; // Set to loop
                MediaPlayer.Play(_mainTheme); // Play the song
            }

            foreach (var obj in _staticGameObjects) obj.Update(gameTime);
            if (_openingSequenceIsActive) return;

            #region Highlighting Buttons

            //Keyboard
            if (InputManager.PressedDown) HighlightedOption = (HighlightedOption + 1) % 4;
            if (InputManager.PressedUp)
            {
                HighlightedOption -= 1;
                if (HighlightedOption < 0) HighlightedOption = 3;
            }

            //Mouse
            bool isHovering = false;
            ClearButtonHighlights();
            for (int i = 0; i < _menuButtons.Length; ++i)
            {
                if (_menuButtons[i].IsMouseHovering(gameTime))
                {
                    _menuButtons[i].IsHighlighted = true;
                    _menuButtons[i].Scale = 1.1f;
                    HighlightedOption = i;
                    isHovering = true;
                    break;
                }
            }
            if (!isHovering)
            {
                _menuButtons[HighlightedOption].Scale = 1.1f;
                _menuButtons[HighlightedOption].IsHighlighted = true;
            }
            #endregion

            #region Selecting Buttons

            if (InputManager.Mouse1Clicked && _menuButtons[HighlightedOption].IsMouseHovering(gameTime))
            {
                HandleSelection(game);
            }

            if (InputManager.PressedSpace && _titleCardIsActive == false)
            {
                HandleSelection(game);
            }


            #endregion
        }

        private void HandleSelection(BrokenGame game)
        {
            switch (HighlightedOption)
            {
                case 0:
                    //Selected "START"
                    game.HandleStartGame();
                    MediaPlayer.Stop();
                    break;
                case 1:
                    //Selected "SETTINGS"

                    break;
                case 2:
                    //Selected "CREDITS"

                    break;
                case 3:
                    //Selected "QUIT"
                    game.Exit();
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!MenuIsActive) return;

            var bgScale = new Vector2((float)OutputManager.ScreenWidth / _mainMenuBg.Width, (float)OutputManager.ScreenHeight / _mainMenuBg.Height);
            
            if (_titleCardIsActive)
            {
                spriteBatch.Draw(_titleCard, Vector2.Zero, null, Color.White * (1f - (float)_titleOutTimer.TimePercentLeft), 0f, Vector2.Zero, bgScale, SpriteEffects.None, 0f);
                return;
            }

            float timeLeft = (float)_menuInTimer.TimePercentLeft;
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
