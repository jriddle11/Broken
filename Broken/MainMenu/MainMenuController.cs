using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Common.ParticleSystem;
using Broken.Common.GameTimer;
using System.Collections.Generic;

namespace Broken
{
    public class MainMenuController
    {
        public int HighlightedOption = 0;
        public bool MenuIsActive = true;

        private GraphicsDeviceManager _graphics;
        private Texture2D _mainMenuBg;
        private Texture2D _titleCard;
        private MenuText[] _menuButtons;
        private List<IGameObject> _staticGameObjects = new();
        private Timer _titleOutTimer;
        private Timer _menuInTimer;
        private bool _openingSequenceIsActive = true;
        private bool _titleCardIsActive = true;

        public MainMenuController(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;

            Color highlightColor = new Color(99, 215, 210);
            Color fogColor = new Color(49, 97, 94);
            int screenWidth = graphics.GraphicsDevice.Viewport.Width;
            int screenHeight = graphics.GraphicsDevice.Viewport.Height;
            var weaponPos = new Vector2( screenWidth - (screenWidth / 6), screenHeight / 2 + 350);
            var fogPos = new Vector2(screenWidth - (screenWidth / 6) - 100, screenHeight / 2);

            _menuButtons = new MenuText[]
            {
                new MenuText("START", new Vector2(400, (screenHeight / 2) - 200), Color.WhiteSmoke, highlightColor),
                new MenuText("SETTINGS", new Vector2(400, (screenHeight / 2) - 90), Color.WhiteSmoke, highlightColor),
                new MenuText("CREDITS", new Vector2(400, (screenHeight / 2) + 20), Color.WhiteSmoke, highlightColor),
                new MenuText("QUIT", new Vector2(400, (screenHeight / 2) + 130), Color.WhiteSmoke, highlightColor)
            };

            _staticGameObjects.Add(new ParticleSystem(fogPos, "My Assets/Fog", fogColor, 0.1f, 50f));
            _staticGameObjects.Add(new MenuWeapon(graphics, weaponPos, -13, 25));
            _titleOutTimer = new Timer(4f);
        }

        public void LoadContent(Game game)
        {
            _mainMenuBg = game.Content.Load<Texture2D>("My Assets/MainMenuBg");
            _titleCard = game.Content.Load<Texture2D>("My Assets/Title");
            foreach (var button in _menuButtons) button.LoadContent(game);
            foreach(var obj in _staticGameObjects) obj.LoadContent(game);
        }

        public void Update(BrokenGame game, GameTime gameTime)
        {
            if(!MenuIsActive) return;
            if (_openingSequenceIsActive)
            {
                if (_titleCardIsActive && _titleOutTimer.TimeIsUp(gameTime))
                {
                    _menuInTimer = new Timer(3f);
                    _titleCardIsActive = false;
                }
                if(_menuInTimer != null)
                {
                    if (_menuInTimer.TimeIsUp(gameTime))
                    {
                        _openingSequenceIsActive = false;
                    }
                }
            }

            foreach (var obj in _staticGameObjects) obj.Update(gameTime);
            if (_openingSequenceIsActive) return;

            #region Highlighting Buttons

            //Keyboard
            if (InputManager.PressedDown) HighlightedOption = (HighlightedOption + 1) % 4;
            if (InputManager.PressedUp)
            {
                HighlightedOption -= 1;
                if(HighlightedOption < 0) HighlightedOption = 3;
            }

            //Mouse
            bool isHovering = false;
            ClearButtonHighlights();
            for(int i = 0; i < _menuButtons.Length; ++i)
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

            #endregion
        }

        private void HandleSelection(BrokenGame game)
        {
            switch (HighlightedOption)
            {
                case 0:
                    //Selected "START"
                    game.HandleStartGame();
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

            var bgScale = new Vector2((float)_graphics.GraphicsDevice.Viewport.Width / _mainMenuBg.Width, (float)_graphics.GraphicsDevice.Viewport.Height / _mainMenuBg.Height);
            
            if (_titleCardIsActive)
            {
                spriteBatch.Draw(_titleCard, Vector2.Zero, null, Color.White * (1f - (float)_titleOutTimer.TimePercentLeft), 0f, Vector2.Zero, bgScale, SpriteEffects.None, 0f);
                return;
            }

            float timeLeft = (float)_menuInTimer.TimePercentLeft;
            spriteBatch.Draw(_mainMenuBg, Vector2.Zero, null, Color.White * timeLeft, 0f, Vector2.Zero, bgScale, SpriteEffects.None, 0f);
            foreach (var obj in _staticGameObjects) obj.Draw(spriteBatch, timeLeft);
            foreach(var button in _menuButtons) button.Draw(spriteBatch, timeLeft);
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
